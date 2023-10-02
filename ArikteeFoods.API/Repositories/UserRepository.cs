using ArikteeFoods.API.Data;
using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Exceptions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ArikteeDbContext _arikteeDbContext;

        public UserRepository(ArikteeDbContext arikteeDbContext)
        {
            this._arikteeDbContext = arikteeDbContext;
        }

        public async Task<User> AddAddress(int userId, AddressToAddDto addressToAddDto)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();

            if (user is not null)
            {
                var newAddress = new DeliveryAddress
                {
                    City = addressToAddDto.City,
                    HouseAddress = addressToAddDto.HouseAddress,
                    UserId = userId,
                    Recent = false
                };
                await _arikteeDbContext.DeliveryAddresses.AddAsync(newAddress);
                await _arikteeDbContext.SaveChangesAsync();

                return user;
            }
            throw new HttpResponseException(404, "This user does not exist.");
        }
    }
}
