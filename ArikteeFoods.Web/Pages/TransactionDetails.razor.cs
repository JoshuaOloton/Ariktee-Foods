using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages
{
    public partial class TransactionDetails
    {
        [Parameter]
        public int ID { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public IEnumerable<ShoppingCartItemDto>? TransDetails { get; set; }

        public String? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TransDetails = await ShoppingCartService.GetItemsByCart(ID);
        }

    }
}
