using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

using ResteuranAPI.Authorization;
using ResteuranAPI.Controllers;
using ResteuranAPI.Entities;
using ResteuranAPI.Errors;
using ResteuranAPI.Intefaces;
using ResteuranAPI.Models;

using System.Security.Claims;

namespace ResteuranAPI.Services;

[Authorize]
public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;

    public RestaurantService(RestaurantDbContext context, 
        IMapper mapper, ILogger<RestaurantService> logger, 
        IAuthorizationService authorizationService,
        IUserContextService userContextService
        )

    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
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

    public PageResults<RestaurantDTO> GetAll(RestaurantQuery query)
    {

        var baseQuery = _context.Restaurants
            .Include(r => r.Address)
            .Include(s => s.Dishes)

            .Where(s => query.SearchPhase == null || (s.Name.ToLower().Contains(query.SearchPhase.ToLower()) || s.Description.ToLower().Contains(query.SearchPhase.ToLower())));

        var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var restult = _mapper.Map<List<RestaurantDTO>>(restaurants);

        var totalItemsCount = (int)baseQuery.Count();

        var pageResult = new PageResults<RestaurantDTO>(restult, totalItemsCount, query.PageSize, query.PageNumber);



        return pageResult;
    }

    public int CreateRestaurant(CreateRestaurantDTO createRestaurantDto)
    {
        var restaurant = _mapper.Map<Restaurant>(createRestaurantDto);
        restaurant.CreatedById = _userContextService.GetUserId;
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();

        return restaurant.Id;
    }

    public void DeleteById(int id)
    {
        _logger.LogWarning($"Restaurant with id: {id} in DELETE action");

        var restaurant = _context.Restaurants.FirstOrDefault(s => s.Id == id);

        if (restaurant == null)
            throw new NotFoundException("Restaurant not found");

        var result = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirment(ResourceOperation.Delete));
        if (!result.IsCompletedSuccessfully)
        {
            throw new ForbidException();
        }

        _context.Restaurants.Remove(restaurant);
        _context.SaveChanges();
    }

    public void UpdateRestaurant(UpdateRestaurantDTO dtoupdate, int id)
    {
         

        var restaurant = _context.Restaurants.SingleOrDefault(s => s.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var result = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirment(ResourceOperation.Update));

        if (!result.IsCompletedSuccessfully)
        {
            throw new ForbidException();
        }

        restaurant.Name = dtoupdate.Name;
        restaurant.Description = dtoupdate.Description;
        restaurant.HasDelivery = dtoupdate.HasDelivery;

        var cmap = _mapper.Map<Restaurant>(restaurant);
        _context.Restaurants.Update(cmap);
        _context.SaveChanges();
    }

 
}