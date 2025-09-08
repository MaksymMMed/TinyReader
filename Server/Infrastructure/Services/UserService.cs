using Application.Common;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using Application.User.Query;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Enums;
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

        public UserService(UserManager<IdentityAppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async Task<Result<IdentityAppUser>> SignUp(string email, string name, string surname, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return Result<IdentityAppUser>.Fail("User with this email already exists")!;
            }
            var identityUser = new IdentityAppUser
            {
                Email = email,
                Name = name,
                Surname = surname,
            };
            var result = await _userManager.CreateAsync(identityUser, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<IdentityAppUser>.Fail($"Failed to create user: {errors}")!;
            }
            return Result<IdentityAppUser>.Success(identityUser);
        }

        public async Task<Result<TokenDto>> SignIn(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<TokenDto>.Fail("Invalid login or password")!;
            }
            var result = await _userManager.CheckPasswordAsync(user, password);
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


        public async Task<Result<TokenDto>> SignUpStudent(string email, string name, string surname, string password, string passwordRepeated)
        {
            if (password != passwordRepeated)
            {
                return Result<TokenDto>.Fail("Passwords do not match")!;
            }
            var result = await SignUp(email, name, surname, password);
            if (!result.IsSuccess)
                return Result<TokenDto>.Fail(result.Error!)!;

            var user = result.Value;

            if (await _roleManager.RoleExistsAsync(UserRoles.Student.ToString()))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Student.ToString());
            }

            return await SignIn(email, password);
        }

        public async Task<Result<TokenDto>> SignUpTeacher(string email, string name, string surname, string password, string passwordRepeated)
        {
            if (password != passwordRepeated)
            {
                return Result<TokenDto>.Fail("Passwords do not match")!;
            }
            var result = await SignUp(email, name, surname, password);
            if (!result.IsSuccess)
                return Result<TokenDto>.Fail(result.Error!)!;

            var user = result.Value;

            if (await _roleManager.RoleExistsAsync(UserRoles.Teacher.ToString()))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Teacher.ToString());
            }

            return await SignIn(email, password);
        }
    }
}
