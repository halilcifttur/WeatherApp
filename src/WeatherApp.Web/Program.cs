using WeatherApp.Infrastructure.Configurations;
using WeatherApp.Web.Configurations;
using WeatherApp.Web.Extensions;
using WeatherApp.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000", "https://*:5001");

builder.Services.AddControllersWithViews();

builder.Services.AddPostgreSql(builder.Configuration);
builder.Services.AddMediatR();
builder.Services.AddRepositoryServices();
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddServiceConfigurations(builder.Configuration);
builder.Host.ConfigureSerilog(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Weather}/{action=Index}/{id?}");

app.Run();
