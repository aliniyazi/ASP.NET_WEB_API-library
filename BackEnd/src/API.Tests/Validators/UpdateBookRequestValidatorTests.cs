using API.Common;
using API.Services.Requests;
using API.Validators;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.Validators
{
    [TestFixture]
    class UpdateBookRequestValidatorTests
    {
        private UpdateBookRequestValidator validator;
        private UpdateBookRequest updateBookRequestModel;
        private IFormFile file;

        [SetUp]
        public void SetUp()
        {
            validator = new UpdateBookRequestValidator();
            file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 21830, "ImageFile", "thewithcher.jpeg");
            updateBookRequestModel = new UpdateBookRequest
            {
                Id = 1,
                Title = "Test title",
                Description = "Test description",
                Genre = "Horror",
                Quantity = 5,
                AuthorFirstName = "Ivan",
                AuthorLastName = "Ivanov",
                ImageFile = file
            };
        }

        [Test]
        public async Task When_TitleNull_ShouldHaveError()
        {
            updateBookRequestModel.Title = null;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_NULL_ERROR);
        }

        [Test]
        public async Task When_TitleEmpty_ShouldHaveError()
        {
            updateBookRequestModel.Title = "";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_EMPTY_ERROR);
        }

        [Test]
        public async Task When_TitleLessThanTwoSymbols_ShouldHaveError()
        {
            updateBookRequestModel.Title = "A";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_MIN_LENGTH_ERROR);
        }

        [Test]
        public async Task When_TitleMoreThanHundretAndTwentySymbols_ShouldHaveError()
        {
            updateBookRequestModel.Title = "On the Origin of Species by Means of Natural Selection, or the Preservation of Favoured Races in the Struggle for Life and more.";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_MAX_LENGTH_ERROR);
        }

        [Test]
        public async Task When_DescriptionNull_ShouldHaveError()
        {
            updateBookRequestModel.Description = null;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_NULL_ERROR);
        }

        [Test]
        public async Task When_DescriptionEmpty_ShouldHaveError()
        {
            updateBookRequestModel.Description = "";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_EMPTY_ERROR);
        }

        [Test]
        public async Task When_DescriptionMoreThanHundretAndTwentySymbols_ShouldHaveError()
        {
            updateBookRequestModel.Description = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga and more.";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_MAX_LENGTH_ERROR);
        }

        [Test]
        public async Task When_QuantityEmpty_ShouldHaveError()
        {
            updateBookRequestModel.Quantity = 0;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_EMPTY_ERROR);
        }

        [Test]
        public async Task When_QuantityLessOrEqualToZero_ShouldHaveError()
        {
            updateBookRequestModel.Quantity = -1;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_MIN_LENGTH_ERROR);
        }

        [Test]
        public async Task When_ImageFileNull_ShouldHaveError()
        {
            updateBookRequestModel.ImageFile = null;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.ImageFile)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_IMAGE_NULL_ERROR);
        }

        [Test]
        public async Task When_GenreNull_ShouldHaveError()
        {
            updateBookRequestModel.Genre = null;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Genre)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_GENRE_NULL_ERROR);
        }

        [Test]
        public async Task When_GenreEmpty_ShouldHaveError()
        {
            updateBookRequestModel.Genre = "";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Genre)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_GENRE_EMPTY_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameNull_ShouldHaveError()
        {
            updateBookRequestModel.AuthorLastName = null;
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_NULL_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameEmpty_ShouldHaveError()
        {
            updateBookRequestModel.AuthorLastName = "";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_EMPTY_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameLessThanTwoSymbols_ShouldHaveError()
        {
            updateBookRequestModel.AuthorLastName = "A";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MIN_LENGTH_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameMoreThanHundretAndTwentySymbols_ShouldHaveError()
        {
            updateBookRequestModel.AuthorLastName = "Taumatawhakatangihangakoauauotamateaturipukakapikimaungahoronukupokaiwhenuakitanatahu";
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MAX_LENGTH_ERROR);
        }

        [Test]
        public async Task When_UpdateBookDataIsValid_ShouldValidateSuccessfully()
        {
            var result = await validator.TestValidateAsync(updateBookRequestModel);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
