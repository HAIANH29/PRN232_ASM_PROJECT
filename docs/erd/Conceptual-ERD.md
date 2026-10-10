# Conceptual ERD

```mermaid
erDiagram
    USER {
        guid Id
        string Email
        string DisplayName
    }

    ROLE {
        guid Id
        string Name
    }

    DIET_GUIDELINE {
        guid Id
        string Title
        string Summary
        string SourceNote
        string SourceTitle
        string SourceChapter
        string SourcePage
        string SourceReference
        string ReviewStatus
        string ReviewedBy
        bool IsActive
    }

    FOOD {
        guid Id
        string Name
        string Category
        string CompatibilityNotes
        string SourceTitle
        string SourceChapter
        string SourcePage
        string SourceReference
        string ReviewStatus
        string ReviewedBy
        bool IsActive
    }

    RECIPE {
        guid Id
        string Name
        string Description
        string SourceTitle
        string SourceChapter
        string SourcePage
        string SourceReference
        string ReviewStatus
        string ReviewedBy
        bool IsActive
    }

    RECIPE_INGREDIENT {
        guid Id
        guid RecipeId
        guid FoodId
        string QuantityText
    }

    BOOK_SOURCE_DOCUMENT {
        guid Id
        string OriginalFileName
        string Status
        string UploadedBy
        datetime UploadedAtUtc
        int ChunkCount
    }

    BOOK_SOURCE_CHUNK {
        guid Id
        guid DocumentId
        int ChunkIndex
        int PageNumber
        string Text
        int TokenEstimate
    }

    KNOWLEDGE_CANDIDATE {
        guid Id
        guid DocumentId
        guid ChunkId
        string CandidateType
        string Title
        string Summary
        string Status
        guid ApprovedKnowledgeId
    }

    MEAL_PLAN {
        guid Id
        guid UserId
        string Name
        date StartDate
        date EndDate
        datetime CreatedAtUtc
        datetime UpdatedAtUtc
    }

    MEAL_PLAN_ITEM {
        guid Id
        guid MealPlanId
        guid RecipeId
        guid FoodId
        date PlannedDate
        string MealSlot
        string Notes
        datetime ReminderAtUtc
    }

    MEAL_RECOMMENDATION_REQUEST {
        guid Id
        guid UserId
        string PreferenceTagsJson
        int Days
        string Status
        string SuggestedMealTitlesJson
        string Disclaimer
        guid AcceptedMealPlanId
        datetime RequestedAtUtc
        datetime CompletedAtUtc
    }

    DAILY_TRACKING {
        guid Id
        guid UserId
        date TrackingDate
        string Notes
        datetime CreatedAtUtc
        datetime UpdatedAtUtc
    }

    MEAL_TRACKING {
        guid Id
        guid DailyTrackingId
        guid MealPlanItemId
        bool IsCompleted
        datetime CompletedAtUtc
        datetime CreatedAtUtc
        datetime UpdatedAtUtc
    }

    PROGRESS_SUMMARY {
        guid Id
        guid UserId
        date PeriodStartDate
        date PeriodEndDate
        int PlannedMeals
        int CompletedMeals
        datetime CalculatedAtUtc
        guid LastNotificationId
        datetime LastNotificationSentAtUtc
    }

    USER }o--o{ ROLE : has
    RECIPE ||--o{ RECIPE_INGREDIENT : contains
    FOOD ||--o{ RECIPE_INGREDIENT : used_by
    BOOK_SOURCE_DOCUMENT ||--o{ BOOK_SOURCE_CHUNK : extracted_into
    BOOK_SOURCE_DOCUMENT ||--o{ KNOWLEDGE_CANDIDATE : generates
    BOOK_SOURCE_CHUNK ||--o{ KNOWLEDGE_CANDIDATE : supports
    MEAL_PLAN ||--o{ MEAL_PLAN_ITEM : contains
    DAILY_TRACKING ||--o{ MEAL_TRACKING : records
```

Cross-service relationships such as `MealPlan.UserId`, `MealPlanItem.RecipeId`, `MealPlanItem.FoodId`, and `MealTracking.MealPlanItemId` are ID references only, not cross-database foreign keys.

Meal scheduling is represented by `MealPlanItem.PlannedDate`, `MealSlot`, and `ReminderAtUtc` rather than a separate `MealSchedule` table in the current workflow.

Book-source tables are internal to Diet Knowledge review. `KnowledgeCandidate.ApprovedKnowledgeId` stores the created guideline/food identifier after approval but is not modeled as a foreign key because candidates can approve into more than one managed content type.
