using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_API.Helper;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApiSettings _apiSettings;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<ApiSettings> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _apiSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto? request)
        {
            if (request is null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            // User creation failed
            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDto()
                {
                    IsRegistrationSuccessful = false,
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            // If user creation succeeded, assign role to user
            var roleResult = await _userManager.AddToRoleAsync(user, Sd.Role_Customer);

            // Role assignment failed
            if (!roleResult.Succeeded)
            {
                return BadRequest(new SignUpResponseDto()
                {
                    IsRegistrationSuccessful = false,
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto? request)
        {
            if (request is null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

            // Authentication failed
            if (!result.Succeeded)
            {
                return Unauthorized(new SignInResponseDto()
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Login Attempt"
                });
            }

            // Authentication succeeded
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null)
            {
                return Unauthorized(new SignInResponseDto()
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Login Attempt"
                });
            }

            // User has logged in successfully. Generate JWT token, then return it to the client with user info
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync(user);

            var jwtToken = new JwtSecurityToken(
                issuer: _apiSettings.ValidIssuer,
                audience: _apiSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signingCredentials);

            // Generate a JWT token string using the given token options
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(new SignInResponseDto()
            {
                IsAuthSuccessful = true,
                Token = token,
                User = new UserDto()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber
                }
            });
        }

        // Creates a symmetric security key from the secret key stored in appsettings.json. This key 
        // is used both for signing the JWT on the server side and verifying the signature on the client side.
        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        // Returns a list of claims for the given user. These claims are used to generate the JWT token.
        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
