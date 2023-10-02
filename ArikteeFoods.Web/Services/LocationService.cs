using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ArikteeFoods.Web.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public LocationService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
        }
        public async Task<UserDto?> AddAddress(int userId, AddressToAddDto addressToAddDto)
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
                var response = await _httpClient.PostAsJsonAsync<AddressToAddDto>($"/api/User/address/{userId}", addressToAddDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDto);
                    }
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    return user;
                }

                /* if error status code occurs,
                 * parse Json using its structure to obtain error details */
                var errJson = await response.Content.ReadAsStringAsync();

                JsonDocument doc = JsonDocument.Parse(errJson);
                JsonElement root = doc.RootElement;
                JsonElement errorElement = root.GetProperty("errors");
                JsonElement errorDetails = errorElement.GetProperty("City");

                String errorMessage = errorDetails[0].ToString();
                throw new Exception(errorMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<CityDto>> GetCities()
        {
            throw new NotImplementedException();
        }
    }
}
