using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;

namespace csJwtError;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        this.logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // 保存原始的 response body stream
            var originalBodyStream = context.Response.Body;
            // 創建一個新的 MemoryStream 來捕獲響應內容
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            if (!(context.Response.StatusCode >= 200 && context.Response.StatusCode < 300))
            {
                // Handle no successful response
                // 獲取 status code
                var statusCode = context.Response.StatusCode;
                // 獲取 reason phrase
                var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
                string message = $"Status code: {statusCode}, Reason: {reasonPhrase}";
                bool hasContent = memoryStream.Length > 0;

                // 如果需要讀取具體內容
                if (!hasContent)
                {
                    var authMessage = context.Response.Headers.WWWAuthenticate;
                    if (authMessage.Count() > 0)
                    {
                        message = authMessage.FirstOrDefault();
                    }

                    var result = new APIResult
                    {
                        Success = false,
                        Message = $"{message} ",
                        Exception = null
                    };
                    var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    });

                    logger.LogError(json);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.Write(jsonBytes, 0, jsonBytes.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalBodyStream);
                }
                else
                {
                    string body = Encoding.UTF8.GetString(memoryStream.ToArray());
                    logger.LogInformation(body);
                    APIResult apiResult;
                    bool isApiResult = false;
                    try
                    {
                        apiResult = JsonSerializer.Deserialize<APIResult>(body, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)  // 允許所有 Unicode 字符直接輸出
                        });
                        if(string.IsNullOrEmpty(apiResult.Message) && apiResult.Success == false)
                        {
                            isApiResult = false;
                        }
                    }
                    catch (Exception)
                    {
                        apiResult = null;
                        isApiResult = false;
                    }

                    if (isApiResult == true)
                    {
                        // Handle successful response
                        // 將 response body stream 設置為原始的 stream
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(originalBodyStream);
                    }
                    else
                    {
                        apiResult = new APIResult
                        {
                            Success = false,
                            Message = $"{body}",
                            Exception = null
                        };
                        var json = JsonSerializer.Serialize(apiResult, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)  // 允許所有 Unicode 字符直接輸出
                        });
                        logger.LogError(json);
                        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Write(jsonBytes, 0, jsonBytes.Length);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(originalBodyStream);
                    }
                }
            }
            else
            {
                // Handle successful response
                // 將 response body stream 設置為原始的 stream
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }

            //src\Security\Authentication\JwtBearer\src\JwtBearerHandler.cs
            //    string? message = ex switch
            //    {
            //        SecurityTokenInvalidAudienceException stia => $"The audience '{stia.InvalidAudience ?? "(null)"}' is invalid",
            //        SecurityTokenInvalidIssuerException stii => $"The issuer '{stii.InvalidIssuer ?? "(null)"}' is invalid",
            //        SecurityTokenNoExpirationException _ => "The token has no expiration",
            //        SecurityTokenInvalidLifetimeException stil => "The token lifetime is invalid; NotBefore: "
            //            + $"'{stil.NotBefore?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}'"
            //            + $", Expires: '{stil.Expires?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}'",
            //        SecurityTokenNotYetValidException stnyv => $"The token is not valid before '{stnyv.NotBefore.ToString(CultureInfo.InvariantCulture)}'",
            //        SecurityTokenExpiredException ste => $"The token expired at '{ste.Expires.ToString(CultureInfo.InvariantCulture)}'",
            //        SecurityTokenSignatureKeyNotFoundException _ => "The signature key was not found",
            //        SecurityTokenInvalidSignatureException _ => "The signature is invalid",
            //        _ => null,
            //    };

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = new APIResult
        {
            Success = false,
            Message = "An error occurred while processing your request.",
            Exception = new ExceptionDetail
            {
                Type = exception.GetType().Name,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            }
        };

        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }
}

// Extension method to easily add the middleware to the pipeline
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
