using System.Globalization;
using LongevityDiet.Web.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IStringLocalizerFactory, DictionaryStringLocalizerFactory>();
builder.Services.AddSingleton<Translator>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Mvc.DataAnnotations.IValidationAttributeAdapterProvider, LocalizedValidationAttributeAdapterProvider>();
builder.Services
    .AddControllersWithViews(options => options.ModelMetadataDetailsProviders.Add(new HumanizedDisplayNameProvider()))
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)));
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ApiGatewayOptions>(
    builder.Configuration.GetSection(ApiGatewayOptions.SectionName));
builder.Services.AddHttpClient<ApiGatewayClient>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiGatewayOptions>>()
        .Value;
    client.BaseAddress = new Uri(EnsureTrailingSlash(options.BaseUrl));
});
builder.Services.AddScoped<UserSession>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// UI language comes from the language cookie (English by default); formats stay en-US.
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US", "en"),
    SupportedCultures = [new CultureInfo("en-US")],
    SupportedUICultures = [new CultureInfo("en"), new CultureInfo("vi")],
    RequestCultureProviders = [new CookieRequestCultureProvider()]
});

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapHealthChecks("/health");


app.Run();

static string EnsureTrailingSlash(string value)
{
    return value.EndsWith("/", StringComparison.Ordinal)
        ? value
        : $"{value}/";
}
