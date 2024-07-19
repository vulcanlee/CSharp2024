using Microsoft.Extensions.Options;

namespace csConfigurationRunTimeChange;

public class SendConsoleMessage : ISendMessage
{
    private readonly IConfiguration configuration;
    private readonly IOptionsMonitor<RealTimeConfig> options;

    public SendConsoleMessage(IConfiguration configuration,
        IOptionsMonitor<RealTimeConfig> options)
    {
        this.configuration = configuration;
        this.options = options;
    }

    public async Task SendAsync(string message)
    {
        string valueUrlInConfiguration = configuration["RealTime:Url"];
        string valueNameInConfiguration = configuration["RealTime:Name"];
        string valueUrlInOptions = options.CurrentValue.Url;
        string valueNameInOptions = options.CurrentValue.Name;

        await Console.Out.WriteLineAsync(message);
        await Console.Out.WriteLineAsync($"   Configuration Url: {valueUrlInConfiguration}");
        await Console.Out.WriteLineAsync($"   Configuration Name: {valueNameInConfiguration}");
        await Console.Out.WriteLineAsync($"   Options Url: {valueUrlInOptions}");
        await Console.Out.WriteLineAsync($"   Options Name: {valueNameInOptions}");
    }
}
