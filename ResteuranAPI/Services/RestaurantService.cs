using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResteuranAPI.Controllers;
using ResteuranAPI.Entities;
using ResteuranAPI.Errors;
using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;


    public RestaurantService(RestaurantDbContext context, IMapper _mapper, ILogger<RestaurantService> logger )
    {
        _context = context;
        this._mapper = _mapper;
        _logger = logger;
    }
    
    public RestaurantDTO GetById(int id)
    {
        var restaurant = _context.Restaurants
            .Include(r => r.Address)
            .Include(s => s.Dishes)
            .FirstOrDefault(s => s.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var result = _mapper.Map<RestaurantDTO>(restaurant);

        return result;
    }

    public IEnumerable<RestaurantDTO> GetAll()
    {
        var restaurants = _context.Restaurants
            .Include(r => r.Address)
            .Include(s => s.Dishes)
            .ToList();

        var restult = _mapper.Map<List<RestaurantDTO>>(restaurants);
        return restult;
    }

    public int CreateRestaurant(CreateRestaurantDTO createRestaurantDto)
    {
        var cMap = _mapper.Map<Restaurant>(createRestaurantDto);
        var restaurant = _context.Restaurants.Add(cMap);
        _context.SaveChanges();

        return cMap.Id;
    }

    public void DeleteById(int id)
    {
        _logger.LogWarning($"Restaurant with id: {id} in DELETE action");

        var restaurant = _context.Restaurants.FirstOrDefault(s => s.Id == id);
        
        if (restaurant == null)
            throw new NotFoundException("Restaurant not found");

        _context.Restaurants.Remove(restaurant);
        _context.SaveChanges();
    }

    public void UpdateRestaurant(UpdateRestaurantDTO dtoupdate, int id)
    {
        var restaurant = _context.Restaurants.SingleOrDefault(s => s.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        restaurant.Name = dtoupdate.Name;
        restaurant.Description = dtoupdate.Description;
        restaurant.HasDelivery = dtoupdate.HasDelivery;

        var cmap = _mapper.Map<Restaurant>(restaurant);
        _context.Restaurants.Update(cmap);
        _context.SaveChanges();
    }
}