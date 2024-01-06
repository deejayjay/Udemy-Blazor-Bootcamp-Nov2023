using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using Tangy_Common;
using TangyWeb_Client.Helpers;

namespace TangyWeb_Client.Service
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwt = await _localStorage.GetItemAsync<string>(Sd.Local_Token);

            if (string.IsNullOrWhiteSpace(jwt))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));                
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwt);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(jwt), "jwtAuthType")));
        }
    }
}
