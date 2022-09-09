using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Errors;
using ResteuranAPI.Models;
using ResteuranAPI.Services;

namespace ResteuranAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }


    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDTO>> GetAll()
    {
        var result = _restaurantService.GetAll();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<RestaurantDTO> Get([FromRoute] int id)
    {
        var result = _restaurantService.GetById(id);
        
        if (result is null)
            throw new NotFoundException("Resteurant not found");

        return Ok(result);
    }


    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDTO dto)
    {
        var restaurantId = _restaurantService.CreateRestaurant(dto);
        return Created($"/api/restaurant/{restaurantId}", null);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteRestaurant([FromRoute] int id)
    {
        _restaurantService.DeleteById(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult Update([FromBody] UpdateRestaurantDTO dtoupdate, [FromRoute] int id)
    {
        _restaurantService.UpdateRestaurant(dtoupdate,id);
        return Ok();
        
    }
}