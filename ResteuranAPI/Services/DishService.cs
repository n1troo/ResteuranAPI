using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        GetRestaurantById(restaurantId);
        
        var dish = _mapper.Map<Dish>(createDishDto);
        _context.Dishes.Add(dish);
        _context.SaveChanges();

        return dish.Id;
    }

    public ActionResult<DishDTO> GetDishById(int restaurantId, int dishId)
    {
        var dish = GetRestaurantById(restaurantId)
            .Dishes
            .FirstOrDefault(s => s.Id == dishId);
        
        var returnerDish = _mapper.Map<DishDTO>(dish);

        return returnerDish;
    }

    public ActionResult<List<DishDTO>> GetAll(int restaurantId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        var dishes = _mapper.Map<List<DishDTO>>(restaurant.Dishes);

        return dishes;
    }

    public void DeleteAll(int restaurantId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        _context.RemoveRange(restaurant.Dishes);
        _context.SaveChanges();
    }

    private Restaurant GetRestaurantById(int restaurantId)
    {
        var restaurant = _context.Restaurants
            .Include(s => s.Dishes)
            .FirstOrDefault(s => s.Id == restaurantId);

        if (restaurant is null) throw new NotFoundException("Not found restaurant!");

        return restaurant;
    }
}