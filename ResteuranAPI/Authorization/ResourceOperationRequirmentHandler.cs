using Microsoft.AspNetCore.Authorization;

using ResteuranAPI.Entities;

using System.Security.Claims;

namespace ResteuranAPI.Authorization
{
    public class ResourceOperationRequirmentHandler : AuthorizationHandler<ResourceOperationRequirment, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirment requirement, Restaurant restaurant)
        {
            if(requirement.ResourceOperation == ResourceOperation.Read || requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var idUser = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if(restaurant.CreatedById == int.Parse(idUser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
