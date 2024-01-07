using Microsoft.AspNetCore.Components;
using System.Web;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Login
    {
        [Inject]
        public IAuthenticationService? AuthService { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        public string? ReturnUrl { get; set; }

        private readonly SignInRequestDto signInRequest = new();
        private bool ShowSignInErrors { get; set; }
        private string Error { get; set; } = string.Empty;


        private async Task LoginUser()
        {
            ShowSignInErrors = false;

            var result = await AuthService!.LoginAsync(signInRequest);

            if (result.IsAuthSuccessful)
            {
                // Login successful
                var absoluteUri = new Uri(NavigationManager!.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];

                if (string.IsNullOrWhiteSpace(ReturnUrl))
                {
                    NavigationManager!.NavigateTo("/");
                }
                else
                {
                    NavigationManager!.NavigateTo(ReturnUrl);
                }
            }
            else
            {
                // Login failed, display errors
                Error = result.ErrorMessage;
                ShowSignInErrors = true;
            }
        }


    }
}
