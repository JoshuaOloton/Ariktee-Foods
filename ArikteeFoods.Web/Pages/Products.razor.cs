using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Principal;

namespace ArikteeFoods.Web.Pages
{
    public partial class Products
    {
        private String searchText = "";

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IAuthService AuthService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        public String ErrorMessage { get; set; }

        public IEnumerable<ProductDto> FoodProducts { get; set; } = new List<ProductDto>();
        

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (AuthService.NewLogin == true)
                {
                    ToastService.ShowSuccess("Welcome to Ariktee Foods. You are now logged in");
                    AuthService.NewLogin = false;
                }
                FoodProducts = await ProductService.GetProducts();

                // Trigger ShoppingCartChangedEvent on oninitialised to display the total cart quantity in CartIcon
                var userId = await LocalStorage.GetItemAsStringAsync("UserId");
                var shoppingItems = await ShoppingCartService.GetCartItems(Convert.ToInt32(userId));
                int totalQuantity = shoppingItems.Sum(e => e.Qty);
                ShoppingCartService.OnShoppingCartChanged(new ShoppingCartEventArgs(totalQuantity));

                await ShoppingCartService.VerifyLastCheckout(Convert.ToInt32(userId));
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        async Task Alert()
        {
            await JSRuntime.InvokeVoidAsync("Call");
        }

        List<ProductDto> FilteredProducts => FoodProducts.Where(
            product => product.ProductName.ToLower().Contains(searchText.ToLower())).ToList();
    }
}
