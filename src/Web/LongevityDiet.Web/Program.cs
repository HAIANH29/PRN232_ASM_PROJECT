using LongevityDiet.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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
