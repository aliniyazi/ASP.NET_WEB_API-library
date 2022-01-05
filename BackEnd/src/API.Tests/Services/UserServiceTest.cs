using API.DataAccess.Models;
using API.Services.Requests;
using API.Services.Service;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using API.Services.Responses;
using System.Net;
using API.Common;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Tests.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private SignInManager<User> signInManager;
        private UserManager<User> userManager;
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "AppSettings:Secret", "SecretSecretSecretSecretSecretSecretSecretSecret" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            userManager = A.Fake<UserManager<User>>();
            signInManager = A.Fake<SignInManager<User>>();
            userService = new UserService(configuration, signInManager, userManager);
        }

        [Test]
        public async Task When_LoginAsync_Expect_ReturnToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var request = new LoginUserRequest()
            {
                Email = "",
                Password = ""
            };

            A.CallTo(() => signInManager.PasswordSignInAsync(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<bool>.Ignored)).Returns(Task.FromResult(SignInResult.Success));

            var result = await userService.LoginAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null);
            Assert.That(tokenHandler.CanReadToken(result.Token), Is.True);
        }

        [Test]
        public async Task When_RegisterAsync_Expect_ReturnSucceeded()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored)).Returns<User>(null);
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Success);
            A.CallTo(() => signInManager.PasswordSignInAsync(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<bool>.Ignored)).Returns(Task.FromResult(SignInResult.Success));

            var registerUserRequest = new RegisterUserRequest() { Email = "", Password = "" };
            var addressRequest = new AddressRequest();
            registerUserRequest.Address = addressRequest;

            var result = await userService.RegisterAsync(registerUserRequest);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task When_RegisterAsync_Expect_ReturnFailed()
        {
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Failed(new IdentityError()));

            AuthenticateResponse result = await userService.RegisterAsync(new RegisterUserRequest());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task When_RegisterLibrarianAsync_Expect_ReturnSucceeded()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored)).Returns<User>(null);
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Success);
            A.CallTo(() => signInManager.PasswordSignInAsync(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<bool>.Ignored)).Returns(Task.FromResult(SignInResult.Success));

            var registerUserRequest = new RegisterUserRequest() { Email = "", Password = "" };
            var addressRequest = new AddressRequest();
            registerUserRequest.Address = addressRequest;

            var expectedResult = new Response<string>(HttpStatusCode.OK, GlobalConstants.REGISTER_SUCCESS);
            var result = await userService.RegisterLibrarianAsync(registerUserRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<string>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_RegisterLibrarianAsync_Expect_ReturnFailed()
        {
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored)).Returns(IdentityResult.Failed(new IdentityError()));

            var result = await userService.RegisterLibrarianAsync(new RegisterUserRequest());
            var expectedResult = new Response<string>(HttpStatusCode.BadRequest,null, GlobalConstants.REGISTER_FAIL);

            Assert.That(result,Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<string>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }
        [Test]
        public async Task When_ResetPasswordAsync_Expect_True()
        {

            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored)).Returns<User>(new User());

            var result = await userService.ResetPasswordAsync("aliniyazi@abv.bg");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.True);

            var apiKey = GlobalConstants.SENDGRID_API_KEY;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("cherokee_2021@abv.bg", "The curious readers");
            var subject = "Password reset";
            var to = new EmailAddress("aliniyazi@abv.bg", "Cherokee Support Team");
            var plainTextContent = $"You password has been changed to 123456789";
            var htmlContent = $"<strong>You password has been changed to 123456789 </strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
        }

        [Test]
        public async Task When_ResetPasswordAsync_Expect_False()
        {
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored)).Returns<User>(null);

            var result = await userService.ResetPasswordAsync("aliniyazi@abv.bg");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task When_ChangePasswordAsync_Expect_True()
        {
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored)).Returns<User>(new User());
            var result = await userService.ChangePasswordAsync("aliniyazi@abv.bg", "123456789Aa!");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task When_ChangePasswordAsync_Expect_False()
        {
            A.CallTo(() => userManager.FindByEmailAsync(A<string>.Ignored)).Returns<User>(null);
            var result = await userService.ChangePasswordAsync("aliniyazi@abv.bg", "123456789Aa!");

            Assert.That(result, Is.False);
        }
    }
}
