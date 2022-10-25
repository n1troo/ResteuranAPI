using Microsoft.AspNetCore.Mvc;

namespace ResteuranAPI.Models
{
    public class RestaurantQuery
    {
        public string? SearchPhase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
 