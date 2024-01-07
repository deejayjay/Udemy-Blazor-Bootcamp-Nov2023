using Microsoft.AspNetCore.Components;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        public IAuthenticationService? AuthService { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthService!.LogoutAsync();
            NavigationManager!.NavigateTo("/");
        }
    }
}
