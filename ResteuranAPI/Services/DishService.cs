using AutoMapper;
using ResteuranAPI.Entities;
using ResteuranAPI.Errors;
using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public class DishService : IDishService
{
    private readonly RestaurantDbContext _context;
    private readonly IMapper _mapper;

    public DishService(RestaurantDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public int Create(int restaurantId, CreateDishDTO createDishDto)
    {
        var dish = _mapper.Map<Dish>(createDishDto);
        // _context.Restaurants
        //     .FirstOrDefault(s => s.Id == restaurantId)?.Dishes.Add(dish);

        if (_context.Restaurants.FirstOrDefault(s => s.Id == restaurantId) is null)
        {
            throw new NotFoundException("Restaurant not found");
        }

        _context.Dishes.Add(dish);
        
        _context.SaveChanges();

        return dish.Id;

    }
}