using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Models;

namespace ResteuranAPI.Intefaces;
public interface IDishService
{
    int Create(int restaurantId, CreateDishDTO createDishDto);
    ActionResult<DishDTO> GetDishById(int restaurantId, int dishId);
    ActionResult<List<DishDTO>> GetAll(int restaurantId);
    void DeleteAll(int restaurantId);
}