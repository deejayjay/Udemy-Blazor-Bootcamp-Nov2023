using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Tangy_Common;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
        )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<SignInResponseDto> LoginAsync(SignInRequestDto signInRequest)
        {
            var content = JsonConvert.SerializeObject(signInRequest);
            var body = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Account/SignIn", body);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignInResponseDto>(contentTemp);

            if (!response.IsSuccessStatusCode)
            {
                return result!;
            }

            await _localStorage.SetItemAsync(Sd.Local_Token, result!.Token);
            await _localStorage.SetItemAsync(Sd.Local_UserDetails, result!.User);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            ((AuthStateProvider)_authenticationStateProvider).NotifyUserLoggedIn(result.Token);

            return new SignInResponseDto { IsAuthSuccessful = true };
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(Sd.Local_Token);
            await _localStorage.RemoveItemAsync(Sd.Local_UserDetails);

            ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogout();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<SignUpResponseDto> RegisterUserAsync(SignUpRequestDto signUpRequest)
        {
            var content = JsonConvert.SerializeObject(signUpRequest);
            var body = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Account/SignUp", body);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignUpResponseDto>(contentTemp);

            if (!response.IsSuccessStatusCode)
            {
                return new SignUpResponseDto 
                { 
                    IsRegistrationSuccessful = false,
                    Errors = result!.Errors
                };
            }

            return new SignUpResponseDto { IsRegistrationSuccessful = true };
        }
    }
}
