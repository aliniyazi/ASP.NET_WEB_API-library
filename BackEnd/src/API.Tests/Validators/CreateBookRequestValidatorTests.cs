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
    class CreateBookRequestValidatorTests
    {
        private CreateBookRequestValidator validator;
        private CreateBookRequest createBookRequestModel;
        private IFormFile file;

        [SetUp]
        public void SetUp()
        {
            validator = new CreateBookRequestValidator();
            file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 21830, "ImageFile", "thewithcher.jpeg");
            createBookRequestModel = new CreateBookRequest
            {
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
            createBookRequestModel.Title = null;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_NULL_ERROR);
        }

        [Test]
        public async Task When_TitleEmpty_ShouldHaveError()
        {
            createBookRequestModel.Title = "";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_EMPTY_ERROR);
        }

        [Test]
        public async Task When_TitleLessThanTwoSymbols_ShouldHaveError()
        {
            createBookRequestModel.Title = "A";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_MIN_LENGTH_ERROR);
        }

        [Test]
        public async Task When_TitleMoreThanHundretAndTwentySymbols_ShouldHaveError()
        {
            createBookRequestModel.Title = "On the Origin of Species by Means of Natural Selection, or the Preservation of Favoured Races in the Struggle for Life and more.";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_TITLE_MAX_LENGTH_ERROR);
        }

        [Test]
        public async Task When_DescriptionNull_ShouldHaveError()
        {
            createBookRequestModel.Description = null;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_NULL_ERROR);
        }

        [Test]
        public async Task When_DescriptionEmpty_ShouldHaveError()
        {
            createBookRequestModel.Description = "";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_EMPTY_ERROR);
        }

        [Test]
        public async Task When_DescriptionMoreThanHundretAndTwentySymbols_ShouldHaveError()
        {
            createBookRequestModel.Description = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga and more.";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_DESCRIPTION_MAX_LENGTH_ERROR);
        }

        [Test]
        public async Task When_QuantityEmpty_ShouldHaveError()
        {
            createBookRequestModel.Quantity = 0;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_EMPTY_ERROR);
        }

        [Test]
        public async Task When_QuantityLessOrEqualToZero_ShouldHaveError()
        {
            createBookRequestModel.Quantity = -1;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_QUANTITY_MIN_LENGTH_ERROR);
        }

        [Test]
        public async Task When_ImageFileNull_ShouldHaveError()
        {
            createBookRequestModel.ImageFile = null;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.ImageFile)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_IMAGE_NULL_ERROR);
        }

        [Test]
        public async Task When_GenreNull_ShouldHaveError()
        {
            createBookRequestModel.Genre = null;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Genre)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_GENRE_NULL_ERROR);
        }

        [Test]
        public async Task When_GenreEmpty_ShouldHaveError()
        {
            createBookRequestModel.Genre = "";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.Genre)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_GENRE_EMPTY_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameNull_ShouldHaveError()
        {
            createBookRequestModel.AuthorLastName = null;
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_NULL_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameEmpty_ShouldHaveError()
        {
            createBookRequestModel.AuthorLastName = "";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_EMPTY_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameLessThanTwoSymbols_ShouldHaveError()
        {
            createBookRequestModel.AuthorLastName = "A";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MIN_LENGTH_ERROR);
        }

        [Test]
        public async Task When_AuthorLastNameMoreThanHundretAndTwentySymbols_ShouldHaveError()
        {
            createBookRequestModel.AuthorLastName = "Taumatawhakatangihangakoauauotamateaturipukakapikimaungahoronukupokaiwhenuakitanatahu";
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldHaveValidationErrorFor(x => x.AuthorLastName)
                .WithErrorMessage(GlobalConstants.CREATE_OR_UPDATE_BOOK_AUTHOR_LAST_NAME_MAX_LENGTH_ERROR);
        }

        [Test]
        public async Task When_CreateBookDataIsValid_ShouldValidateSuccessfully()
        {
            var result = await validator.TestValidateAsync(createBookRequestModel);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
