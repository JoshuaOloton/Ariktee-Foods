using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ArikteeFoods.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public Task<UserDto> CreateUser()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto?> LoginUser(UserLogin userLogin)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserLogin>("/api/auth/login", userLogin);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDto);
                    }
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    return user;
                }
                throw new Exception("Error signing in user.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<UserDto> LogoutUser()
        {
            throw new NotImplementedException();
        }
    }
}
