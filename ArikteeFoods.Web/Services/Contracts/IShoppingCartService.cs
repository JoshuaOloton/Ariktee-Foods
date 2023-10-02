using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Models.PaystackModels;

namespace ArikteeFoods.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartItemDto?> AddCartItem(CartItemToAddDto cartItemToAddDto);

        Task<List<ShoppingCartDto>?> GetAllCarts(int userId);

        Task<List<ShoppingCartItemDto>?> GetItemsByCart(int cartId);

        Task<List<ShoppingCartItemDto>?> GetCartItems(int userId);

        Task<ShoppingCartDto?> GetCurrentCart(int userId);

        Task<ShoppingCartItemDto?> UpdateCartItem(int Id, CartItemQtyUpdateDto cartItemQtyUpdateDto);

        Task<ShoppingCartItemDto?> DeleteCartItem(int id);

        Task<InitializeCheckoutDto?> InitializeCheckout(int cartId, TotalPayInfo payInfo);

        Task VerifyLastCheckout(int userID);

        event EventHandler<ShoppingCartEventArgs> ShoppingCartChanged;

        void OnShoppingCartChanged(ShoppingCartEventArgs e);
    }
}
