using Application.Common;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public UserService(UserManager<IdentityAppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        private string HashPassword(string password, IdentityAppUser user)
        {
            var passwordHasher = new PasswordHasher<IdentityAppUser>();
            string hashedPassword = passwordHasher.HashPassword(user, password);
            return hashedPassword;
        }

        public async Task<Result<bool>> SignUp(AppUser user, string password, string passwordRepeated)
        {

            if (password != passwordRepeated)
            {
                throw new ArgumentException("Passwords do not match");
            }
            var identityUser = new IdentityAppUser
            {
                UserName = user.Email,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
            };
            identityUser.PasswordHash = HashPassword(password, identityUser);
            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Fail($"Failed to create user: {errors}");
            }
            return Result<bool>.Success(true);
        }

        public async Task<Result<TokenDto>> SignIn(string Email, string password)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Result<TokenDto>.Fail("Invalid login or password")!;
            }
            var hashedPassword = HashPassword(password, user);
            var result = await _userManager.CheckPasswordAsync(user, hashedPassword);
            if (!result)
            {
                return Result<TokenDto>.Fail("Invalid login or password")!;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            TokenDto tokenDto = new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return Result<TokenDto>.Success(tokenDto);
        }
    }
}
