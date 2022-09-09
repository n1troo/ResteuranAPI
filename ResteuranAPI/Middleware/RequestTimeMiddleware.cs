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

     
        _tLogger.LogInformation($"Execution time of the request \"{context.Request.Path}\" was {stopwatch.Elapsed.Milliseconds} ms");
    }
}