using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.API.Repositories.Contracts
{
    public interface IAuthRepository
    {
        String GenerateToken(int userId, String userName, String email);
        Task<User?> GetUser(UserToLoginDto userToLoginDto);
        Task<User?> GetUserById(int userId);
    }
}
