using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Models.PaystackModels;

namespace ArikteeFoods.API.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<Cart>> GetAllCarts(int userId);
        Task<VwCartItem?> GetItem(int Id);
        Task<IEnumerable<VwCartItem>> GetCartItems(int cartId);
        Task<IEnumerable<VwCartItem>?> GetAllItems(int userId);
        Task<IEnumerable<VwCartItem>?> GetItems(int userId, int cartId);
        Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<Cart?> GetCurrentCart(int userId);
        Task<Cart?> GetLastCart(int userId);
        Task<CartItem?> UpdateQty(int Id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
        Task<CartItem?> DeleteItem(int id);
        Task<InitializeCheckoutDto> InitializeCheckout(int cartId, TotalPayInfo payInfo);
        Task<VerifyCheckoutDto> VerifyCheckout(CartCheckoutToVerifyDto cartCheckoutToVerifyDto);
        Task<bool> VerifyUserExists(String email);
    }
}
