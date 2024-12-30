using Microsoft.AspNetCore.Mvc;

namespace csApiExceptionHandling.Controllers;

[ApiController]
[Route("[controller]")]
public class HasExceptionController : ControllerBase
{
    private readonly ILogger<HasExceptionController> _logger;

    public HasExceptionController(ILogger<HasExceptionController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        throw new Exception("This is an exception from Get method.");
        return null;
    }
}
