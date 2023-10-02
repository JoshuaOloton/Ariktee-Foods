using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Principal;

namespace ArikteeFoods.Web
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            this._localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            var name = await _localStorage.GetItemAsStringAsync("Fullname");
            var state = new AuthenticationState(new ClaimsPrincipal());
            if (token != null && !String.IsNullOrWhiteSpace(token))
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim("authToken", token),
                    new Claim(ClaimTypes.Name, name ?? "")
                }, "tokenauth_type");
                state = new AuthenticationState(new ClaimsPrincipal(identity));
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            return state;
        }
    }
}
