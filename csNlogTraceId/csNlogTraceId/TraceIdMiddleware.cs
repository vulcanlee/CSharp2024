namespace csNlogTraceId;

public class TraceIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TraceIdMiddleware> logger;

    public TraceIdMiddleware(RequestDelegate next,
        ILogger<TraceIdMiddleware> logger)
    {
        _next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var traceId = context.TraceIdentifier;
            NLog.MappedDiagnosticsLogicalContext.Set("TraceId", traceId);
            await _next(context);
        }
        catch (Exception ex)
        {
            // 记录异常
            logger.LogError(ex, "發生不可預期的錯誤");
            // 将异常重新抛出
            //throw;
        }
    }
}
