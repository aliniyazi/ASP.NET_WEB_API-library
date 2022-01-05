using FluentValidation;
using API.Services.Requests;
using API.Common;

namespace API.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_NULL_ERROR)
                .NotEmpty()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_EMPTY_ERROR)
                .MinimumLength(GlobalConstants.MIN_LENGTH_TITLE)
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_MIN_LENGTH_ERROR)
                .MaximumLength(GlobalConstants.MAX_LENGTH_TITLE)
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_MAX_LENGTH_ERROR);

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_NULL_ERROR)
                .NotEmpty()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_EMPTY_ERROR)
                .MaximumLength(GlobalConstants.MAX_LENGTH_DESCRIPTION)
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_MAX_LENGTH_ERROR);

            RuleFor(x => x.Quantity)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_NULL_ERROR)
                .NotEmpty()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_EMPTY_ERROR)
                .GreaterThan(GlobalConstants.MIN_VALUE_QUANTITY)
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_MIN_LENGTH_ERROR);

            RuleFor(x => x.ImageFile)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_IMAGE_NULL_ERROR)
                .NotEmpty()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_IMAGE_EMPTY_ERROR);

            RuleFor(x => x.Genre)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_GENRE_NULL_ERROR)
                .NotEmpty()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_GENRE_EMPTY_ERROR);

            RuleFor(x => x.AuthorLastName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_NULL_ERROR)
                .NotEmpty()
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_EMPTY_ERROR)
                .MinimumLength(GlobalConstants.MIN_LENGTH_FIRST_OR_LASTNAME)
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MIN_LENGTH_ERROR)
                .MaximumLength(GlobalConstants.MAX_LENGTH_FIRST_OR_LASTNAME)
                .WithMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MAX_LENGTH_ERROR);
        }
    }
}
