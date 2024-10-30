using Serilog;
using Serilog.Events;

namespace WeatherApp.Web.Configurations;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this ConfigureHostBuilder hostBuilder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration) 
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}
