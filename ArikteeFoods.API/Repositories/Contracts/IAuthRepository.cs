using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.API.Repositories.Contracts
{
    public interface IAuthRepository
    {
        String GenerateToken(int userId, String userName, String email);
        RefreshToken GenerateRefreshToken();
        Task SetRefreshToken(int userId, RefreshToken? refreshToken);
        Task<User?> GetUser(UserToLoginDto userToLoginDto);
        Task<User?> GetUserById(int userId);
        Task<User> RegisterUser(UserToRegisterDto userToRegisterDto);
        Task<User?> GetUserByRefreshToken(String refreshToken);
    }
}
