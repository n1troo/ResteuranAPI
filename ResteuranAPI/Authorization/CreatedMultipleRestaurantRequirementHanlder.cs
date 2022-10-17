using Microsoft.AspNetCore.Authorization;

using ResteuranAPI.Entities;
using ResteuranAPI.Intefaces;

namespace ResteuranAPI.Authorization
{
    public class CreatedMultipleRestaurantRequirementHanlder : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IUserContextService _userContextService;

        public CreatedMultipleRestaurantRequirementHanlder(RestaurantDbContext restaurantDbContext, IUserContextService userContextService)
        {
            _restaurantDbContext = restaurantDbContext;
            _userContextService = userContextService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
        {
            int userId = _userContextService.GetUserId.Value;

            if(_restaurantDbContext.Restaurants.Where(s => s.CreatedById == userId).Count() >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
