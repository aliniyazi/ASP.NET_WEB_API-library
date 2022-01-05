using API.Common;
using API.DataAccess.Models;
using API.Services.Mappers;
using API.Services.Requests;
using API.Services.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using API.Services.Responses;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Service
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public UserService(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<AuthenticateResponse> LoginAsync(LoginUserRequest userInput)
        {
            var result = await signInManager.PasswordSignInAsync(userInput.Email, userInput.Password, false, false);
            if (result.Succeeded)
            {
                var generateToken = GenerateJwtToken(userInput);
                var token = new AuthenticateResponse(await generateToken);
                return token;
            }
            return null;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<AuthenticateResponse> RegisterAsync(RegisterUserRequest request)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                var user = Mapper.MapFrom(request);

                string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, confirmEmailToken);

                var result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.User);

                    return await LoginAsync(new LoginUserRequest() { Email = user.Email, Password = request.Password });
                }
            }

            return null;
        }

        public async Task<Response<string>> RegisterLibrarianAsync(RegisterUserRequest request)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                var user = Mapper.MapFrom(request);

                string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, confirmEmailToken);

                var result = await userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var roles = new string[] { Roles.User, Roles.Librarian };
                    await userManager.AddToRolesAsync(user, roles);

                    return new Response<string>(HttpStatusCode.OK, GlobalConstants.REGISTER_SUCCESS);
                }
            }

            return new Response<string>(HttpStatusCode.BadRequest, null, GlobalConstants.REGISTER_FAIL);
        }

        /// <summary>
        /// Generate basic Access tokent which exprires in one day for testing front/end and QA 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>AccessToken</returns>
        private async Task<string> GenerateJwtToken(LoginUserRequest user)
        {
            var claims = new List<Claim>
            {
                new Claim("email", user.Email.ToString()),
            };

            var userRole = await userManager.FindByEmailAsync(user.Email);
            var roles = await userManager.GetRolesAsync(userRole);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = configuration.GetSection("AppSettings").GetSection("Secret");
            var key = Encoding.UTF8.GetBytes(jwtSecret.Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration.GetSection("AppSettings").GetSection("Issuer").Value,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<int> GetNumOfUsersAsync()
        {
            return userManager.Users.Count();
        }
        public async Task<bool> ResetPasswordAsync(string email)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                Random rnd = new Random();
                var newPasswordNumberPart = rnd.Next(10000000, 99999999);
                var newPassword = newPasswordNumberPart + "Che!";

                await ResetPasswordTokenAsync(existingUser, newPassword);
                try
                {
                    var apiKey = GlobalConstants.SENDGRID_API_KEY;
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress("cherokee_2021@abv.bg", "The curious readers");
                    var subject = "Password reset";
                    var to = new EmailAddress(email, "Cherokee Support Team");
                    var plainTextContent = $"You password has been changed to {newPassword}";
                    var htmlContent = $"<strong>You password has been changed to {newPassword}</strong>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                    if (response.IsSuccessStatusCode) { return true; }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }
        public async Task<bool> ChangePasswordAsync(string email, string password)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await ResetPasswordTokenAsync(existingUser, password);
                return true;
            }
            return false;
        }
        public async Task ResetPasswordTokenAsync(User targetUser, string password)
        {
            string resetToken = await userManager.GeneratePasswordResetTokenAsync(targetUser);
            await userManager.ResetPasswordAsync(targetUser, resetToken, password);
        }
        //public async Task<User[]> GetAllUsersAsync(int UsersPerPage, int Page)
        //{

        //    var Users = userManager.Users.ToList();
        //}
    }
}
