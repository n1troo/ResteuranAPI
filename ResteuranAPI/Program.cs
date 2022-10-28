using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using ResteuranAPI.Authorization;
using ResteuranAPI.Entities;
using ResteuranAPI.Intefaces;
using ResteuranAPI.Middleware;
using ResteuranAPI.Models;
using ResteuranAPI.Models.Validators;
using ResteuranAPI.Services;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();


//singleton - raz utworzona podczas trwania aplikacji
//scoped - za kazdym razem nowy obiekt przy zapytaniu
//transient - za kadym razem gdy odwołujemy sie do nich przez konstruktor

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var configuration = builder.Configuration;

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    builder.Services.AddScoped<IUserContextService, UserContextService>();
   
    builder.Services.AddScoped<IRestaurantService, RestaurantService>();
    builder.Services.AddScoped<ErrorHandlingMiddlewarece>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
    builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();


    builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
    builder.Services.AddSingleton(configuration);
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Nationality", localbuilder => localbuilder.RequireClaim("Nationality", "German", "Poland"));
        options.AddPolicy("AtList20", localbuilder => localbuilder.AddRequirements(new MinimumAgeRequiment(20)) );
        options.AddPolicy("MinRestaurant", localbuilder => localbuilder.AddRequirements(new CreatedMultipleRestaurantRequirement(2)));
    });

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });

    builder.Services.AddScoped<IAuthorizationHandler,CreatedMultipleRestaurantRequirementHanlder>();
    builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequimentHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirmentHandler>();
    builder.Services.AddHttpContextAccessor();//dla IHttpContextAccessor

    builder.Services.AddCors(options=>
    {
        options.AddPolicy("FrontedClient", builder =>
        {
            builder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(configuration["AllowedOrigins"]);  // .WithOrigins("http://localhost:8080");

        });
    });

    builder.Services.AddDbContext<RestaurantDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("RestaurantDbContext")));

    var app = builder.Build();

    app.UseResponseCaching();
    app.UseStaticFiles();
    app.UseCors("FrontedClient");


    app.UseMiddleware<ErrorHandlingMiddlewarece>();
    app.UseMiddleware<RequestTimeMiddleware>();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseAuthentication();
    app.UseAuthorization();

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