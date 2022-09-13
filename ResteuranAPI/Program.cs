using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

    var configuration = builder.Configuration;


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddDbContext<RestaurantDbContext>();
    builder.Services.AddScoped<IRestaurantService, RestaurantService>();
    builder.Services.AddScoped<ErrorHandlingMiddlewarece>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
    builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
    builder.Services.AddSingleton(configuration);
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Nationality", localbuilder => localbuilder.RequireClaim("Nationality", "German", "Polish"));
        options.AddPolicy("AtList20", localbuilder => localbuilder.AddRequirements(new MinimumAgeRequiment(20)) );
    });

    builder.Logging.AddNLog();

    builder.Services.AddAuthentication(options =>
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

    builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequimentHandler>();


    var app = builder.Build();

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