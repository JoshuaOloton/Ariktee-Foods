using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace ArikteeFoods.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<ProductDto?> GetProduct(int Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Products/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }
                    var product = await response.Content.ReadFromJsonAsync<ProductDto>();
                    return product;
                }
                // if error status code
                var errMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(errMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var token =  "";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("/api/Products");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                    return products;
                }
                // if error status code
                var errMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(errMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
