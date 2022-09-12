using ResteuranAPI.Models;

namespace ResteuranAPI.Services;

public interface IAccountService
{
    void RegisterUser(RegisterUserDTO registerUserDto);
}