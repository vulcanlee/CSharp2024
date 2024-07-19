namespace csConfigurationRunTimeChange;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<ISendMessage, SendConsoleMessage>();

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.Configure<RealTimeConfig>(builder.Configuration.GetSection("RealTime"));

        var host = builder.Build();
        host.Run();
    }
}