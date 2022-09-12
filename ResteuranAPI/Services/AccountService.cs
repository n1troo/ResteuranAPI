using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResteuranAPI.Entities;
using ResteuranAPI.Errors;
using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _restaurantDbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ConfigurationManager _configurationManager;

    public AccountService(RestaurantDbContext restaurantDbContext, IPasswordHasher<User> passwordHasher,ConfigurationManager configurationManager )
    {
        _restaurantDbContext = restaurantDbContext;
        _passwordHasher = passwordHasher;
        _configurationManager = configurationManager;
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

    public string GenerateJwt(LoginDTO loginDto)
    {
        var user = _restaurantDbContext.Users
            .Include(s=>s.Role)
            .FirstOrDefault(s => s.Email == loginDto.Email);
        
        
        if (user is null) throw new BadRequestExcetpion("Not found this user in db");

        var passwordStatus = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

        if (passwordStatus == PasswordVerificationResult.Failed)
        {
            throw new BadRequestExcetpion("Not valid password!");
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("Nationality",user.Nationality),
            new Claim("DateOfBrith",user.DateOfBirth.ToShortDateString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager["JWT:Secret"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(10);

        var token = new JwtSecurityToken(
            issuer: _configurationManager["JWT:ValidIssuer"],
            audience: _configurationManager["JWT:ValidAudience"],
            claims: claims,
            expires: expires, 
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}