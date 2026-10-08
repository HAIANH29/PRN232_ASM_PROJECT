namespace LongevityDiet.MealPlanning.Application.Services;

public sealed class MealPlanningService : IMealPlanningService
{
    public MealPlanningServiceStatus GetStatus()
    {
        return new MealPlanningServiceStatus(
            "Meal Planning Service",
            "MealPlanningDb",
            ["MealPlan", "MealPlanItem"],
            ["Diet Knowledge REST lookup placeholder", "RabbitMQ reminder publisher", "Recommendation gRPC client"]);
    }
}
