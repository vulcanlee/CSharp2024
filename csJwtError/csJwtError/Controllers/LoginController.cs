using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace csJwtError.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;

        public LoginController(ILogger<LoginController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            var token = GenerateToken("VulcanLee");
            logger.LogInformation(token);
            return token;
        }

        string GenerateToken(string account)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, account),

            };

            var token = new JwtSecurityToken
            (
                issuer: "http://localhost:5029",
                audience: "http://localhost:5029",
                claims: claims,
                expires: DateTime.Now.AddSeconds(60*10),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes("ECFFF7277E594BE2A71886F6973A2C5929A2C618AFD86" +
                            "DFBEB36B477A83716F46255DFEFE32DAA5B8F66AF1")), // https://randomkeygen.com/
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
    }
}
