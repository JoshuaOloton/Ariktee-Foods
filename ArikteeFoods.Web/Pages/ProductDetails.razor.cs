using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using ArikteeFoods.Web.Services.Contracts;
using ArikteeFoods.Web.Services;
using Blazored.LocalStorage;

namespace ArikteeFoods.Web.Pages
{
    public partial class ProductDetails
    {
        private String? selectedUnit;

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        public String? ErrorMessage { get; set; }

        public ProductDto? FoodProduct { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                FoodProduct = await ProductService.GetProduct(Id);
                selectedUnit = FoodProduct.ProductUnits.Select(e => e.Unit).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected void GoToHome()
        {
            NavigationManager.NavigateTo("/home");
        }

        protected async Task AddToCart_Click(ProductDto productDto)
        {
            var userId = await LocalStorage.GetItemAsStringAsync("UserId");
            var currentCart = await ShoppingCartService.GetCurrentCart(Convert.ToInt32(userId));
            if (currentCart is null)
            {
                throw new Exception("Error fetching cart details.");
            }
            var newCartItem = await ShoppingCartService.AddCartItem(new CartItemToAddDto
            {
                CartId = currentCart.Id,
                ProductId = productDto.Id,
                UnitId = productDto.ProductUnits.Where(e => e.Unit == selectedUnit).Select(e => e.Id).FirstOrDefault(),
                UnitAmount = productDto.ProductUnits.Where(e => e.Unit == selectedUnit).Select(e => e.Price).FirstOrDefault()
            });
            NavigationManager.NavigateTo("/ShoppingCart");
        }
    }
}
