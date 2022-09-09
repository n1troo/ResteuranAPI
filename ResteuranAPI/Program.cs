using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using ResteuranAPI.Entities;
using ResteuranAPI.Middleware;
using ResteuranAPI.Services;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
    builder.Services.AddDbContext<RestaurantDbContext>();
    builder.Services.AddScoped<IRestaurantService, RestaurantService>();
    builder.Services.AddScoped<ErrorHandlingMiddlewarece>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IDishService, DishService>();

//singleton - raz utworzona podczas trwania aplikacji
//scoped - za kazdym razem nowy obiekt przy zapytaniu
//transient - za kadym razem gdy odwołujemy sie do nich przez konstruktor

    builder.Logging.AddNLog();

    var app = builder.Build();

    app.UseMiddleware<ErrorHandlingMiddlewarece>();
    app.UseMiddleware<RequestTimeMiddleware>();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}