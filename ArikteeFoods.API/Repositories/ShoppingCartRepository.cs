using ArikteeFoods.API.Data;
using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Exceptions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Models.PaystackModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace ArikteeFoods.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ArikteeDbContext _arikteeDbContext;
        private readonly HttpClient _httpClient;

        public ShoppingCartRepository(ArikteeDbContext arikteeDbContext, HttpClient httpClient)
        {
            this._arikteeDbContext = arikteeDbContext;
            this._httpClient = httpClient;
        }

        private async Task<bool> IsItemExists(CartItemToAddDto cartItemToAddDto)
        {
            // CHECK IF CART ID AND PRODUCT ID EXISTS WITH SPECIFIC UNIT ID IN CARTITEMS TABLE
            return await _arikteeDbContext.CartItems.AnyAsync(e => 
                e.CartId == cartItemToAddDto.CartId && 
                e.ProductId == cartItemToAddDto.ProductId &&
                e.UnitId == cartItemToAddDto.UnitId);
        }

        public async Task<IEnumerable<Cart>> GetAllCarts(int userId)
        {
            var user = await _arikteeDbContext.Users.FindAsync(userId);
            if (user is not null)
            {
                return await _arikteeDbContext.Carts.Where(e => e.UserId == userId).Include(e => e.User).ToListAsync();
            }
            throw new HttpResponseException(404, "The user ID provided does not match an available user.");
        }

        public async Task<VwCartItem?> GetItem(int Id) => await _arikteeDbContext.VwCartItems.Where(e => e.Id == Id).FirstOrDefaultAsync();

        public async Task<IEnumerable<VwCartItem>> GetCartItems(int cartId)
        {
            var cart = await _arikteeDbContext.Carts.Where(e => e.Id == cartId).FirstOrDefaultAsync();
            if (cart is not null)
            {
                return await _arikteeDbContext.VwCartItems.Where(e => e.CartId == cartId).ToListAsync();
            }
            throw new HttpResponseException(404, "The cart ID provided does not match an available cart.");
        }

        public async Task<IEnumerable<VwCartItem>?> GetAllItems(int userId)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
            if (user == null) return null;
            return await _arikteeDbContext.VwCartItems.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<VwCartItem>?> GetItems(int userId, int cartId)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
            if (user == null) return null;
            return await _arikteeDbContext.VwCartItems.Where(e => e.UserId == userId && e.CartId == cartId).ToListAsync();
        }

        public async Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await IsItemExists(cartItemToAddDto) == false)
            {
                // CHECK IF CART ID AND PRODUCT ID EXISTS
                var newItem = await (from product in _arikteeDbContext.Products
                                    join cart in _arikteeDbContext.Carts
                                    on cartItemToAddDto.CartId equals cart.Id
                                    where product.Id == cartItemToAddDto.ProductId
                                    select new CartItem
                                    {
                                        CartId = cartItemToAddDto.CartId,
                                        ProductId = cartItemToAddDto.ProductId,
                                        UnitId = cartItemToAddDto.UnitId,
                                        Qty = 1,
                                        UnitAmount = cartItemToAddDto.UnitAmount
                                    }).FirstOrDefaultAsync();
                if (newItem is not null)
                {
                    var result = await _arikteeDbContext.CartItems.AddAsync(newItem);
                    await _arikteeDbContext.SaveChangesAsync();

                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<Cart?> GetCurrentCart(int userId)
        {
            if (await _arikteeDbContext.Users.AnyAsync(e => e.Id == userId) == true)
            {
                var currentCart = await _arikteeDbContext
                    .Carts.Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.Id)
                    .Include(e => e.User)
                    .FirstOrDefaultAsync();
                if (currentCart is null || currentCart.TransStatus != 0)
                {
                    // if user has no cart or recent cart(transaction)' status is 1(paid), create new cart
                    currentCart = await CreateCart(userId);
                }
                return currentCart;
            }
            return null;
        }

        public async Task<Cart?> GetLastCart(int userId)
        {
            if (await _arikteeDbContext.Users.AnyAsync(e => e.Id == userId) == true)
            {
                var currentCart = await _arikteeDbContext
                    .Carts.Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.Id)
                    .Include(e => e.User)
                    .FirstOrDefaultAsync();
                
                return currentCart;
            }
            return null;
        }

        private async Task<Cart> CreateCart(int userId)
        {
            var newCart = new Cart
            {
                UserId = userId,
                TransDate = DateTime.UtcNow,
                TransStatus = 0 // Pending transaction
            };
            var result = await _arikteeDbContext.Carts.AddAsync(newCart);
            await _arikteeDbContext.SaveChangesAsync();
            return result.Entity;
        }

        private async Task<Cart?> GetCart(int cartId)
        {
            return await _arikteeDbContext.Carts.Where(e => e.Id == cartId).FirstOrDefaultAsync();
        }

        public async Task<CartItem?> UpdateQty(int Id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var cartItem = await _arikteeDbContext.CartItems.FindAsync(Id);
            if (cartItem is null) return null;

            if (cartItemQtyUpdateDto.CartItemId != Id)
            {
                throw new Exception("The provided CartItem ID in the request body does not match the ID in the URL.");
            }
            cartItem.Qty = cartItemQtyUpdateDto.Qty;
            await _arikteeDbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> DeleteItem(int id)
        {
            var cartItem = await _arikteeDbContext.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return null;
            }
            var result = _arikteeDbContext.CartItems.Remove(cartItem);
            await _arikteeDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<InitializeCheckoutDto> InitializeCheckout(int cartId, TotalPayInfo payInfo)
        {
            var cart = await _arikteeDbContext.Carts.Where(e => e.Id == cartId).Include(e => e.CartItems).FirstOrDefaultAsync();
            if (cart is not null)
            {
                //var cartAmount = cart.CartItems.Sum(e => e.)
                // If trans reference and authorization url exists, return
                if (cart.AuthorizationUrl != null && cart.TransReference != null)
                {
                    return new InitializeCheckoutDto
                    {
                        Authorization_url = cart.AuthorizationUrl,
                        Reference = cart.TransReference
                    };
                }
                // If not, send request to api to generate payment reference and authorization url
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_28bcec9e9f6804d68ad55eeed470a755a50b5c1a");
                var response = await _httpClient.PostAsJsonAsync<ApplicantPayInfo>("initialize", payInfo.PayInfo);
                var responseData = await response.Content.ReadFromJsonAsync<InitializeResponseBody>();
                if (response.IsSuccessStatusCode)
                {
                    String authorizeUrl = responseData.Data.Authorization_url;
                    String transReference = responseData.Data.Reference;

                    cart.AuthorizationUrl = authorizeUrl;
                    cart.TransReference = transReference;
                    cart.DeliveryAddress = payInfo.Address;
                    await _arikteeDbContext.SaveChangesAsync();
                    return new InitializeCheckoutDto
                    {
                        Authorization_url = authorizeUrl,
                        Reference = transReference
                    };
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new HttpResponseException(Convert.ToInt32(response.StatusCode), responseData.Message);

            }
            throw new HttpResponseException(404, "The cart ID provided does not match an available cart.");
            //response.StatusCode
        }

        public async Task<VerifyCheckoutDto> VerifyCheckout(CartCheckoutToVerifyDto cartCheckoutToVerifyDto)
        {
            var cart = await _arikteeDbContext.Carts.Where(e => e.Id == cartCheckoutToVerifyDto.CartId).FirstOrDefaultAsync();
            if (cart is not null && cart?.AuthorizationUrl is not null && cart?.TransReference is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_28bcec9e9f6804d68ad55eeed470a755a50b5c1a");
                var response = await _httpClient.GetAsync($"verify/{cartCheckoutToVerifyDto.Reference}");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(VerifyCheckoutDto);
                }
                var responseData = await response.Content.ReadFromJsonAsync<VerifyResponseBody>();
                if (response.IsSuccessStatusCode)
                {
                    String message;
                    var status = responseData.Data.Status;
                    if (status == "success")
                    {
                        message = "Payment has been confirmed.";
                        cart.TransStatus = 1;
                        cart.PaymentDate = DateTime.Now;
                        await _arikteeDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        message = "Payment could not be confirmed.";
                    }
                    return new VerifyCheckoutDto
                    {
                        Status = status,
                        Message = message
                    };
                }
                throw new HttpResponseException(400, responseData.Message);
            }
            throw new HttpResponseException(404, "The cart ID provided does not match an available cart.");
            //response.StatusCode
        }


        public async Task<bool> VerifyUserExists(String email)
        {
            return await _arikteeDbContext.Users.AnyAsync(e => e.Email == email);
        }
    }
}
