using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace ArikteeFoods.Web.Pages.Account
{
    public partial class Login
    {
        private UserLogin userLogin = new();
        private AuthenticationState authenticationState;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private string loginButtonText = "Login";
        private bool isLoggingIn = false;

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


        protected async Task LoginUser()
        {
            try
            {
                // Change login button text and disable button
                loginButtonText = "Logging in...";
                isLoggingIn = true;

                var user = await AuthService.LoginUser(userLogin);

                /* Set injected value to true so blazored toast welcomes user after log in 
                and Add email and authtoken to localstorage */
                AuthService.NewLogin = true;

                
                await LocalStorage.SetItemAsStringAsync("authToken", user.AccessToken);
                await LocalStorage.SetItemAsStringAsync("Email Address", userLogin.Email);
                await LocalStorage.SetItemAsStringAsync("Fullname", user.Fullname);
                await LocalStorage.SetItemAsStringAsync("UserId", user.Id.ToString());

                // Trigger getauthenticationstate event and Add Bearer token authorization header
                await AuthenticationStateProvider.GetAuthenticationStateAsync();

                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);
                NavigationManager.NavigateTo("/home");
            }
            catch (Exception)
            {
                ToastService.ShowError("Please check your login details and try again.");
            }
        }
    }
}
