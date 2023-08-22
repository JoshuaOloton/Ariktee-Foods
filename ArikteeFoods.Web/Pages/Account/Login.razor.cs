using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services.Contracts;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace ArikteeFoods.Web.Pages.Account
{
    public partial class Login
    {
        private UserLogin userLogin = new();

        [Inject]
        public IAuthService AuthService { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        protected async Task LoginUser()
        {
            // Login user
            Console.WriteLine("Login user..");
            await AuthService.LoginUser(userLogin);
            await LocalStorage.SetItemAsync<String>("Email Address", userLogin.Email);
            NavigationManager.NavigateTo("");
        }
    }
}
