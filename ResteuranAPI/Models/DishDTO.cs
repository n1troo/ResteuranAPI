namespace ResteuranAPI.Models
{
    public class DishDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime AddDate { get; set; }
        //public int RestaurantId { get; set; }
    }
}
