using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResteuranAPI.Errors;
using ResteuranAPI.Intefaces;
using ResteuranAPI.Models;
using ResteuranAPI.Services;

using System.Security.Claims;

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

    [Authorize(Policy = "Nationality")]
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDTO>> GetAll()
    {
        var result = _restaurantService.GetAll();

        return Ok(result);
    }

    ///[Authorize(Policy = "AtList20")]
    [HttpGet("{id:int}")]
    public ActionResult<RestaurantDTO> Get([FromRoute] int id)
    {
        var result = _restaurantService.GetById(id);
        
        if (result is null)
            throw new NotFoundException("Resteurant not found");

        return Ok(result);
    }

    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost]
    public ActionResult Create([FromBody] CreateRestaurantDTO dto)
    {
      
        var restaurantId = _restaurantService.CreateRestaurant(dto);
        return Created($"/api/restaurant/{restaurantId}", null);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete([FromRoute] int id)
    {

        _restaurantService.DeleteById(id );
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult Update([FromBody] UpdateRestaurantDTO dtoupdate, [FromRoute] int id)
    {
        _restaurantService.UpdateRestaurant(dtoupdate, id);
        return Ok();
        
    }
}