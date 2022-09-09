using System.Diagnostics;

namespace ResteuranAPI.Middleware;

public class RequestTimeMiddleware :IMiddleware
{
    private readonly ILogger<RequestTimeMiddleware> _tLogger;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> tLogger)
    {
        _tLogger = tLogger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        await next.Invoke(context);
        stopwatch.Stop();

        _tLogger.LogInformation($"Time of working in {context.Request.Path} was {stopwatch.Elapsed.Milliseconds} ms");
    }
}