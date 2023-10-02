using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace ArikteeFoods.Web.Pages.Account
{
    public partial class Register
    {
        private UserRegister userRegister = new();
        private AuthenticationState authenticationState;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public IAuthService AuthService { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthService.LoginRequired)
            {
                ToastService.ShowError("Please log in to continue.");
                AuthService.LoginRequired = false;
            }

            authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authenticationState.User.Identity != null && authenticationState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/home");
            }
        }


        protected async Task RegisterUser()
        {
            try
            {
                // Register user
                Console.WriteLine("Register user..");
                var user = await AuthService.RegisterUser(userRegister);

                var loggedInUser = await AuthService.LoginUser(new UserLogin
                {
                    Email = userRegister.Email,
                    Password = userRegister.Password
                });

                // Set injected value to true so blazored toast welcomes user after log in
                AuthService.NewLogin = true;

                // Add email and authtoken to localstorage
                await LocalStorage.SetItemAsStringAsync("authToken", loggedInUser.AccessToken);
                await LocalStorage.SetItemAsStringAsync("Email Address", userRegister.Email);
                await LocalStorage.SetItemAsStringAsync("Fullname", loggedInUser.Fullname);
                await LocalStorage.SetItemAsStringAsync("UserId", loggedInUser.Id.ToString());

                // Trigger getauthenticationstate event
                await AuthenticationStateProvider.GetAuthenticationStateAsync();

                // Add Bearer token authorization header
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedInUser.AccessToken);
                NavigationManager.NavigateTo("/home");
            }
            catch (Exception)
            {
                ToastService.ShowError("This email address already exists. Please login if you have an account with us.");
            }
        }
    }
}
