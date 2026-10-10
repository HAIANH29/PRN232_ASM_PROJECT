using System.Reflection;
using DietAdminFoodsController = LongevityDiet.DietKnowledge.Api.Controllers.AdminFoodsController;
using DietAdminGuidelinesController = LongevityDiet.DietKnowledge.Api.Controllers.AdminDietGuidelinesController;
using DietAdminRecipesController = LongevityDiet.DietKnowledge.Api.Controllers.AdminRecipesController;
using DietBookSourcesController = LongevityDiet.DietKnowledge.Api.Controllers.BookSourcesController;
using DietFoodsController = LongevityDiet.DietKnowledge.Api.Controllers.FoodsController;
using DietKnowledgeCandidatesController = LongevityDiet.DietKnowledge.Api.Controllers.KnowledgeCandidatesController;
using IdentityAuthController = LongevityDiet.Identity.Api.Controllers.AuthController;
using MealPlansController = LongevityDiet.MealPlanning.Api.Controllers.MealPlansController;
using MealRecommendationsController = LongevityDiet.MealPlanning.Api.Controllers.MealRecommendationsController;
using Microsoft.AspNetCore.Authorization;
using TrackingDailyController = LongevityDiet.Tracking.Api.Controllers.DailyTrackingController;
using TrackingProgressController = LongevityDiet.Tracking.Api.Controllers.ProgressSummariesController;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class ApiAuthorizationTests
{
    [Theory]
    [InlineData(typeof(DietAdminGuidelinesController))]
    [InlineData(typeof(DietAdminFoodsController))]
    [InlineData(typeof(DietAdminRecipesController))]
    [InlineData(typeof(DietBookSourcesController))]
    [InlineData(typeof(DietKnowledgeCandidatesController))]
    public void Diet_knowledge_admin_controllers_require_admin_role(Type controllerType)
    {
        var authorize = Assert.Single(controllerType.GetCustomAttributes<AuthorizeAttribute>());
        Assert.Equal("Admin", authorize.Roles);
    }

    [Theory]
    [InlineData(typeof(MealPlansController))]
    [InlineData(typeof(MealRecommendationsController))]
    [InlineData(typeof(TrackingDailyController))]
    [InlineData(typeof(TrackingProgressController))]
    public void User_owned_workflow_controllers_require_authentication(Type controllerType)
    {
        var authorize = Assert.Single(controllerType.GetCustomAttributes<AuthorizeAttribute>());
        Assert.Null(authorize.Roles);
        Assert.Null(authorize.Policy);
    }

    [Fact]
    public void Public_food_browse_controller_is_anonymous_but_admin_food_controller_is_not()
    {
        Assert.Single(typeof(DietFoodsController).GetCustomAttributes<AllowAnonymousAttribute>());
        Assert.Empty(typeof(DietFoodsController).GetCustomAttributes<AuthorizeAttribute>());

        var authorize = Assert.Single(typeof(DietAdminFoodsController).GetCustomAttributes<AuthorizeAttribute>());
        Assert.Equal("Admin", authorize.Roles);
    }

    [Fact]
    public void Identity_profile_and_admin_check_have_expected_authorization_boundaries()
    {
        var profile = typeof(IdentityAuthController).GetMethod(nameof(IdentityAuthController.GetProfile));
        var adminCheck = typeof(IdentityAuthController).GetMethod(nameof(IdentityAuthController.AdminCheck));

        Assert.NotNull(profile);
        Assert.NotNull(adminCheck);
        Assert.Single(profile!.GetCustomAttributes<AuthorizeAttribute>());
        Assert.Equal("AdminOnly", Assert.Single(adminCheck!.GetCustomAttributes<AuthorizeAttribute>()).Policy);
    }
}
