using Microsoft.AspNetCore.Mvc;

namespace csDIRegistrationDifferent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VerifyRegistrationDIController : ControllerBase
    {
        private readonly ILogger<VerifyRegistrationDIController> logger;
        private readonly TogetherMyService togetherMyService;
        private readonly ITransientMyService transient;
        private readonly IScopedMyService scoped;
        private readonly ISingletonMyService singleton;

        public VerifyRegistrationDIController(ILogger<VerifyRegistrationDIController> logger,
            TogetherMyService togetherMyService, ITransientMyService transient,
            IScopedMyService scoped, ISingletonMyService singleton)
        {
            this.logger = logger;
            this.togetherMyService = togetherMyService;
            this.transient = transient;
            this.scoped = scoped;
            this.singleton = singleton;
        }
        [HttpGet(Name = "GetVerifyRegistrationDI")]
        public void Get()
        {
            Console.WriteLine($"=== [From VerifyRegistrationDIController ] ======================");
            Console.WriteLine($"Transient Value: {transient.Value}");
            Console.WriteLine($"Scoped Value: {scoped.Value}");
            Console.WriteLine($"Singleton Value: {singleton.Value}");
            Console.WriteLine($"=================================================================");
            Console.WriteLine($"");
            Console.WriteLine($"");
            Console.WriteLine($"");
            togetherMyService.Show();
        }
    }
}
