using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Models;

namespace ArikteeFoods.Web.Services.Contracts
{
    public interface IAuthService
    {
        Task<UserDto?> LoginUser(UserLogin userLogin);
        Task<UserDto> LogoutUser();
        Task<UserDto> CreateUser();
    }
}
