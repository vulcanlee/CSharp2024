using Microsoft.AspNetCore.Mvc;
using NLog;

namespace csNlogTraceId.Controllers;

[ApiController]
[Route("[controller]")]
public class ThrowExceptionController : ControllerBase
{
    private readonly ILogger<ThrowExceptionController> _logger;

    public ThrowExceptionController(ILogger<ThrowExceptionController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("開始執行測試例外異常 API");

        throw new Exception("這是一個測試例外異常");
    }
}
