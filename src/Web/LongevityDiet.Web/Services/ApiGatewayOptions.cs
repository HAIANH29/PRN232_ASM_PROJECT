namespace LongevityDiet.Web.Services;

public sealed class ApiGatewayOptions
{
    public const string SectionName = "ApiGateway";

    public string BaseUrl { get; set; } = "http://api-gateway:8080";
}
