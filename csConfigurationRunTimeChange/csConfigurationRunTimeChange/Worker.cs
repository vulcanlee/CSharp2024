namespace csConfigurationRunTimeChange;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISendMessage sendMessage;

    public Worker(ILogger<Worker> logger,
        ISendMessage sendMessage)
    {
        _logger = logger;
        this.sendMessage = sendMessage;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await sendMessage.SendAsync($"--> Message: {DateTimeOffset.Now}");
            }
            await Task.Delay(3000, stoppingToken);
        }
    }
}
