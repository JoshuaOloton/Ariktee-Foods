using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages
{
    public partial class Products
    {
        private String searchText = "";

        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductDto> FoodProducts { get; set; } = new List<ProductDto>();
        

        protected override async Task OnInitializedAsync()
        {
            FoodProducts = await ProductService.GetProducts();
            //FilteredProducts = FoodProducts.Where(e => e.ProductName.Contains(searchText)).ToList();
        }

        //protected void OnFilter_Click()
        //{
        //    FilteredProducts = FoodProducts.Where(e => e.ProductName.Contains(searchText)).ToList();
        //    Console.WriteLine(searchText);
        //}

        List<ProductDto> FilteredProducts => FoodProducts.Where(
            product => product.ProductName.ToLower().Contains(searchText.ToLower())).ToList();
    }
}
