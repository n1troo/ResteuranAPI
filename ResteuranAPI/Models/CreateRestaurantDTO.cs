using System.ComponentModel.DataAnnotations;

namespace ResteuranAPI.Models
{
    public class CreateRestaurantDTO
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ConcactEmail { get; set; }
        public string ConcactNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        
        public DateTime AddDate { get; set; }
        

    }
}