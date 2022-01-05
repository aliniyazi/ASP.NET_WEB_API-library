using API.Common;
using API.Controllers;
using API.Services.Requests;
using API.Services.Responses;
using API.Services.ServiceContracts;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace API.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private IUserService userService;
        private UserController userController;

        [SetUp]
        public void SetUp()
        {
            userService = Substitute.For<IUserService>();
            userController = new UserController(userService);
        }

        [Test]
        public async Task When_Login_Expect_Ok()
        {
            this.userService.LoginAsync(Arg.Any<LoginUserRequest>()).Returns(Task.FromResult(new AuthenticateResponse("success")));

            var result = await userController.Login(new LoginUserRequest());

            Assert.That(result, Is.Not.Null);

            var response = result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var responseValue = response.Value as AuthenticateResponse;

            Assert.That(responseValue.Token, Is.EqualTo("success"));
        }

        [Test]
        public async Task When_Login_Expect_BadRequest()
        {
            userService.LoginAsync(Arg.Any<LoginUserRequest>()).Returns(Task.FromResult<AuthenticateResponse>(null));

            var result = await userController.Login(new LoginUserRequest());

            Assert.That(result, Is.Not.Null);

            var response = result as BadRequestObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

            var responseValue = response.Value as string;

            Assert.That(responseValue, Is.EqualTo(GlobalConstants.LOG_IN_ERROR));
        }

        [Test]
        public async Task When_Register_Expect_Ok()
        {
            this.userService.RegisterAsync(Arg.Any<RegisterUserRequest>()).Returns(Task.FromResult(new AuthenticateResponse("success")));

            var result = await userController.Register(new RegisterUserRequest());

            Assert.That(result, Is.Not.Null);

            var response = result as OkObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var responseValue = response.Value as AuthenticateResponse;

            Assert.That(responseValue.Token, Is.EqualTo("success"));
        }

        [Test]
        public async Task When_Register_Expect_BadRequest()
        {
            userService.LoginAsync(Arg.Any<LoginUserRequest>()).Returns(Task.FromResult<AuthenticateResponse>(null));

            var result = await userController.Register(new RegisterUserRequest());

            Assert.That(result, Is.Not.Null);

            var response = result as BadRequestObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

            var responseValue = response.Value as string;

            Assert.That(responseValue, Is.EqualTo(GlobalConstants.REGISTER_ERROR));
        }

        [Test]
        public async Task When_RegisterLibrarian_Expect_Ok()
        {
            userService.RegisterLibrarianAsync(Arg.Any<RegisterUserRequest>()).ReturnsForAnyArgs(new Response<string>(HttpStatusCode.OK, GlobalConstants.REGISTER_SUCCESS));

            var result = await userController.RegisterLibrarian(new RegisterUserRequest());

            Assert.That(result, Is.Not.Null);

            var response = result as OkObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task When_RegisterLibrarian_Expect_BadRequest()
        {
            userService.RegisterLibrarianAsync(Arg.Any<RegisterUserRequest>()).ReturnsForAnyArgs(new Response<string>(HttpStatusCode.BadRequest, GlobalConstants.REGISTER_FAIL));

            var result = await userController.RegisterLibrarian(new RegisterUserRequest());

            Assert.That(result, Is.Not.Null);

            var response = result as BadRequestObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
