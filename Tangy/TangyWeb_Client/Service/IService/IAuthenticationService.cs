using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IAuthenticationService
    {
        Task<SignUpResponseDto> RegisterUserAsync(SignUpRequestDto requestDto);
        Task<SignInResponseDto> LoginAsync(SignInRequestDto requestDto);
        Task LogoutAsync();
    }
}
