using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages
{
    public partial class ShoppingCart
    {
        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; }
        public String? ErrorMessage { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        protected int TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var userId = await LocalStorage.GetItemAsStringAsync("UserId");
                ShoppingCartItems = await ShoppingCartService.GetCartItems(Convert.ToInt32(userId));

                CartChanged();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ShoppingCartItemDto? GetCartItem(int Id)
        {
            return ShoppingCartItems?.FirstOrDefault(e => e.Id == Id);
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Select(e => (e.ProductUnitPrice * e.Qty)).Sum();
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Select(e => e.Qty).Sum();
        }

        private void CartChanged()
        {
            CalculateCartSummary();
            ShoppingCartService.OnShoppingCartChanged(new ShoppingCartEventArgs(TotalQuantity));
        }

        private void CalculateCartSummary()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        // UPDATE/REMOVE CART ITEM IN THE UI BEFORE ACCESSING THE DB TO PREVENT UI LAG

        protected async void UpdateQty_Click(ShoppingCartItemDto shoppingCartItemDto, int updateBy)
        {
            if (shoppingCartItemDto.Qty + updateBy > 0)
            {
                int newQty = shoppingCartItemDto.Qty + updateBy;
                UpdateCart_UI(shoppingCartItemDto, newQty);     // CALL UPDATE CART ITEM UI FUNCTION BEFORE DB ENTITY UPDATE

                await ShoppingCartService.UpdateCartItem(shoppingCartItemDto.Id, new CartItemQtyUpdateDto
                {
                    CartItemId = shoppingCartItemDto.Id,
                    Qty = newQty
                });
            }
        }

        protected async void DeleteCartItem_Click(ShoppingCartItemDto cartItem)
        {
            DeleteCartItem_UI(cartItem);    // CALL DELETE CART ITEM UI FUNCTION BEFORE DB ENTITY DELETE

            await ShoppingCartService.DeleteCartItem(cartItem.Id);
        }

        private void UpdateCart_UI(ShoppingCartItemDto shoppingCartItemDto, int Qty)
        {
            var cartItemToUpdate = GetCartItem(shoppingCartItemDto.Id);
            if (cartItemToUpdate != null)
            {
                cartItemToUpdate.Qty = Qty;

                CartChanged();
            }
        }

        private void DeleteCartItem_UI(ShoppingCartItemDto shoppingCartItemDto)
        {
            if (GetCartItem(shoppingCartItemDto.Id) != null)
            {
                ShoppingCartItems.Remove(shoppingCartItemDto);

                CartChanged();
            }
        }
    }
}
