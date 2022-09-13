using Microsoft.AspNetCore.Authorization;

namespace ResteuranAPI.Authorization;

public class MinimumAgeRequiment : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public MinimumAgeRequiment(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}