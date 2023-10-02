using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Models;

namespace ArikteeFoods.Web.Services.Contracts
{
    public interface IAuthService
    {
        bool NewLogin { get; set; }
        bool NewSignout { get; set; }
        bool LoginRequired { get; set; }

        Task<UserDto?> GetUser(int userId);
        Task<LoggedInUserDto?> LoginUser(UserLogin userLogin);
        Task<UserDto?> RegisterUser(UserRegister userRegister);
        Task<UserDto> LogoutUser();
    }
}
