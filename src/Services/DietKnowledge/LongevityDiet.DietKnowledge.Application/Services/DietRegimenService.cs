using System.ComponentModel.DataAnnotations;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Common;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;

namespace LongevityDiet.DietKnowledge.Application.Services;

public interface IDietRegimenService
{
    Task<PagedResult<DietRegimenModel>> ListAsync(
        DietRegimenQuery query,
        CancellationToken cancellationToken = default);

    Task<DietRegimenModel> GetAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    Task<DietRegimenModel> CreateAsync(
        SaveDietRegimenCommand command,
        CancellationToken cancellationToken = default);

    Task<DietRegimenModel> UpdateAsync(
        Guid id,
        SaveDietRegimenCommand command,
        CancellationToken cancellationToken = default);

    Task<DietRegimenModel> SetActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default);
}

public sealed class DietRegimenService(IDietRegimenRepository repository) : IDietRegimenService
{
    private static readonly string[] MealSlots = ["Breakfast", "Lunch", "Dinner", "Snack"];

    public async Task<PagedResult<DietRegimenModel>> ListAsync(
        DietRegimenQuery query,
        CancellationToken cancellationToken = default)
    {
        var normalized = query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Search = string.IsNullOrWhiteSpace(query.Search) ? null : query.Search.Trim(),
            ReviewStatus = NormalizeStatusFilter(query.ReviewStatus)
        };

