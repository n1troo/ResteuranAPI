

using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ResteuranAPI.Authorization;

public class MinimumAgeRequimentHandler : AuthorizationHandler<MinimumAgeRequiment>
{
    private readonly ILogger<MinimumAgeRequiment> _logger;

    public MinimumAgeRequimentHandler(ILogger<MinimumAgeRequiment> logger)
    {
        _logger = logger;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequiment requirement)
    {
        var age =  DateTime.Parse(context.User.FindFirst(claim => claim.Type == "DateOfBrith").Value);

        var userEmial = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
        
        _logger.LogInformation($"User {userEmial} is autorization custom claims");
        
        if (age.AddYears(requirement.MinimumAge) > DateTime.Today)
        {
            _logger.LogInformation($"User {userEmial} is autorization successed");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogInformation($"User {userEmial} is autorization faild");
        }
    

        return Task.CompletedTask;
    }
}