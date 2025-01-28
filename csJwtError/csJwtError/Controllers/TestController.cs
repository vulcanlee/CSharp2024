using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace csJwtError.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Jwt")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> logger;

        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public void Get()
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]/{id}")]
        public void HasArg(int id)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult BadHasBody()
        {
            return BadRequest("�o�{�즳���D�A�ШD�L��(no APIResult)");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult BadHasApiResult()
        {
            var result = new APIResult
            {
                Success = false,
                Message = "�o�{�즳���D�A�ШD�L��(with APIResult)",
                Exception = null
            };

            return BadRequest(result);
        }
    }
}