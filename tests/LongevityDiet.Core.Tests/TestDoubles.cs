using LongevityDiet.Contracts.Messaging;
using LongevityDiet.DietKnowledge.Application.Abstractions;
using LongevityDiet.DietKnowledge.Application.Models;
using LongevityDiet.DietKnowledge.Domain.Entities;
using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Application.Common;
using LongevityDiet.Identity.Domain.Entities;
using LongevityDiet.MealPlanning.Application.Abstractions;
using LongevityDiet.MealPlanning.Application.Models;
using LongevityDiet.MealPlanning.Domain.Entities;
using LongevityDiet.Recommendation.Service;
using LongevityDiet.Tracking.Application.Abstractions;

namespace LongevityDiet.Core.Tests;

internal sealed class InMemoryUserRepository : IUserRepository
{
    public List<User> Users { get; } = [];

    public List<Role> Roles { get; } =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = IdentityRoleNames.User,
            NormalizedName = IdentityRoleNames.Normalize(IdentityRoleNames.User)
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = IdentityRoleNames.Admin,
            NormalizedName = IdentityRoleNames.Normalize(IdentityRoleNames.Admin)
        }
    ];

    public int SaveChangesCount { get; private set; }

    public IQueryable<User> Query()
    {
        return Users.AsQueryable();
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Users.FirstOrDefault(user => user.Id == id));
    }

    public Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Users.FirstOrDefault(user => user.NormalizedEmail == normalizedEmail));
    }

    public Task<Role?> GetRoleByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Roles.FirstOrDefault(role => role.NormalizedName == normalizedName));
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Users.Any(user => user.NormalizedEmail == normalizedEmail));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCount++;
        return Task.CompletedTask;
    }
}

internal sealed class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return $"hashed:{password}";
    }

    public bool Verify(string password, string passwordHash)
    {
        return passwordHash == Hash(password);
    }
}

internal sealed class FakeJwtTokenGenerator : IJwtTokenGenerator
{
    public TokenResult Generate(User user)
    {
        return new TokenResult($"token:{user.Id}", DateTimeOffset.UtcNow.AddHours(1));
    }
}

internal sealed class InMemoryDietKnowledgeRepository : IDietKnowledgeRepository
{
    public List<DietGuideline> Guidelines { get; } = [];

    public List<Food> Foods { get; } = [];

    public List<Recipe> Recipes { get; } = [];

    public List<BookSourceDocument> Documents { get; } = [];

    public List<BookSourceChunk> Chunks { get; } = [];

    public List<KnowledgeCandidate> Candidates { get; } = [];

    public int SaveChangesCount { get; private set; }

