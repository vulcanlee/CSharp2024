using Microsoft.AspNetCore.Mvc;
using NLog;

namespace csNlogTraceId.Controllers;

[ApiController]
[Route("[controller]")]
public class LongTaskController : ControllerBase
{
    private readonly ILogger<LongTaskController> _logger;

    public LongTaskController(ILogger<LongTaskController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task Get()
    {
        _logger.LogInformation("進入到控制內做些一秒的工作");
        await Task.Delay(1000);
        using (var perfLogger = new PerformanceLogger(_logger, "量測程式碼範例"))
        {
            await Task.Delay(5000);
        }
    }
}
