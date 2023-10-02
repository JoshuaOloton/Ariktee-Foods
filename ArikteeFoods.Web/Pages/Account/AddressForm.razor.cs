using ArikteeFoods.Web.Models;
using ArikteeFoods.Web.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace ArikteeFoods.Web.Pages.Account
{
    public partial class AddressForm
    {
        private NewAddress newAddress = new ();

        protected void AddAddress()
        {
            try
            {
                // Login user
                
            }
            catch (Exception)
            {
                //ToastService.ShowError("Please check your login details and try again.");
            }
        }
    }
}
