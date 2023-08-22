using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages
{
    public partial class ProductDetails
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Parameter]
        public int Id { get; set; }
        public ProductDto FoodProduct { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FoodProduct = await ProductService.GetProduct(Id);
        }
    }
}
