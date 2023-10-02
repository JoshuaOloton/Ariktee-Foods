using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace ArikteeFoods.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ProductService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
        }
        public async Task<ProductDto?> GetProduct(int Id)
        {
            try
            {
                // Add Bearer token authorization header
                /* if token is expired or about to expire, refresh token */
                var token = await _localStorage.GetItemAsStringAsync("authToken");

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                var expiration = jsonToken.ValidTo;

                var currentTime = DateTime.UtcNow;
                bool tokenIsExpiredOrAboutToExpire = expiration <= currentTime;

                if (tokenIsExpiredOrAboutToExpire)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "/api/User/refresh-token");
                    var tokenResponse = await _httpClient.SendAsync(request);
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenResult = await tokenResponse.Content.ReadFromJsonAsync<RefreshTokenDto>();
                        if (tokenResult is not null)
                        {
                            token = tokenResult.AccessToken;
                            await _localStorage.SetItemAsStringAsync("authToken", token);
                        }
                    }
                    // IMPLEMENT LOGOUT LOGIC HERE
                    // .. 
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
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
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
