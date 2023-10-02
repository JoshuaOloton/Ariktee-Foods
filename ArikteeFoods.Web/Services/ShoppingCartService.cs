using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Models.PaystackModels;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ArikteeFoods.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public event EventHandler<ShoppingCartEventArgs> ShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
        }

        public void OnShoppingCartChanged(ShoppingCartEventArgs e)
        {
            ShoppingCartChanged?.Invoke(this, e);
        }

        public async Task<ShoppingCartItemDto?> AddCartItem(CartItemToAddDto cartItemToAddDto)
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
                var response = await _httpClient.PostAsJsonAsync("/api/ShoppingCart/", cartItemToAddDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ShoppingCartItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<ShoppingCartItemDto>();
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
			catch (Exception)
			{
				throw;
			}
        }

        public async Task<List<ShoppingCartDto>?> GetAllCarts(int userId)
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
                var response = await _httpClient.GetAsync($"/api/ShoppingCart/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ShoppingCartDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<ShoppingCartDto>>();
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShoppingCartItemDto>?> GetItemsByCart(int cartId)
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
                var response = await _httpClient.GetAsync($"/api/ShoppingCart/GetItemsByCart/{cartId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ShoppingCartItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<ShoppingCartItemDto>>();
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShoppingCartItemDto>?> GetCartItems(int userId)
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
                var response = await _httpClient.GetAsync($"/api/ShoppingCart/{userId}/GetItems");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ShoppingCartItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<ShoppingCartItemDto>>();
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShoppingCartDto?> GetCurrentCart(int userId)
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
                var response = await _httpClient.GetAsync($"/api/ShoppingCart/{userId}/GetCart");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ShoppingCartDto);
                    }
                    return await response.Content.ReadFromJsonAsync<ShoppingCartDto>();
                }
                var errMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShoppingCartItemDto?> UpdateCartItem(int Id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
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
                var response = await _httpClient.PatchAsJsonAsync($"/api/ShoppingCart/{Id}", cartItemQtyUpdateDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ShoppingCartItemDto>();
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShoppingCartItemDto?> DeleteCartItem(int id)
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
                var response = await _httpClient.DeleteAsync($"/api/ShoppingCart/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ShoppingCartItemDto>();
                }
                return default(ShoppingCartItemDto);    // if del request is unsuccessful, return null
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<InitializeCheckoutDto?> InitializeCheckout(int cartId, TotalPayInfo payInfo)
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
                var response = await _httpClient.PostAsJsonAsync<TotalPayInfo>($"/api/Payments/{cartId}", payInfo);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(InitializeCheckoutDto);
                    }
                    var authorizeUrl = await response.Content.ReadFromJsonAsync<InitializeCheckoutDto>();
                    return authorizeUrl;
                }
                var errMsg = response.Content.ReadAsStringAsync();
                throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task VerifyLastCheckout(int userID)
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
                var response = await _httpClient.GetAsync($"/api/ShoppingCart/{userID}/GetLastCart");
                if (!response.IsSuccessStatusCode)
                {
                    var errMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return;

                var lastCart = await response.Content.ReadFromJsonAsync<ShoppingCartDto>();
                // If payment invoice has been generated, but payment has not been made, send request to verify endpoint
                if (lastCart?.AuthorizationUrl != null && lastCart?.TransReference != null && lastCart.TransStatus == 0)
                {
                    var checkoutToVerify = new CartCheckoutToVerifyDto
                    {
                        CartId = lastCart.Id,
                        Reference = lastCart.TransReference
                    };

                    response = await _httpClient.PostAsJsonAsync<CartCheckoutToVerifyDto>($"/api/Payments/VerifyCheckout", checkoutToVerify);
                    if (response.IsSuccessStatusCode)
                    {
                        // Payment is verified, and the cart record has been updated successfully in the db
                        return;
                    }
                    var errMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Status: {response.StatusCode}, Message: {errMsg}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
