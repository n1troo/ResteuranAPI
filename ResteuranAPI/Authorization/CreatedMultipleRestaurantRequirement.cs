using Microsoft.AspNetCore.Authorization;

namespace ResteuranAPI.Authorization
{
    public class CreatedMultipleRestaurantRequirement :IAuthorizationRequirement
    {
        public int MinimumRestaurantCreated { get; }

        public CreatedMultipleRestaurantRequirement(int minimumRestaurantCreated)
        {
            MinimumRestaurantCreated = minimumRestaurantCreated;
        }
    }
}
