using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public interface IDishService
{
    int Create(int restaurantId, CreateDishDTO createDishDto);
    ActionResult<DishDTO> GetDishById(int restaurantId, int dishId);
    ActionResult<List<DishDTO>> GetAll(int restaurantId);
    void DeleteAll(int restaurantId);
}