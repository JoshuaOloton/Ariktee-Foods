using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Models.PaystackModels;
using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages
{
    public partial class Checkout
    {
        private SelectAddress selectAddress = new();
        private NewAddress newAddress = new();
        protected decimal PaymentAmount { get; set; }
        protected bool ShowAddressForm { get; set; } = false;

        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; }

        public UserDto ShoppingCartUser { get; set; }

        public String? ErrorMessage { get; set; }

        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public ILocationService LocationService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public ILocalStorageService LocalStorage { get; set; }
        [Inject]
        public IToastService ToastService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected int TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = await LocalStorage.GetItemAsStringAsync("UserId");
            ShoppingCartItems = await ShoppingCartService.GetCartItems(Convert.ToInt32(userId));

            ShoppingCartUser = await AuthService.GetUser(Convert.ToInt32(userId));

            PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
        }

        protected async Task ProceedToCheckout(int cartID)
        {
            try
            {
                var email = await LocalStorage.GetItemAsStringAsync("Email Address");
                // ApplicantPayInfo contains property definitions for 
                ApplicantPayInfo applicantPayInfo = new
                (
                    email: email,
                    amount: (ShoppingCartItems.Sum(e => e.TotalPrice) * 100).ToString(),
                    callback_url: "https://www.google.com"
                );
                var paystackReturn = await ShoppingCartService.InitializeCheckout(cartID, new TotalPayInfo
                {
                    PayInfo = applicantPayInfo,
                    Address = selectAddress.DeliveryAddress
                });
                if (paystackReturn != null)
                {
                    ToastService.ShowSuccess("Payment link generated.");
                    NavigationManager.NavigateTo(paystackReturn.Authorization_url);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void CartChanged()
        {
            CalculateCartSummary();
            ShoppingCartService.OnShoppingCartChanged(new ShoppingCartEventArgs(TotalQuantity));
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Select(e => (e.ProductUnitPrice * e.Qty)).Sum();
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Select(e => e.Qty).Sum();
        }

        private void CalculateCartSummary()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        protected async Task AddAddress()
        {
            try
            {
                var userId = await LocalStorage.GetItemAsStringAsync("UserId");
                await LocationService.AddAddress(Convert.ToInt32(userId), new AddressToAddDto
                {
                    HouseAddress = newAddress.StreetAddress,
                    City = newAddress.City
                });
                ShowAddressForm = false;
                ToastService.ShowSuccess("Your address has been added successfully.");
                UpdateAddress_UI(newAddress.StreetAddress, newAddress.City);
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private void UpdateAddress_UI(String address, String city)
        {
            ShoppingCartUser.DeliveryAddresses.Add(new AddressDto
            {
                City = city,
                HouseAddress = address
            });
        }
    }


}
