using ArikteeFoods.API.Data;
using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ArikteeDbContext _arikteeDbContext;

        public ShoppingCartRepository(ArikteeDbContext arikteeDbContext)
        {
            this._arikteeDbContext = arikteeDbContext;
        }

        private async Task<bool> IsItemExists(CartItemToAddDto cartItemToAddDto)
        {
            return await _arikteeDbContext.CartItems.AnyAsync(e => 
            e.CartId == cartItemToAddDto.CartId && e.ProductId == cartItemToAddDto.ProductId);
        }

        public async Task<VwCartItem?> GetItem(int Id) => await _arikteeDbContext.VwCartItems.Where(e => e.Id == Id).FirstOrDefaultAsync();

        public async Task<IEnumerable<VwCartItem>?> GetItems(int userId)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
            if (user == null) return null;
            return await _arikteeDbContext.VwCartItems.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await IsItemExists(cartItemToAddDto) == false)
            {
                var newItem = await (from product in _arikteeDbContext.Products    // CHECK IF PRODUCT EXISTS FOR PRODUCT ID
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = cartItemToAddDto.ProductId,
                                      Qty = 1
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

    }
}
