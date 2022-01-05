using API.Services.Requests;
using API.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Threading.Tasks;

namespace API.Tests.Validators
{
    [TestFixture]
    class RegisterRequestValidatorTests
    {
        private RegisterRequestValidator validator;
        private RegisterUserRequest  registerUserRequestModel;
       
        [SetUp]
        public void SetUp()
        {
            validator = new RegisterRequestValidator();
            registerUserRequestModel = new RegisterUserRequest
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "test@abv.bg",
                Password = "Parola123!",
                ConfirmPassword = "Parola123!",
                PhoneNumber = "0893100100",
                Address = new AddressRequest
                {
                    Country = "Bulgaria",
                    City = "Sofia",
                    StreetName = "Test street",
                    StreetNumber = "5",
                },

            };
        }

        [Test]
        public async Task When_FirstNameNull_ShouldHaveError()
        {
            registerUserRequestModel.FirstName = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("FirstName can not be null");
        }

        [Test]
        public async Task When_FirstNameEmpty_ShouldHaveError()
        {
            registerUserRequestModel.FirstName = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("FirstName can not be empty");
        }

        [Test]
        public async Task When_FirstNameLessThanTwoSymbol_ShouldHaveError()
        {
            registerUserRequestModel.FirstName = "A";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("FirstName must be at least two characters");
        }

        [Test]
        public async Task When_FirstNameMoreThanFiftySymbol_ShouldHaveError()
        {
            registerUserRequestModel.FirstName = "Taumatawhakatangihangakoauauotamateaturipukakapikimaungahoronukupokaiwhenuakitanatahu";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("FirstName can not be more than 50 characters");
        }

        [Test]
        public async Task When_LastNameNull_ShouldHaveError()
        {
            registerUserRequestModel.LastName = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("LastName can not be null");
        }

        [Test]
        public async Task When_LastNameEmpty_ShouldHaveError()
        {
            registerUserRequestModel.LastName = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("LastName can not be empty");
        }

        [Test]
        public async Task When_LastNameLessThanTwoSymbol_ShouldHaveError()
        {
            registerUserRequestModel.LastName = "A";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("LastName must be at least two characters");
        }

        [Test]
        public async Task When_LastNameMoreThanFiftySymbol_ShouldHaveError()
        {
            registerUserRequestModel.LastName = "Taumatawhakatangihangakoauauotamateaturipukakapikimaungahoronukupokaiwhenuakitanatahu";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("LastName can not be more than 50 characters");
        }

        [Test]
        public async Task When_EmailNull_ShouldHaveError()
        {
            registerUserRequestModel.Email = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email can not be null");
        }

        [Test]
        public async Task When_EmailEmpty_ShouldHaveError()
        {
            registerUserRequestModel.Email = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email can not be empty");
        }

        [Test]
        public async Task When_EmailInvalidFormat_ShouldHaveError()
        {
            registerUserRequestModel.Email = "test@abv";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email address");
        }

        [Test]
        public async Task When_PasswordNull_ShouldHaveError()
        {
            registerUserRequestModel.Password = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password can not be null");
        }

        [Test]
        public async Task When_PasswordEmpty_ShouldHaveError()
        {
            registerUserRequestModel.Password = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password can not be empty");
        }

        [Test]
        public async Task When_PasswordLengthLessThanTenSymbols_ShouldHaveError()
        {
            registerUserRequestModel.Password = "Parola";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must be at least 10 characters long");
        }

        [Test]
        public async Task When_PasswordDontHaveAtleatOneSymbol_ShouldHaveError()
        {
            registerUserRequestModel.Password = "Parola1234";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Invalid Password");
        }
        [Test]
        public async Task When_PasswordDontHaveAtleatOneUpperLetter_ShouldHaveError()
        {
            registerUserRequestModel.Password = "parola1234";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Invalid Password");
        }
        [Test]
        public async Task When_PasswordDontHaveAtleatOneLowerLetter_ShouldHaveError()
        {
            registerUserRequestModel.Password = "PAROLA1234";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Invalid Password");
        }

        [Test]
        public async Task When_PasswordDontHaveAtleatOneNumber_ShouldHaveError()
        {
            registerUserRequestModel.Password = "Parola!!!!";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Invalid Password");
        }

        [Test]
        public async Task When_PhoneNumberNull_ShouldHaveError()
        {
            registerUserRequestModel.PhoneNumber = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("PhoneNumber can not be null");
        }

        [Test]
        public async Task When_PhoneNumberEmpty_ShouldHaveError()
        {
            registerUserRequestModel.PhoneNumber = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("PhoneNumber can not be empty");
        }

        [Test]
        public async Task When_AddresCountryNull_ShouldHaveError()
        {
            registerUserRequestModel.Address.Country = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.Country)
                .WithErrorMessage("Country name can not be null");
        }

        [Test]
        public async Task When_AddresCountryEmpty_ShouldHaveError()
        {
            registerUserRequestModel.Address.Country = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.Country)
                .WithErrorMessage("Country name can not be empty");
        }

        [Test]
        public async Task When_AddresCityNull_ShouldHaveError()
        {
            registerUserRequestModel.Address.City = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.City)
                .WithErrorMessage("City name can not be null");
        }

        [Test]
        public async Task When_AddresCityEmpty_ShouldHaveError()
        {
            registerUserRequestModel.Address.City = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.City)
                .WithErrorMessage("City name can not be empty");
        }

        [Test]
        public async Task When_AddresStreetNameNull_ShouldHaveError()
        {
            registerUserRequestModel.Address.StreetName = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.StreetName)
                .WithErrorMessage("StreetName can not be null");
        }

        [Test]
        public async Task When_AddresStreetNameEmpty_ShouldHaveError()
        {
            registerUserRequestModel.Address.StreetName = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.StreetName)
                .WithErrorMessage("StreetName can not be empty");
        }

        [Test]
        public async Task When_AddresStreetNumberNull_ShouldHaveError()
        {
            registerUserRequestModel.Address.StreetNumber = null;
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.StreetNumber)
                .WithErrorMessage("StreetNumber can not be null");
        }

        [Test]
        public async Task When_AddresStreetNumberEmpty_ShouldHaveError()
        {
            registerUserRequestModel.Address.StreetNumber = "";
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Address.StreetNumber)
                .WithErrorMessage("StreetNumber can not be empty");
        }

        [Test]
        public async Task When_RegisterDataValid_ShouldValidateSuccessfully()
        {
            var result = await validator.TestValidateAsync(registerUserRequestModel);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
