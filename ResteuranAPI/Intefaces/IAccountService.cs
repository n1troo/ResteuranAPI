using ResteuranAPI.Models;

namespace ResteuranAPI.Intefaces;

public interface IAccountService
{
    void RegisterUser(RegisterUserDTO registerUserDto);
    string GenerateJwt(LoginDTO loginDto);
    
}