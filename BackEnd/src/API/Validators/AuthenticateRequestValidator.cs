using API.Common;
using API.Services.Requests;
using FluentValidation;

namespace API.Validators
{
    public class AuthenticateRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public AuthenticateRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Email can not be null")
                .NotEmpty()
                .WithMessage("Email can not be Empty")
                .Matches(GlobalConstants.EMAIL_VALIDATION_REGEX)
                .WithMessage("Invalid email Address");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Password can not be null")
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .MinimumLength(GlobalConstants.MIN_PASSWORD_LENGTH)
                .WithMessage("Password must be at least 10 characters long");
        }
    }
}
