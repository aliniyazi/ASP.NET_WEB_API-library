using API.Common;
using API.Services.Requests;
using FluentValidation;

namespace API.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("FirstName can not be null")
                .NotEmpty()
                .WithMessage("FirstName can not be empty")
                .MinimumLength(GlobalConstants.MIN_LENGTH_FIRST_OR_LASTNAME)
                .WithMessage("FirstName must be at least two characters")
                .MaximumLength(GlobalConstants.MAX_LENGTH_FIRST_OR_LASTNAME)
                .WithMessage("FirstName can not be more than 50 characters");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("LastName can not be null")
                .NotEmpty()
                .WithMessage("LastName can not be empty")
                .MinimumLength(GlobalConstants.MIN_LENGTH_FIRST_OR_LASTNAME)
                .WithMessage("LastName must be at least two characters")
                .MaximumLength(GlobalConstants.MAX_LENGTH_FIRST_OR_LASTNAME)
                .WithMessage("LastName can not be more than 50 characters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Email can not be null")
                .NotEmpty()
                .WithMessage("Email can not be empty")
                .Matches(GlobalConstants.EMAIL_VALIDATION_REGEX)
                .WithMessage("Invalid email address");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("PhoneNumber can not be null")
                .NotEmpty()
                .WithMessage("PhoneNumber can not be empty");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Password can not be null")
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .MinimumLength(GlobalConstants.MIN_PASSWORD_LENGTH)
                .WithMessage("Password must be at least 10 characters long")
                .Matches(GlobalConstants.PASSWORD_ALLOWED_CHARACTERS)
                .WithMessage("Invalid Password");

            RuleFor(x => x.Address.Country)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Country name can not be null")
                .NotEmpty()
                .WithMessage("Country name can not be empty");

            RuleFor(x => x.Address.City)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("City name can not be null")
                .NotEmpty()
                .WithMessage("City name can not be empty");

            RuleFor(x => x.Address.StreetName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("StreetName can not be null")
                .NotEmpty()
                .WithMessage("StreetName can not be empty");

            RuleFor(x => x.Address.StreetNumber)
                 .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("StreetNumber can not be null")
                .NotEmpty()
                .WithMessage("StreetNumber can not be empty");
        }
    }
}
