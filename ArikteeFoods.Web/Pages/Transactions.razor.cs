using ArikteeFoods.Web.Services.Contracts;
using ArikteeFoods.Models.DTOs;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using ArikteeFoods.Web.Services;

namespace ArikteeFoods.Web.Pages
{
    public partial class Transactions
    {
        public List<ShoppingCartDto>? ShoppingCarts { get; set; }

        public String? ErrorMessage { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var userId = await LocalStorage.GetItemAsStringAsync("UserId");
            ShoppingCarts = await ShoppingCartService.GetAllCarts(Convert.ToInt32(userId));
            if (ShoppingCarts != null)
            {
                foreach (var cart in ShoppingCarts)
                {
                    cart.SubTotal = await GetTotalPrice(cart.Id);
                }
            }

            var shoppingItems = await ShoppingCartService.GetCartItems(Convert.ToInt32(userId));
            int totalQuantity = shoppingItems.Sum(e => e.Qty);
            ShoppingCartService.OnShoppingCartChanged(new ShoppingCartEventArgs(totalQuantity));
        }

        private void FetchData()
        {
            //var result = 
        }

        protected void OpenTransactionDetails(int cartID)
        {
            var parameters = new ModalParameters().Add(nameof(TransactionDetails.ID), cartID);
            Modal.Show<TransactionDetails>("Transaction Details", parameters);
        }

        protected async Task<int> GetTotalPrice(int Id)
        {
            var cartItems = await ShoppingCartService.GetItemsByCart(Id);
            var subTotal = cartItems.Sum(e => e.TotalPrice);
            return subTotal;
        }

        protected String GetOrderStatus(int index)
        {
            List<String> status = new()
            {
                "Pending Payment", "Delivery In Progress", "Order Delivered"
            };
            return status[index];
        }

        protected void GoToCheckout()
        {
            NavigationManager.NavigateTo("/Checkout");
        }
    }
}
