using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public interface IDishService
{
    int Create(int restaurantId, CreateDishDTO createDishDto);
}