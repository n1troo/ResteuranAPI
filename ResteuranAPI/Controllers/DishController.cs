using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Errors;
using ResteuranAPI.Models;
using ResteuranAPI.Services;

namespace ResteuranAPI.Controllers
{
    [ApiController]
    [Route("/api/{restaurantId}/dish")]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, CreateDishDTO createDishDTO)
        {
            int idDish = _dishService.Create(restaurantId,createDishDTO);

            return Created($"api/restaurant/{restaurantId}/dish/{idDish}",null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDTO> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _dishService.GetDishById(restaurantId, dishId);
            return Ok(dish);
        }
        
        [HttpGet]
        public ActionResult<List<DishDTO>> GetAll([FromRoute] int restaurantId)
        {
            var allDishes = _dishService.GetAll(restaurantId);
            return Ok(allDishes);
        }

        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int restaurantId)
        {
            _dishService.DeleteAll(restaurantId);
            return Ok();
        }
    }
}
