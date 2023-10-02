using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ArikteeFoods.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private bool newLogin;
        private bool newSignout;
        private bool loginRequired;

        public AuthService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public bool NewLogin 
        {
            get { return newLogin; }
            set { newLogin = value; }
        }

        public bool NewSignout
        {
            get { return newSignout; }
            set { newSignout = value; }
        }

        public bool LoginRequired
        {
            get { return loginRequired; }
            set { loginRequired = value; }
        }

        public async Task<UserDto?> GetUser(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/User/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDto);
                    }
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    return user;
                }
                throw new Exception("Error fetching user.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoggedInUserDto?> LoginUser(UserLogin userLogin)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserLogin>("/api/User/login", userLogin);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(LoggedInUserDto);
                    }
                    var user = await response.Content.ReadFromJsonAsync<LoggedInUserDto>();
                    return user;
                }
                throw new Exception("Error signing in user.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto?> RegisterUser(UserRegister userRegister)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserRegister>("/api/User/register", userRegister);
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
