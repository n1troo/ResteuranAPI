using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Controllers;
using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public interface IRestaurantService
{
    RestaurantDTO GetById(int id);
    IEnumerable<RestaurantDTO> GetAll();
    int CreateRestaurant(CreateRestaurantDTO createRestaurantDto);
    void DeleteById(int id);
    void UpdateRestaurant(UpdateRestaurantDTO dtoupdate, int id);
}