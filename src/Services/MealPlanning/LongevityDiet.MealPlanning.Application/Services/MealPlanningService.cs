using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using LongevityDiet.Contracts.Messaging;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Common;
using LongevityDiet.MealPlanning.Application.Models;
using LongevityDiet.MealPlanning.Domain.Entities;

namespace LongevityDiet.MealPlanning.Application.Services;

public sealed class MealPlanningService(
    IMealPlanRepository repository,
    IDietKnowledgeCatalogClient dietKnowledgeCatalogClient,
    IReminderPublisher reminderPublisher,
    IRecommendationRequestPublisher recommendationRequestPublisher) : IMealPlanningService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public MealPlanningServiceStatus GetStatus()
    {
        return new MealPlanningServiceStatus(
            "Meal Planning Service",
            "MealPlanningDb",
            ["MealPlan", "MealPlanItem", "MealRecommendationRequest"],
            [
                "Diet Knowledge REST lookup",
                "RabbitMQ reminder publisher",
                "RabbitMQ recommendation request publisher",
                "RabbitMQ recommendation result consumer"
            ]);
    }

    public async Task<PagedResult<MealPlanModel>> ListMealPlansAsync(
        Guid userId,
        MealPlanQuery query,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);

        var result = await repository.ListMealPlansAsync(
            userId,
            Normalize(query),
            cancellationToken);

        return MapPaged(result, MapMealPlan);
    }

    public async Task<MealPlanModel> GetMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var mealPlan = await GetOwnedMealPlanAsync(mealPlanId, userId, cancellationToken);
        return MapMealPlan(mealPlan);
    }

    public async Task<MealPlanModel> CreateMealPlanAsync(
        CreateMealPlanCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var userEmail = CleanEmail(command.UserEmail);
        ValidatePlanDates(command.StartDate, command.EndDate);

        var mealPlan = new MealPlan
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Name = CleanRequired(command.Name, "Meal plan name is required."),
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        foreach (var itemCommand in command.Items)
        {
            mealPlan.Items.Add(await CreateItemAsync(
                itemCommand,
                mealPlan.StartDate,
                mealPlan.EndDate,
                cancellationToken));
        }

        await repository.AddMealPlanAsync(mealPlan, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        await PublishReminderMessagesAsync(mealPlan, mealPlan.Items, userEmail, cancellationToken);

        return MapMealPlan(mealPlan);
    }

    public async Task<MealPlanModel> UpdateMealPlanAsync(
        Guid mealPlanId,
        UpdateMealPlanCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        CleanEmail(command.UserEmail);
        ValidatePlanDates(command.StartDate, command.EndDate);

        var mealPlan = await GetOwnedMealPlanAsync(mealPlanId, command.UserId, cancellationToken);
        if (mealPlan.Items.Any(item =>
                item.PlannedDate < command.StartDate || item.PlannedDate > command.EndDate))
        {
            throw new ValidationException(
                "The updated date range must still contain all existing meal plan items.");
        }

        mealPlan.Name = CleanRequired(command.Name, "Meal plan name is required.");
        mealPlan.StartDate = command.StartDate;
        mealPlan.EndDate = command.EndDate;
        mealPlan.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return MapMealPlan(mealPlan);
    }

    public async Task DeleteMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var mealPlan = await GetOwnedMealPlanAsync(mealPlanId, userId, cancellationToken);
        repository.RemoveMealPlan(mealPlan);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<MealPlanItemModel> AddMealPlanItemAsync(
        Guid mealPlanId,
        AddMealPlanItemCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var userEmail = CleanEmail(command.UserEmail);
        var mealPlan = await GetOwnedMealPlanAsync(mealPlanId, command.UserId, cancellationToken);

        var item = await CreateItemAsync(
            new CreateMealPlanItemCommand(
                command.PlannedDate,
                command.MealSlot,
                command.RecipeId,
                command.FoodId,
                command.Notes,
                command.ReminderAtUtc),
            mealPlan.StartDate,
            mealPlan.EndDate,
            cancellationToken);

        mealPlan.Items.Add(item);
        mealPlan.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        await PublishReminderMessagesAsync(mealPlan, [item], userEmail, cancellationToken);

        return MapMealPlanItem(item);
    }

    public async Task<MealPlanItemModel> UpdateMealPlanItemAsync(
        Guid mealPlanId,
        Guid itemId,
        UpdateMealPlanItemCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var userEmail = CleanEmail(command.UserEmail);
        var mealPlan = await GetOwnedMealPlanAsync(mealPlanId, command.UserId, cancellationToken);
        var item = mealPlan.Items.FirstOrDefault(existing => existing.Id == itemId)
            ?? throw new KeyNotFoundException("Meal plan item was not found.");

        var replacement = await CreateItemAsync(
            new CreateMealPlanItemCommand(
                command.PlannedDate,
                command.MealSlot,
                command.RecipeId,
                command.FoodId,
                command.Notes,
                command.ReminderAtUtc),
            mealPlan.StartDate,
            mealPlan.EndDate,
            cancellationToken);

        item.PlannedDate = replacement.PlannedDate;
        item.MealSlot = replacement.MealSlot;
        item.RecipeId = replacement.RecipeId;
        item.FoodId = replacement.FoodId;
        item.Notes = replacement.Notes;
        item.ReminderAtUtc = replacement.ReminderAtUtc;
        item.UpdatedAtUtc = DateTimeOffset.UtcNow;
        mealPlan.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        await PublishReminderMessagesAsync(mealPlan, [item], userEmail, cancellationToken);

        return MapMealPlanItem(item);
    }

    public async Task DeleteMealPlanItemAsync(
        Guid mealPlanId,
        Guid itemId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var mealPlan = await GetOwnedMealPlanAsync(mealPlanId, userId, cancellationToken);
        var item = mealPlan.Items.FirstOrDefault(existing => existing.Id == itemId)
            ?? throw new KeyNotFoundException("Meal plan item was not found.");

        repository.RemoveMealPlanItem(item);
        mealPlan.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<RecommendationRequestModel> RequestRecommendationAsync(
        CreateRecommendationRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var days = command.Days;
        if (days is < 1 or > 14)
        {
            throw new ValidationException("Recommendation days must be between 1 and 14.");
        }

        var preferenceTags = CleanTags(command.PreferenceTags);
        var requestedAtUtc = DateTimeOffset.UtcNow;
        var request = new MealRecommendationRequest
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            PreferenceTagsJson = SerializeTags(preferenceTags),
            Days = days,
            Status = RecommendationRequestStatuses.Pending,
            RequestedAtUtc = requestedAtUtc
        };

        await repository.AddRecommendationRequestAsync(request, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await recommendationRequestPublisher.PublishAsync(
            new RecommendationRequestMessage(
                request.Id,
                command.UserId,
                preferenceTags,
                days,
                requestedAtUtc),
            cancellationToken);

        return MapRecommendationRequest(request);
    }

    public async Task<PagedResult<RecommendationRequestModel>> ListRecommendationRequestsAsync(
        Guid userId,
        RecommendationRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(userId);
        var result = await repository.ListRecommendationRequestsAsync(
            userId,
            Normalize(query),
            cancellationToken);

        return MapPaged(result, MapRecommendationRequest);
    }

    public async Task<RecommendationRequestModel> GetRecommendationRequestAsync(
        Guid requestId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var request = await repository.GetRecommendationRequestAsync(
                requestId,
                userId,
                cancellationToken)
            ?? throw new KeyNotFoundException("Recommendation request was not found.");

        return MapRecommendationRequest(request);
    }

    public async Task<MealPlanModel> AcceptRecommendationAsync(
        Guid requestId,
        AcceptRecommendationCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureUser(command.UserId);
        var userEmail = CleanEmail(command.UserEmail);

        var request = await repository.GetRecommendationRequestAsync(
                requestId,
                command.UserId,
                cancellationToken)
            ?? throw new KeyNotFoundException("Recommendation request was not found.");

        if (request.Status != RecommendationRequestStatuses.Completed)
        {
            throw new ValidationException("Only completed recommendations can be accepted.");
        }

        var mealPlan = await BuildMealPlanFromAcceptedRecommendationAsync(
            command,
            cancellationToken);

        request.Status = RecommendationRequestStatuses.Accepted;
        request.AcceptedMealPlanId = mealPlan.Id;
        request.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await repository.AddMealPlanAsync(mealPlan, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        await PublishReminderMessagesAsync(mealPlan, mealPlan.Items, userEmail, cancellationToken);

        return MapMealPlan(mealPlan);
    }

    private async Task<MealPlan> BuildMealPlanFromAcceptedRecommendationAsync(
        AcceptRecommendationCommand command,
        CancellationToken cancellationToken)
    {
        ValidatePlanDates(command.StartDate, command.EndDate);

        var mealPlan = new MealPlan
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Name = CleanRequired(command.Name, "Meal plan name is required."),
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        foreach (var itemCommand in command.Items)
        {
            mealPlan.Items.Add(await CreateItemAsync(
                itemCommand,
                mealPlan.StartDate,
                mealPlan.EndDate,
                cancellationToken));
        }

        return mealPlan;
    }

    private async Task<MealPlan> GetOwnedMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        EnsureUser(userId);

        return await repository.GetMealPlanAsync(mealPlanId, userId, cancellationToken)
            ?? throw new KeyNotFoundException("Meal plan was not found.");
    }

    private async Task<MealPlanItem> CreateItemAsync(
        CreateMealPlanItemCommand command,
        DateOnly planStartDate,
        DateOnly planEndDate,
        CancellationToken cancellationToken)
    {
        if (command.PlannedDate < planStartDate || command.PlannedDate > planEndDate)
        {
            throw new ValidationException("Meal item planned date must be inside the meal plan date range.");
        }

        var mealSlot = CleanRequired(command.MealSlot, "Meal slot is required.");
        if (!MealSlots.IsValid(mealSlot))
        {
            throw new ValidationException(
                $"Meal slot must be one of: {string.Join(", ", MealSlots.All)}.");
        }

        mealSlot = MealSlots.Normalize(mealSlot);
        await ValidateCatalogReferenceAsync(command.FoodId, command.RecipeId, cancellationToken);

        return new MealPlanItem
        {
            Id = Guid.NewGuid(),
            PlannedDate = command.PlannedDate,
            MealSlot = mealSlot,
            RecipeId = command.RecipeId,
            FoodId = command.FoodId,
            Notes = CleanOptional(command.Notes),
            ReminderAtUtc = command.ReminderAtUtc ??
                BuildDefaultReminderAtUtc(command.PlannedDate, mealSlot),
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private async Task ValidateCatalogReferenceAsync(
        Guid? foodId,
        Guid? recipeId,
        CancellationToken cancellationToken)
    {
        var hasFood = foodId.HasValue && foodId.Value != Guid.Empty;
        var hasRecipe = recipeId.HasValue && recipeId.Value != Guid.Empty;

        if (hasFood == hasRecipe)
        {
            throw new ValidationException("Meal plan item must reference exactly one approved food or recipe.");
        }

        if (hasFood &&
            !await dietKnowledgeCatalogClient.FoodExistsAsync(foodId!.Value, cancellationToken))
        {
            throw new ValidationException("Meal plan item references a food that is missing or not approved.");
        }

        if (hasRecipe &&
            !await dietKnowledgeCatalogClient.RecipeExistsAsync(recipeId!.Value, cancellationToken))
        {
            throw new ValidationException("Meal plan item references a recipe that is missing or not approved.");
        }
    }

    private async Task PublishReminderMessagesAsync(
        MealPlan mealPlan,
        IEnumerable<MealPlanItem> items,
        string userEmail,
        CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            await reminderPublisher.PublishAsync(
                new ReminderRequestedMessage(
                    Guid.NewGuid(),
                    mealPlan.UserId,
                    mealPlan.Id,
                    userEmail,
                    item.ReminderAtUtc,
                    $"Meal reminder: {item.MealSlot}",
                    $"Your meal '{mealPlan.Name}' is scheduled for {item.PlannedDate:yyyy-MM-dd} ({item.MealSlot})."),
                cancellationToken);
        }
    }

    private static MealPlanModel MapMealPlan(MealPlan mealPlan)
    {
        return new MealPlanModel(
            mealPlan.Id,
            mealPlan.UserId,
            mealPlan.Name,
            mealPlan.StartDate,
            mealPlan.EndDate,
            mealPlan.Items
                .OrderBy(item => item.PlannedDate)
                .ThenBy(item => MealSlots.All.ToList().IndexOf(item.MealSlot))
                .Select(MapMealPlanItem)
                .ToArray(),
            mealPlan.CreatedAtUtc,
            mealPlan.UpdatedAtUtc);
    }

    private static MealPlanItemModel MapMealPlanItem(MealPlanItem item)
    {
        return new MealPlanItemModel(
            item.Id,
            item.PlannedDate,
            item.MealSlot,
            item.RecipeId,
            item.FoodId,
            item.Notes,
            item.ReminderAtUtc,
            item.CreatedAtUtc,
            item.UpdatedAtUtc);
    }

    private static RecommendationRequestModel MapRecommendationRequest(
        MealRecommendationRequest request)
    {
        return new RecommendationRequestModel(
            request.Id,
            request.UserId,
            DeserializeTags(request.PreferenceTagsJson),
            request.Days,
            request.Status,
            DeserializeTags(request.SuggestedMealTitlesJson),
            request.Disclaimer,
            request.AcceptedMealPlanId,
            request.RequestedAtUtc,
            request.CompletedAtUtc,
            request.UpdatedAtUtc);
    }

    private static PagedResult<TDestination> MapPaged<TSource, TDestination>(
        PagedResult<TSource> result,
        Func<TSource, TDestination> mapper)
    {
        return new PagedResult<TDestination>(
            result.Items.Select(mapper).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    private static MealPlanQuery Normalize(MealPlanQuery query)
    {
        if (query.FromDate.HasValue &&
            query.ToDate.HasValue &&
            query.FromDate.Value > query.ToDate.Value)
        {
            throw new ValidationException("FromDate cannot be after ToDate.");
        }

        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Search = CleanOptional(query.Search),
            SortBy = CleanOptional(query.SortBy),
            SortDirection = CleanOptional(query.SortDirection)
        };
    }

    private static RecommendationRequestQuery Normalize(RecommendationRequestQuery query)
    {
        var status = CleanOptional(query.Status);
        if (!string.IsNullOrWhiteSpace(status) &&
            status != RecommendationRequestStatuses.Pending &&
            status != RecommendationRequestStatuses.Completed &&
            status != RecommendationRequestStatuses.Accepted)
        {
            throw new ValidationException("Recommendation status must be Pending, Completed, or Accepted.");
        }

        return query with
        {
            PageNumber = Math.Max(1, query.PageNumber),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            Status = status
        };
    }

    private static void ValidatePlanDates(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
        {
            throw new ValidationException("Meal plan start date cannot be after end date.");
        }
    }

    private static void EnsureUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException();
        }
    }

    private static string CleanEmail(string value)
    {
        return CleanRequired(value, "Authenticated user email is required.");
    }

    private static string CleanRequired(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException(message);
        }

        return value.Trim();
    }

    private static string CleanOptional(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private static IReadOnlyCollection<string> CleanTags(IReadOnlyCollection<string> tags)
    {
        return tags
            .Select(CleanOptional)
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(20)
            .ToArray();
    }

    private static string SerializeTags(IReadOnlyCollection<string> tags)
    {
        return JsonSerializer.Serialize(tags, JsonOptions);
    }

    private static IReadOnlyCollection<string> DeserializeTags(string json)
    {
        return JsonSerializer.Deserialize<IReadOnlyCollection<string>>(json, JsonOptions)
            ?? Array.Empty<string>();
    }

    private static DateTimeOffset BuildDefaultReminderAtUtc(DateOnly plannedDate, string mealSlot)
    {
        var time = MealSlots.DefaultReminderTimeUtc(mealSlot);
        return new DateTimeOffset(plannedDate.ToDateTime(time, DateTimeKind.Utc));
    }
}
