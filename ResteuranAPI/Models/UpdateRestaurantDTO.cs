using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;

namespace ResteuranAPI.Controllers;

public class UpdateRestaurantDTO
{
   [Required]
   [MaxLength(25)]
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasDelivery { get; set; }
}