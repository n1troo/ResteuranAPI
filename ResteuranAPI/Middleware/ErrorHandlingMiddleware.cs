using ResteuranAPI.Errors;
using ResteuranAPI.Services;


namespace ResteuranAPI.Middleware
{
    public class ErrorHandlingMiddlewarece : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddlewarece> _logger;

        public ErrorHandlingMiddlewarece(ILogger<ErrorHandlingMiddlewarece> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException exception)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(exception.Message);
            }
            catch (BadRequestExcetpion exception)
            {
                _logger.LogWarning(exception.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(exception.Message);
            }
        }
    }
}
