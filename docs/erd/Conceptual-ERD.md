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
    }

    FOOD {
        guid Id
        string Name
        string Category
    }

    RECIPE {
        guid Id
        string Name
        string Description
    }

    RECIPE_INGREDIENT {
        guid Id
        guid RecipeId
        guid FoodId
        string QuantityText
    }

    MEAL_PLAN {
        guid Id
        guid UserId
        string Name
        date StartDate
        date EndDate
    }

    MEAL_PLAN_ITEM {
        guid Id
        guid MealPlanId
        guid RecipeId
        guid FoodId
        date PlannedDate
        string MealSlot
    }

    DAILY_TRACKING {
        guid Id
        guid UserId
        date TrackingDate
    }

    MEAL_TRACKING {
        guid Id
        guid DailyTrackingId
        guid MealPlanItemId
        bool IsCompleted
    }

    PROGRESS_SUMMARY {
        guid Id
        guid UserId
        date PeriodStartDate
        date PeriodEndDate
        int PlannedMeals
        int CompletedMeals
    }

    USER }o--o{ ROLE : has
    RECIPE ||--o{ RECIPE_INGREDIENT : contains
    FOOD ||--o{ RECIPE_INGREDIENT : used_by
    MEAL_PLAN ||--o{ MEAL_PLAN_ITEM : contains
    DAILY_TRACKING ||--o{ MEAL_TRACKING : records
```

Cross-service relationships such as `MealPlan.UserId`, `MealPlanItem.RecipeId`, `MealPlanItem.FoodId`, and `MealTracking.MealPlanItemId` are ID references only, not cross-database foreign keys.
