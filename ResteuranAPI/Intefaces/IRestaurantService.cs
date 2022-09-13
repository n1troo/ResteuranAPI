using ResteuranAPI.Controllers;
using ResteuranAPI.Models;

namespace ResteuranAPI.Intefaces;

public interface IRestaurantService
{
    public void UpdateRestaurant(UpdateRestaurantDTO dtoupdate, int id);
    public void DeleteById(int id);
    public IEnumerable<RestaurantDTO> GetAll();
    public RestaurantDTO GetById(int id);
    public int CreateRestaurant(CreateRestaurantDTO createRestaurantDto);

}