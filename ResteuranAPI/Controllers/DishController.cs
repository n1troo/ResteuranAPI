using Microsoft.AspNetCore.Mvc;

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

            return Created($"api/{restaurantId}/dish/{idDish}",null);
        }
    }
}