    public Task<LongevityDiet.DietKnowledge.Application.Models.PagedResult<DietGuideline>> ListGuidelinesAsync(
        DietGuidelineQuery query,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ToPaged(Guidelines, query.PageNumber, query.PageSize));
    }

    public Task<DietGuideline?> GetGuidelineByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guidelines.FirstOrDefault(guideline => guideline.Id == id));
    }

    public Task<bool> GuidelineTitleExistsAsync(
        string title,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guidelines.Any(guideline =>
            string.Equals(guideline.Title, title, StringComparison.OrdinalIgnoreCase) &&
            (!excludedId.HasValue || guideline.Id != excludedId.Value)));
    }

    public Task AddGuidelineAsync(DietGuideline guideline, CancellationToken cancellationToken = default)
    {
        Guidelines.Add(guideline);
        return Task.CompletedTask;
    }

    public Task<LongevityDiet.DietKnowledge.Application.Models.PagedResult<Food>> ListFoodsAsync(
        FoodQuery query,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ToPaged(Foods, query.PageNumber, query.PageSize));
    }

    public Task<Food?> GetFoodByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Foods.FirstOrDefault(food => food.Id == id));
    }

    public Task<IReadOnlyCollection<Guid>> GetActiveFoodIdsAsync(
        IReadOnlyCollection<Guid> foodIds,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Guid> result = Foods
            .Where(food => food.IsActive && foodIds.Contains(food.Id))
            .Select(food => food.Id)
            .ToArray();

        return Task.FromResult(result);
    }

    public Task<bool> FoodNameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Foods.Any(food =>
            string.Equals(food.Name, name, StringComparison.OrdinalIgnoreCase) &&
            (!excludedId.HasValue || food.Id != excludedId.Value)));
    }

    public Task AddFoodAsync(Food food, CancellationToken cancellationToken = default)
    {
        Foods.Add(food);
        return Task.CompletedTask;
    }

    public Task<LongevityDiet.DietKnowledge.Application.Models.PagedResult<Recipe>> ListRecipesAsync(
        RecipeQuery query,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ToPaged(Recipes, query.PageNumber, query.PageSize));
    }

    public Task<Recipe?> GetRecipeByIdAsync(
        Guid id,
        bool includeInactive,
        bool approvedOnly,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Recipes.FirstOrDefault(recipe => recipe.Id == id));
    }

    public Task<bool> RecipeNameExistsAsync(
        string name,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Recipes.Any(recipe =>
            string.Equals(recipe.Name, name, StringComparison.OrdinalIgnoreCase) &&
            (!excludedId.HasValue || recipe.Id != excludedId.Value)));
    }

    public Task AddRecipeAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        Recipes.Add(recipe);
        return Task.CompletedTask;
    }

    public void ReplaceRecipeIngredients(Recipe recipe, IReadOnlyCollection<RecipeIngredient> ingredients)
    {
        recipe.Ingredients.Clear();
        foreach (var ingredient in ingredients)
        {
            recipe.Ingredients.Add(ingredient);
        }
    }

    public Task<LongevityDiet.DietKnowledge.Application.Models.PagedResult<BookSourceDocument>> ListBookSourceDocumentsAsync(
        BookSourceDocumentQuery query,
        CancellationToken cancellationToken = default)
    {
        var documents = Documents.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            documents = documents.Where(document => document.Status == query.Status);
        }

        return Task.FromResult(ToPaged(documents.ToList(), query.PageNumber, query.PageSize));
    }

    public Task<BookSourceDocument?> GetBookSourceDocumentByIdAsync(
        Guid id,
        bool includeChunks = false,
        bool includeCandidates = false,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Documents.FirstOrDefault(document => document.Id == id));
    }

    public Task AddBookSourceDocumentAsync(BookSourceDocument document, CancellationToken cancellationToken = default)
    {
        Documents.Add(document);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<BookSourceChunk>> ListBookSourceChunksAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<BookSourceChunk> result = Chunks
            .Where(chunk => chunk.DocumentId == documentId)
            .OrderBy(chunk => chunk.ChunkIndex)
            .ToArray();

        return Task.FromResult(result);
    }

    public Task AddBookSourceChunksAsync(
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        Chunks.AddRange(chunks);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<KnowledgeCandidate>> ListKnowledgeCandidatesAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<KnowledgeCandidate> result = Candidates
            .Where(candidate => candidate.DocumentId == documentId)
            .ToArray();

        return Task.FromResult(result);
    }

    public Task<KnowledgeCandidate?> GetKnowledgeCandidateByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Candidates.FirstOrDefault(candidate => candidate.Id == id));
    }

    public Task AddKnowledgeCandidatesAsync(
        IReadOnlyCollection<KnowledgeCandidate> candidates,
        CancellationToken cancellationToken = default)
    {
        Candidates.AddRange(candidates);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCount++;
        return Task.CompletedTask;
    }

    private static LongevityDiet.DietKnowledge.Application.Models.PagedResult<T> ToPaged<T>(
        IReadOnlyCollection<T> values,
        int pageNumber,
        int pageSize)
    {
        return new LongevityDiet.DietKnowledge.Application.Models.PagedResult<T>(
            values.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray(),
            pageNumber,
            pageSize,
            values.Count);
    }
}

internal sealed class FakeBookKnowledgeFileStore(string storagePath) : IBookKnowledgeFileStore
{
    public Task<string> SaveAsync(Stream content, string storedFileName, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Path.Combine(storagePath, storedFileName));
    }
}

internal sealed class FakeBookTextExtractor(IReadOnlyCollection<BookPageText> pages) : IBookTextExtractor
{
    public Task<IReadOnlyCollection<BookPageText>> ExtractPagesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(pages);
    }
}

internal sealed class FakeBookTextChunker : IBookTextChunker
{
    public IReadOnlyCollection<BookSourceChunk> CreateChunks(
        Guid documentId,
        IReadOnlyCollection<BookPageText> pages)
    {
        return pages.Select((page, index) => new BookSourceChunk
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            ChunkIndex = index + 1,
            PageNumber = page.PageNumber,
            Text = page.Text,
            CharacterCount = page.Text.Length,
            TokenEstimate = Math.Max(1, page.Text.Length / 4),
            CreatedAtUtc = DateTimeOffset.UtcNow
        }).ToArray();
    }
}

internal sealed class FakeKnowledgeCandidateGenerator : IKnowledgeCandidateGenerator
{
    public IReadOnlyCollection<KnowledgeCandidateDraft> Drafts { get; init; } = [];

    public Task<IReadOnlyCollection<KnowledgeCandidateDraft>> GenerateAsync(
        BookSourceDocument document,
        IReadOnlyCollection<BookSourceChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Drafts);
    }
}

internal sealed class InMemoryMealPlanRepository : IMealPlanRepository
{
    public List<MealPlan> MealPlans { get; } = [];

    public List<MealRecommendationRequest> RecommendationRequests { get; } = [];

    public int SaveChangesCount { get; private set; }

    public Task<LongevityDiet.MealPlanning.Application.Models.PagedResult<MealPlan>> ListMealPlansAsync(
        Guid userId,
        MealPlanQuery query,
        CancellationToken cancellationToken = default)
    {
        var values = MealPlans.Where(plan => plan.UserId == userId).ToArray();
        return Task.FromResult(ToPaged(values, query.PageNumber, query.PageSize));
    }

    public Task<MealPlan?> GetMealPlanAsync(
        Guid mealPlanId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(MealPlans.FirstOrDefault(plan =>
            plan.Id == mealPlanId && plan.UserId == userId));
    }

