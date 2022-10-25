using ResteuranAPI.Controllers;
using ResteuranAPI.Models;

using System.Security.Claims;

namespace ResteuranAPI.Intefaces;

public interface IRestaurantService
{
    public void UpdateRestaurant(UpdateRestaurantDTO dtoupdate, int id);
    public void DeleteById(int id);
    public PageResults<RestaurantDTO> GetAll(RestaurantQuery searchPhase);
    public RestaurantDTO GetById(int id);
    public int CreateRestaurant(CreateRestaurantDTO createRestaurantDto);

}