using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ArikteeFoods.Web.Pages
{
    public partial class Index
    {
        private String searchText = "";
        private AuthenticationState authenticationState;

        [Inject]
        public IAuthService AuthService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        public String ErrorMessage { get; set; }

        public IEnumerable<ProductDto> FoodProducts { get; set; } = new List<ProductDto>();


        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (AuthService.NewSignout == true)
                {
                    ToastService.ShowError($"You have logged out. To resume shopping, please log in again.");
                    AuthService.NewLogin = false;
                }
                FoodProducts = await ProductService.GetProducts();

                authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (authenticationState.User.Identity != null && authenticationState.User.Identity.IsAuthenticated)
                {
                    NavigationManager.NavigateTo("/home");
                }
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

        protected int GetBasePrice(ProductDto product)
        {
            return product.ProductUnits.Min(e => e.Price);
        }

        List<ProductDto> FilteredProducts => FoodProducts.Where(
            product => product.ProductName.ToLower().Contains(searchText.ToLower())).ToList();
    }
}
