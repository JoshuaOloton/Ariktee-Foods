using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.Web.Services.Contracts
{
    public interface ILocationService
    {
        Task<UserDto?> AddAddress(int userId, AddressToAddDto addressToAddDto);
        Task<IEnumerable<CityDto>> GetCities();
    }
}
