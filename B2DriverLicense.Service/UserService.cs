using B2DriverLicense.Core.Dtos;
using B2DriverLicense.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace B2DriverLicense.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
        }

        public async Task<string> Authenticate(UserLoginDto user)
        {
            var existing = await _userManager.FindByNameAsync(user.UserName);
            if (existing == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(existing, user.Password, user.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(existing);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, existing.UserName),
                new Claim(ClaimTypes.Name, existing.LastName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["Tokens:Issuer"],
                    audience: _configuration["Tokens:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Register(UserRegisterDto user)
        {
            var entity = new AppUser
            {
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };
            var result = await _userManager.CreateAsync(entity, user.Password);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
    }
}
