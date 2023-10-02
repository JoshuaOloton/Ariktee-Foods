using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages
{
    public partial class DisplayProduct
    {
        [Parameter]
        public ProductDto Product { get; set; }

        public int BasePrice { get; set; }
        public String? BaseUnit { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        protected override void OnInitialized()
        {
            BasePrice = Product.ProductUnits.Min(e => e.Price);
            BaseUnit = Product.ProductUnits.Where(e => e.Price == BasePrice).Select(e => e.Unit).FirstOrDefault();
        }

        protected async Task AddToCart_Click(ProductDto productDto)
        {
            //var userId = await LocalStorage.GetItemAsStringAsync("UserId");
            //var currentCart = await ShoppingCartService.GetCurrentCart(Convert.ToInt32(userId));
            //if (currentCart is null)
            //{
            //    throw new Exception("Error fetching cart details.");
            //}
            //var newCartItem = await ShoppingCartService.AddCartItem(new CartItemToAddDto
            //{
            //    CartId = currentCart.Id,
            //    ProductId = productDto.Id,
            //    UnitId = productDto.ProductUnits.Where(e => e.Unit == selectedUnit).Select(e => e.Id).FirstOrDefault(),
            //    UnitAmount = productDto.ProductUnits.Where(e => e.Unit == selectedUnit).Select(e => e.Price).FirstOrDefault()
            //});
            //NavigationManager.NavigateTo("/ShoppingCart");
        }
    }
}
