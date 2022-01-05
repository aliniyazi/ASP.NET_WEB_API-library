using API.Services.Requests;
using API.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Threading.Tasks;

namespace API.Tests.Validators
{
    [TestFixture]
    public class AuthenticateRequestValidatorTests
    {
        private AuthenticateRequestValidator validator;
        [SetUp]
        public void SetUp()
        {
            validator = new AuthenticateRequestValidator();
        }

        [Test]
        public async Task When_EmailNull_ShouldHaveError()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = null };
            var result = await validator.TestValidateAsync(loginUserRequestModel);
            
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email can not be null");
        }

        [Test]
        public async Task When_EmailEmpty_ShouldHaveError()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = "" };
            var result = await validator.TestValidateAsync(loginUserRequestModel);
            
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email can not be Empty");
        }

        [Test]
        public async Task When_EmailInvalidFormat_ShouldHaveError()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = "testabv.bg" };
            var result = await validator.TestValidateAsync(loginUserRequestModel);
            
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email Address");
        }

        [Test]
        public async Task When_EmailAndPasswordValidFormat_ShouldValidateSuccessfully()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = "test@abv.bg", Password = "Parola123!" };
            var result = await validator.TestValidateAsync(loginUserRequestModel);
            
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task When_PasswordNull_ShouldHaveError()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = "test@abv.bg", Password = null };
            var result = await validator.TestValidateAsync(loginUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password can not be null");
        }

        [Test]
        public async Task When_PasswordEmpty_ShouldHaveError()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = "test@abv.bg", Password = "" };
            var result = await validator.TestValidateAsync(loginUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password can not be empty");
        }

        [Test]
        public async Task When_PasswordLengthLessThanTenSymbols_ShouldHaveError()
        {
            var loginUserRequestModel = new LoginUserRequest { Email = "test@abv.bg", Password = "Parola" };
            var result = await validator.TestValidateAsync(loginUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must be at least 10 characters long");
        }
    }
}
