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
        _logger.LogInformation("�}�l������ըҥ~���` API");

        throw new Exception("�o�O�@�Ӵ��ըҥ~���`");
    }
}
