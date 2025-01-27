
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace csJwtError
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region 加入使用 Cookie & JWT 認證需要的宣告
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            builder.Services.AddAuthentication(
                "Jwt")
                .AddJwtBearer("Jwt", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:5029",
                        ValidAudience = "http://localhost:5029",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes("ECFFF7277E594BE2A71886F6973A2C5929A2C618AFD86" +
                            "DFBEB36B477A83716F46255DFEFE32DAA5B8F66AF1")), // https://randomkeygen.com/)),
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            #endregion

            var app = builder.Build();

            app.UseExceptionHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
