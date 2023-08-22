using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.API.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<VwCartItem?> GetItem(int Id);
        Task<IEnumerable<VwCartItem>?> GetItems(int userId);
        Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto);
    }
}
