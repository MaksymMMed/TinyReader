using Application.Common;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
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
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, UserManager<IdentityAppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private async Task<Result<IdentityAppUser>> SignUp(string email, string name, string surname, string password, UserRoles role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return Result<IdentityAppUser>.Fail("User with this email already exists")!;
            }
            var identityUser = new IdentityAppUser
            {
                Email = email,
                UserName = email,
            };
            var result = await _userManager.CreateAsync(identityUser, password);

            if (!result.Succeeded)
            {

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<IdentityAppUser>.Fail($"Failed to create user: {errors}")!;
            }

            if (role == UserRoles.Student)
            {
                var user = new Student
                {
                    Id = identityUser.Id,
                    Email = email,
                    Name = name,
                    Surname = surname,
                };
                _context.Students.Add(user);
            }
            else
            {
                var user = new Teacher
                {
                    Id = identityUser.Id,
                    Email = email,
                    Name = name,
                    Surname = surname,
                };
                _context.Teachers.Add(user);
            }
            await _context.SaveChangesAsync();

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
            var result = await SignUp(email, name, surname, password,UserRoles.Student);
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
            var result = await SignUp(email, name, surname, password, UserRoles.Teacher);
            if (!result.IsSuccess)
                return Result<TokenDto>.Fail(result.Error!)!;

            var user = result.Value;

            if (await _roleManager.RoleExistsAsync(UserRoles.Teacher.ToString()))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Teacher.ToString());
            }

            return await SignIn(email, password);
        }

        public Task<Result<bool>> DeleteUser(string password)
        {
            string email = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Email)!.Value;

            var user = _userManager.FindByEmailAsync(email).Result;

            if (user == null)
            {
                return Task.FromResult(Result<bool>.Fail("User not found")!);
            }

            var result = _userManager.CheckPasswordAsync(user, password).Result;
            if (!result)
            {
                return Task.FromResult(Result<bool>.Fail("Invalid password")!);
            }
            var deleteResult = _userManager.DeleteAsync(user).Result;
            if (!deleteResult.Succeeded)
            {
                var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                return Task.FromResult(Result<bool>.Fail($"Failed to delete user: {errors}")!);
            }
            return Task.FromResult(Result<bool>.Success(true));
        }

        public Task<Result<string>> GenerateConfirmationToken(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> GenerateResetPasswordToken(string email)
        {
            throw new NotImplementedException();
        }

        public Task SendConfirmationEmail(string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendResetPasswordEmail(string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ResetPassword(string email, string resetToken, string newPassword, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ConfirmEmail(string email, string confirmationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangeUserName(string newName, string newSurname)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ChangeEmail(string newEmail)
        {
            throw new NotImplementedException();
        }
    }
}
