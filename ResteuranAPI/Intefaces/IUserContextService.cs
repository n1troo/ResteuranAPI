using System.Security.Claims;

namespace ResteuranAPI.Intefaces
{
    public interface IUserContextService
    {
        public ClaimsPrincipal User { get; }
        public int? GetUserId { get; }
    }
}