    public Task AddMealPlanAsync(MealPlan mealPlan, CancellationToken cancellationToken = default)
    {
        MealPlans.Add(mealPlan);
        return Task.CompletedTask;
    }

    public Task AddMealPlanItemAsync(MealPlanItem mealPlanItem, CancellationToken cancellationToken = default)
    {
        var plan = MealPlans.First(plan => plan.Id == mealPlanItem.MealPlanId);
        plan.Items.Add(mealPlanItem);
        return Task.CompletedTask;
    }

    public void RemoveMealPlan(MealPlan mealPlan)
    {
        MealPlans.Remove(mealPlan);
    }

    public void RemoveMealPlanItem(MealPlanItem mealPlanItem)
    {
        var plan = MealPlans.FirstOrDefault(existing => existing.Id == mealPlanItem.MealPlanId);
        plan?.Items.Remove(mealPlanItem);
    }

    public Task<LongevityDiet.MealPlanning.Application.Models.PagedResult<MealRecommendationRequest>> ListRecommendationRequestsAsync(
        Guid userId,
        RecommendationRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        var values = RecommendationRequests.Where(request => request.UserId == userId).ToArray();
        return Task.FromResult(ToPaged(values, query.PageNumber, query.PageSize));
    }

    public Task<MealRecommendationRequest?> GetRecommendationRequestAsync(
        Guid requestId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(RecommendationRequests.FirstOrDefault(request =>
            request.Id == requestId && request.UserId == userId));
    }

    public Task<MealRecommendationRequest?> GetRecommendationRequestForUpdateAsync(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(RecommendationRequests.FirstOrDefault(request => request.Id == requestId));
    }

    public Task AddRecommendationRequestAsync(
        MealRecommendationRequest request,
        CancellationToken cancellationToken = default)
    {
        RecommendationRequests.Add(request);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCount++;
        return Task.CompletedTask;
    }

    private static LongevityDiet.MealPlanning.Application.Models.PagedResult<T> ToPaged<T>(
        IReadOnlyCollection<T> values,
        int pageNumber,
        int pageSize)
    {
        return new LongevityDiet.MealPlanning.Application.Models.PagedResult<T>(
            values.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray(),
            pageNumber,
            pageSize,
            values.Count);
    }
}

internal sealed class FakeDietKnowledgeCatalogClient : IDietKnowledgeCatalogClient
{
    public HashSet<Guid> ExistingFoodIds { get; } = [];

    public HashSet<Guid> ExistingRecipeIds { get; } = [];

    public Task<bool> FoodExistsAsync(Guid foodId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ExistingFoodIds.Contains(foodId));
    }

    public Task<bool> RecipeExistsAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ExistingRecipeIds.Contains(recipeId));
    }
}

internal sealed class RecordingReminderPublisher : IReminderPublisher
{
    public List<ReminderRequestedMessage> Messages { get; } = [];

    public Task PublishAsync(ReminderRequestedMessage message, CancellationToken cancellationToken = default)
    {
        Messages.Add(message);
        return Task.CompletedTask;
    }
}

internal sealed class RecordingRecommendationRequestPublisher : IRecommendationRequestPublisher
{
    public List<RecommendationRequestMessage> Messages { get; } = [];

    public Task PublishAsync(RecommendationRequestMessage message, CancellationToken cancellationToken = default)
    {
        Messages.Add(message);
        return Task.CompletedTask;
    }
}

internal sealed class RecordingNotificationClient : INotificationClient
{
    public List<(Guid UserId, string RecipientEmail, string Subject, string Body, Guid ProgressSummaryId)> Requests { get; } = [];

    public Guid NotificationId { get; init; } = Guid.NewGuid();

    public Task<Guid?> RequestProgressNotificationAsync(
        Guid userId,
        string recipientEmail,
        string subject,
        string body,
        Guid progressSummaryId,
        CancellationToken cancellationToken = default)
    {
        Requests.Add((userId, recipientEmail, subject, body, progressSummaryId));
        return Task.FromResult<Guid?>(NotificationId);
    }
}

internal sealed class FakeApprovedKnowledgeClient(ApprovedKnowledgeContext context) : IApprovedKnowledgeClient
{
    public Task<ApprovedKnowledgeContext> GetApprovedKnowledgeAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(context);
    }
}

internal sealed class FakeGeminiClient : IGeminiClient
{
    public bool IsEnabled { get; init; }

    public string? Response { get; init; }

    public string? CapturedPrompt { get; private set; }

    public Task<string?> GenerateTextAsync(string prompt, CancellationToken cancellationToken = default)
    {
        CapturedPrompt = prompt;
        return Task.FromResult(Response);
    }
}

internal sealed class RecordingHttpMessageHandler : HttpMessageHandler
{
    public List<HttpRequestMessage> Requests { get; } = [];

    public Func<HttpRequestMessage, HttpResponseMessage> Responder { get; init; } =
        _ => new HttpResponseMessage(System.Net.HttpStatusCode.OK);

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Requests.Add(request);
        return Task.FromResult(Responder(request));
    }
}
