using Microsoft.AspNetCore.Components;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Register
    {
        [Inject]
        public IAuthenticationService? AuthService { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        private readonly SignUpRequestDto signUpRequest = new();
        
        private bool ShowRegistrationErrors { get; set; }
        private IEnumerable<string> Errors { get; set; } = new List<string>();

        private async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
           
            var result = await AuthService!.RegisterUserAsync(signUpRequest);

            if (result.IsRegistrationSuccessful)
            {
                // Registration successful, redirect to home page
                NavigationManager!.NavigateTo("/login");
            }
            else
            {
                // Registration failed, display errors
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
        }
    }
}