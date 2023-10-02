using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.API.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> AddAddress(int userId, AddressToAddDto addressToAddDto);
    }
}
