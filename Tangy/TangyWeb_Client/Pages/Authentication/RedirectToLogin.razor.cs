using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class RedirectToLogin
    {
        [CascadingParameter]
        public Task<AuthenticationState>? AuthState { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        private bool NotAuthorized { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthState!;

            if (authState?.User?.Identity is null || !authState.User.Identity.IsAuthenticated) 
            {
                var returnUrl = NavigationManager!.ToBaseRelativePath(NavigationManager.Uri);

                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    NavigationManager.NavigateTo("login");
                }
                else
                {
                    NavigationManager.NavigateTo($"login?returnUrl={returnUrl}");
                }
            }
            else
            {
                NotAuthorized = true;
            }
        }
    }
}