        var result = await repository.ListAsync(normalized, cancellationToken);
        return new PagedResult<DietRegimenModel>(
            result.Items.Select(Map).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<DietRegimenModel> GetAsync(
        Guid id,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var regimen = await repository.GetByIdAsync(
                id,
                includeInactive,
                approvedOnly: !includeInactive,
                cancellationToken)
            ?? throw new KeyNotFoundException("Diet regimen was not found.");

        return Map(regimen);
    }

    public async Task<DietRegimenModel> CreateAsync(
        SaveDietRegimenCommand command,
        CancellationToken cancellationToken = default)
    {
        var name = Clean(command.Name);
        if (await repository.NameExistsAsync(name, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("A diet regimen with this name already exists.");
        }

        var status = ValidateStatus(command);
        await ValidateChildrenAsync(command, cancellationToken);

        var regimen = new DietRegimen
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = Clean(command.Description),
            DurationDays = command.DurationDays,
            SourceTitle = CleanOrDefault(command.SourceTitle, "The Longevity Diet"),
            SourceChapter = Optional(command.SourceChapter),
            SourcePage = Optional(command.SourcePage),
            ReviewStatus = status,
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        repository.ReplaceChildren(regimen, MapGuidelines(command), MapMeals(command));
        await repository.AddAsync(regimen, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return await GetAsync(regimen.Id, includeInactive: true, cancellationToken);
    }

    public async Task<DietRegimenModel> UpdateAsync(
        Guid id,
        SaveDietRegimenCommand command,
        CancellationToken cancellationToken = default)
    {
        var regimen = await repository.GetByIdAsync(
                id,
                includeInactive: true,
                approvedOnly: false,
                cancellationToken)
            ?? throw new KeyNotFoundException("Diet regimen was not found.");

        var name = Clean(command.Name);
        if (await repository.NameExistsAsync(name, id, cancellationToken))
        {
            throw new InvalidOperationException("A diet regimen with this name already exists.");
        }

        var status = ValidateStatus(command);
        await ValidateChildrenAsync(command, cancellationToken);

        regimen.Name = name;
        regimen.Description = Clean(command.Description);
        regimen.DurationDays = command.DurationDays;
        regimen.SourceTitle = CleanOrDefault(command.SourceTitle, "The Longevity Diet");
        regimen.SourceChapter = Optional(command.SourceChapter);
        regimen.SourcePage = Optional(command.SourcePage);
        regimen.ReviewStatus = status;
        regimen.UpdatedAtUtc = DateTimeOffset.UtcNow;

        repository.ReplaceChildren(regimen, MapGuidelines(command), MapMeals(command));
        await repository.SaveChangesAsync(cancellationToken);

        return await GetAsync(regimen.Id, includeInactive: true, cancellationToken);
    }

    public async Task<DietRegimenModel> SetActivationAsync(
        Guid id,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var regimen = await repository.GetByIdAsync(
                id,
                includeInactive: true,
                approvedOnly: false,
                cancellationToken)
            ?? throw new KeyNotFoundException("Diet regimen was not found.");

        regimen.IsActive = isActive;
        regimen.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await repository.SaveChangesAsync(cancellationToken);

        return Map(regimen);
    }

    private async Task ValidateChildrenAsync(
        SaveDietRegimenCommand command,
        CancellationToken cancellationToken)
    {
        if (command.DurationDays < 1 || command.DurationDays > 365)
        {
            throw new ValidationException("DurationDays must be between 1 and 365.");
        }

        var guidelineIds = command.Guidelines.Select(link => link.GuidelineId).Distinct().ToArray();
        if (guidelineIds.Length != command.Guidelines.Count)
        {
            throw new ValidationException("A regimen cannot link the same guideline more than once.");
        }

        var recipeIds = command.Meals.Select(meal => meal.RecipeId).Distinct().ToArray();

        foreach (var meal in command.Meals)
        {
            if (meal.DayNumber < 1 || meal.DayNumber > command.DurationDays)
            {
                throw new ValidationException("Meal DayNumber must be within the regimen duration.");
            }

            if (!MealSlots.Contains(meal.MealSlot, StringComparer.OrdinalIgnoreCase))
            {
                throw new ValidationException("MealSlot must be Breakfast, Lunch, Dinner, or Snack.");
            }
        }

        if (command.Meals
            .GroupBy(meal => (meal.DayNumber, Slot: meal.MealSlot.ToLowerInvariant(), meal.RecipeId))
            .Any(group => group.Count() > 1))
        {
            throw new ValidationException("The same recipe cannot be added twice to the same day and meal slot.");
        }

        if (guidelineIds.Length > 0 &&
            await repository.CountGuidelinesAsync(guidelineIds, cancellationToken) != guidelineIds.Length)
        {
            throw new ValidationException("All regimen guidelines must reference active approved guidelines.");
        }

        if (recipeIds.Length > 0 &&
            await repository.CountRecipesAsync(recipeIds, cancellationToken) != recipeIds.Length)
        {
            throw new ValidationException("All regimen meals must reference active approved recipes.");
        }
    }

    private static string ValidateStatus(SaveDietRegimenCommand command)
    {
        if (!KnowledgeReviewStatuses.IsValid(command.ReviewStatus))
        {
            throw new ValidationException("ReviewStatus must be NeedsReview, Approved, or Rejected.");
        }

        var status = KnowledgeReviewStatuses.Normalize(command.ReviewStatus);
        if (status == KnowledgeReviewStatuses.Approved &&
            (string.IsNullOrWhiteSpace(command.SourceChapter) ||
             (string.IsNullOrWhiteSpace(command.SourcePage))))
        {
            throw new ValidationException("Approved regimens must include SourceChapter and SourcePage.");
        }

        return status;
    }

    private static string NormalizeStatusFilter(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return string.Empty;
        }

        if (!KnowledgeReviewStatuses.IsValid(status))
        {
            throw new ValidationException("ReviewStatus must be NeedsReview, Approved, or Rejected.");
        }

        return KnowledgeReviewStatuses.Normalize(status);
    }

    private static IReadOnlyCollection<DietRegimenGuideline> MapGuidelines(SaveDietRegimenCommand command)
    {
        return command.Guidelines.Select(link => new DietRegimenGuideline
        {
            Id = Guid.NewGuid(),
            GuidelineId = link.GuidelineId,
            SortOrder = link.SortOrder,
            Note = Optional(link.Note)
        }).ToArray();
    }

    private static IReadOnlyCollection<DietRegimenMeal> MapMeals(SaveDietRegimenCommand command)
    {
        return command.Meals.Select(meal => new DietRegimenMeal
        {
            Id = Guid.NewGuid(),
            DayNumber = meal.DayNumber,
            MealSlot = MealSlots.First(slot => string.Equals(slot, meal.MealSlot, StringComparison.OrdinalIgnoreCase)),
            RecipeId = meal.RecipeId,
            Notes = Optional(meal.Notes)
        }).ToArray();
    }

    private static DietRegimenModel Map(DietRegimen regimen)
    {
        return new DietRegimenModel(
            regimen.Id,
            regimen.Name,
            regimen.Description,
            regimen.DurationDays,
            regimen.SourceTitle,
            regimen.SourceChapter,
            regimen.SourcePage,
            regimen.ReviewStatus,
            regimen.IsActive,
            regimen.Guidelines
                .OrderBy(link => link.SortOrder)
                .Select(link => new DietRegimenGuidelineModel(
                    link.Id,
                    link.GuidelineId,
                    link.Guideline?.Title,
                    link.SortOrder,
                    link.Note))
                .ToArray(),
            regimen.Meals
                .OrderBy(meal => meal.DayNumber)
                .ThenBy(meal => Array.IndexOf(MealSlots, meal.MealSlot))
                .Select(meal => new DietRegimenMealModel(
                    meal.Id,
                    meal.DayNumber,
                    meal.MealSlot,
                    meal.RecipeId,
                    meal.Recipe?.Name,
                    meal.Notes))
                .ToArray(),
            regimen.CreatedAtUtc,
            regimen.UpdatedAtUtc);
    }

    private static string Clean(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException("A required value is missing.");
        }

        return value.Trim();
    }

    private static string CleanOrDefault(string? value, string fallback)
    {
        return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    }

    private static string Optional(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
}
