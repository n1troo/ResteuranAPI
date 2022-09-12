using Microsoft.AspNetCore.Identity;
using ResteuranAPI.Entities;
using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _restaurantDbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(RestaurantDbContext restaurantDbContext, IPasswordHasher<User> passwordHasher)
    {
        _restaurantDbContext = restaurantDbContext;
        _passwordHasher = passwordHasher;
    }
    public void RegisterUser(RegisterUserDTO registerUserDto)
    {
        var newUser = new User()
        {
            Email = registerUserDto.Email,
            RoleId = registerUserDto.RoleId,
            Nationality = registerUserDto.Nationality,
            DateOfBirth = registerUserDto.DateOfBirth,
            PasswordHash = registerUserDto.Password,
            FirstName = registerUserDto.LastName,
            LastName = registerUserDto.FirstName

        };
       
        
        var hasedPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
        newUser.PasswordHash = hasedPassword;
        
        _restaurantDbContext.Users.Add(newUser);
        _restaurantDbContext.SaveChanges();
    }
    
}