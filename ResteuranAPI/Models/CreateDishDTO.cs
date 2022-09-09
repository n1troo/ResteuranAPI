using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;

namespace ResteuranAPI.Models
{
    public class CreateDishDTO
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime AddDate { get; set; }
        
        public int RestaurantId { get; set; }
    }
}
