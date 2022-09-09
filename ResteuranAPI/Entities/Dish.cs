namespace ResteuranAPI.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public DateTime AddDate { get; set; }

    public int RestaurantId { get; set; }
    public virtual Restaurant Restaurant { get; set; }
}