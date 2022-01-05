using API.Common;
using API.DataAccess.Enums;
using API.DataAccess.Models;
using API.Repositories.Contracts;
using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Responses;
using API.Services.Service;
using API.Services.ServiceContracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.Services
{
    [TestFixture]
    public class BooksServiceTests
    {
        private IBookService bookService;
        private IBookRepository bookRepository;
        private IAuthorRepository authorRepository;
        private IFileService fileService;
        private IFormFile file;
        private string imageFile;
        private string getImageResult;
        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            bookRepository = Substitute.For<IBookRepository>();
            authorRepository = Substitute.For<IAuthorRepository>();
            userService = Substitute.For<IUserService>();
            fileService = A.Fake<IFileService>();
            file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 21830, "ImageFile", "thewithcher.jpeg");
            imageFile = "52284819-d7f4-4550-b66a-5dd6b3cb535d/thewithcher.jpeg";
            getImageResult = "data:image/jpeg;base64" + Convert.ToBase64String(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")).ToArray());
            userService = Substitute.For<IUserService>();
            bookService = new BookService(bookRepository, authorRepository, userService, fileService);
        }

        [Test]
        public async Task When_CreateBookAsync_WithValidData_ShouldReturnOk()
        {
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));

            var bookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.InsertAsync(default).ReturnsForAnyArgs(new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            });

            var expectedResult = new Response<BookDTO>(HttpStatusCode.OK, bookDTO);
            var actual = await bookService.CreateBookAsync(createBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content.Id, Is.EqualTo(expectedResult.Content.Id));
            Assert.That(actual.Content.Title, Is.EqualTo(expectedResult.Content.Title));
            Assert.That(actual.Content.ImageName, Is.EqualTo(expectedResult.Content.ImageName));
            Assert.That(actual.Content.Description, Is.EqualTo(expectedResult.Content.Description));
            Assert.That(actual.Content.Genre, Is.EqualTo(expectedResult.Content.Genre));
            Assert.That(actual.Content.Quantity, Is.EqualTo(expectedResult.Content.Quantity));
            Assert.That(actual.Content.AuthorFirstName, Is.EqualTo(expectedResult.Content.AuthorFirstName));
            Assert.That(actual.Content.AuthorLastName, Is.EqualTo(expectedResult.Content.AuthorLastName));
        }

        [Test]
        public async Task When_CreateBookAsync_WithAlreadyExistingBook_ShouldReturnBadRequest()
        {
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));

            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var testBook = new Book
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            };

            bookRepository.GetBookByTitleAndAuthorNamesAsync(default, default, default).ReturnsForAnyArgs(testBook);
            var expectedResult = new Response<BookDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.BOOK_ALREADY_EXISTS_ERROR);
            var actual = await bookService.CreateBookAsync(createBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_CreateBookAsync_WithoutImage_ShouldReturnBadRequest()
        {
            file = null;

            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var testBook = new Book
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            };

            bookRepository.GetBookByTitleAndAuthorNamesAsync(default, default, default).ReturnsForAnyArgs(testBook);
            var expectedResult = new Response<BookDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.CREATE_OR_EDIT_BOOK_NO_IMAGE);
            var actual = await bookService.CreateBookAsync(createBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_CreateBookAsync_FailedImageUpload_ShouldReturnFailedDependency()
        {
            imageFile = null;
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));

            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var expectedResult = new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.UPLOAD_IMAGE_ERROR);
            var actual = await bookService.CreateBookAsync(createBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_CreateBookAsync_FailedImageGet_ShouldReturnFailedDependency()
        {
            getImageResult = null;
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));

            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var bookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.InsertAsync(default).ReturnsForAnyArgs(new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            });

            var expectedResult = new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR);
            var actual = await bookService.CreateBookAsync(createBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_DeleteBookByIdAsync_WithValidData_ShouldReturnOk()
        {
            var id = 1;
            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(new Book());
            bookRepository.DeleteAsync(default).ReturnsForAnyArgs(new Book { Id = id });
            var expectedResult = new Response<int>(HttpStatusCode.OK, id);
            var actual = await bookService.DeleteBookByIdAsync(id);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.Content, Is.EqualTo(expectedResult.Content));
            Assert.That(actual.ErrorMessage, Is.Null);
        }

        [Test]
        public async Task When_DeleteBookByIdAsync_WithNotExistingBook_ShouldReturnNotFound()
        {
            var id = 1;
            var expectedResult = new Response<int>(HttpStatusCode.NotFound, id, GlobalConstants.BOOK_NOT_FOUND_ERROR);
            var actual = await bookService.DeleteBookByIdAsync(id);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_GetBookByIdAsync_WithValidData_ShouldReturnOk()
        {
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));

            var id = 1;
            var bookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            });

            authorRepository.GetByIdAsync(default).ReturnsForAnyArgs(new Author
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            });

            var expectedResult = new Response<BookDTO>(HttpStatusCode.OK, bookDTO);
            var actual = await this.bookService.GetBookByIdAsync(id);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content.Id, Is.EqualTo(expectedResult.Content.Id));
            Assert.That(actual.Content.Title, Is.EqualTo(expectedResult.Content.Title));
            Assert.That(actual.Content.Description, Is.EqualTo(expectedResult.Content.Description));
            Assert.That(actual.Content.Genre, Is.EqualTo(expectedResult.Content.Genre));
            Assert.That(actual.Content.ImageName, Is.EqualTo(expectedResult.Content.ImageName));
            Assert.That(actual.Content.Quantity, Is.EqualTo(expectedResult.Content.Quantity));
            Assert.That(actual.Content.AuthorFirstName, Is.EqualTo(expectedResult.Content.AuthorFirstName));
            Assert.That(actual.Content.AuthorLastName, Is.EqualTo(expectedResult.Content.AuthorLastName));
        }

        [Test]
        public async Task When_GetBookByIdAsync_WithNotExistingBook_ShouldReturnNotFound()
        {
            var id = 1;
            var expectedResult = new Response<BookDTO>(HttpStatusCode.NotFound, null, GlobalConstants.BOOK_NOT_FOUND_ERROR);
            var actual = await this.bookService.GetBookByIdAsync(id);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }
        [Test]
        public async Task When_GetBookByIdAsync_FailedImageGet_ShouldReturnFailedDependency()
        {
            getImageResult = null;
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));

            var id = 1;
            var bookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            });

            authorRepository.GetByIdAsync(default).ReturnsForAnyArgs(new Author
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            });

            var expectedResult = new Response<BookDTO>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR);
            var actual = await this.bookService.GetBookByIdAsync(id);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateBookAsync_WithAlreadyExistingBook_ShouldReturnOk()
        {
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));

            var updateBookRequest = new UpdateBookRequest
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var testBook = new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            };

            var testAuthor = new Author
            {
                Id = 1,
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };

            var testBookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(testBook);
            bookRepository.Update(default).ReturnsForAnyArgs(testBook);
            authorRepository.GetAuthorByAuthorNamesAsync(default, default).ReturnsForAnyArgs(testAuthor);

            var expectedResult = new Response<BookDTO>(HttpStatusCode.OK, testBookDTO);
            var actual = await this.bookService.UpdateBookAsync(updateBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content.Title, Is.EqualTo(expectedResult.Content.Title));
            Assert.That(actual.Content.Description, Is.EqualTo(expectedResult.Content.Description));
            Assert.That(actual.Content.Genre, Is.EqualTo(expectedResult.Content.Genre));
            Assert.That(actual.Content.ImageName, Is.EqualTo(expectedResult.Content.ImageName));
            Assert.That(actual.Content.Quantity, Is.EqualTo(expectedResult.Content.Quantity));
            Assert.That(actual.Content.AuthorFirstName, Is.EqualTo(expectedResult.Content.AuthorFirstName));
            Assert.That(actual.Content.AuthorLastName, Is.EqualTo(expectedResult.Content.AuthorLastName));
        }

        [Test]
        public async Task When_UpdateBookAsync_WithNotExistingBook_ShouldReturnBadRequest()
        {
            var updateBookRequest = new UpdateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var expectedResult = new Response<CreateBookRequest>(HttpStatusCode.NotFound, default, GlobalConstants.BOOK_NOT_FOUND_ERROR);
            var actual = await this.bookService.UpdateBookAsync(updateBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateBookAsync_WithoutImage_ShouldReturnBadRequest()
        {
            file = null;
            var updateBookRequest = new UpdateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var expectedResult = new Response<CreateBookRequest>(HttpStatusCode.BadRequest, default, GlobalConstants.CREATE_OR_EDIT_BOOK_NO_IMAGE);
            var actual = await this.bookService.UpdateBookAsync(updateBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateBookAsync_FailedImageUpload_ShouldReturnFailedDependency()
        {
            imageFile = null;
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));

            var updateBookRequest = new UpdateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };
            var testBook = new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            };

            var testBookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(testBook);

            var expectedResult = new Response<CreateBookRequest>(HttpStatusCode.FailedDependency, default, GlobalConstants.UPLOAD_IMAGE_ERROR);
            var actual = await this.bookService.UpdateBookAsync(updateBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateBookAsync_FailedImageGet_ShouldReturnFailedDependency()
        {
            getImageResult = null;
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));
            var updateBookRequest = new UpdateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };
            var testBook = new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            };

            var testBookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(testBook);

            var expectedResult = new Response<CreateBookRequest>(HttpStatusCode.FailedDependency, default, GlobalConstants.GET_IMAGE_ERROR);
            var actual = await this.bookService.UpdateBookAsync(updateBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateBookAsync_FailedImageDelete_ShouldReturnFailedDependency()
        {
            A.CallTo(() => fileService.UploadImageToAzureStorage(A<IFormFile>.Ignored)).Returns(Task.FromResult(imageFile));
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));
            A.CallTo(() => fileService.DeleteImageFromAzure(A<string>.Ignored)).Throws<Exception>();

            var updateBookRequest = new UpdateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageFile = file,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };
            var testBook = new Book
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = Genres.Action,
                ImageName = imageFile,
                Quantity = 1,
                Author = new Author { FirstName = "TestFirstName", LastName = "TestLastName" },
            };

            var testBookDTO = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookRepository.GetByIdAsync(default).ReturnsForAnyArgs(testBook);

            var expectedResult = new Response<CreateBookRequest>(HttpStatusCode.FailedDependency, default, GlobalConstants.DELETE_IMAGE_ERROR);
            var actual = await this.bookService.UpdateBookAsync(updateBookRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]

        public async Task When_GettingTwoWeeks_WithNoBooksOnPage_Expect_EmptyPage()
        {
            IEnumerable<Book> books = new List<Book>();
            BookDTO[] expectedBooks = new BookDTO[0];

            DateTime now = DateTime.UtcNow;
            bookRepository.GetLastestBookDate().Returns(Task.FromResult(now));
            bookRepository.GetBooksForLastTwoWeeksAsync(2, 5, now).Returns(Task.FromResult(books));
            bookRepository.GetNumOfBooksForLastTwoWeeksAsync(now).Returns(Task.FromResult(5));
            PageDTO dto = new PageDTO(expectedBooks, 1);

            var expectedResult = new Response<PageDTO>(HttpStatusCode.OK, dto);
            var result = await this.bookService.GetBooksForLastTwoWeeksAsync(2, 5);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<PageDTO>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.Content.data, Is.Empty);
            Assert.That(result.Content.recordsTotal, Is.EqualTo(5));
        }
        [Test]
        public async Task When_GettingTwoWeeks_WithBookOnPage_Expect_AtLeastOneBook()
        {

            Book book1 = new Book
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book2 = new Book
            {
                Title = "Sherlock Holmes",
                Description = "A crime book",
                Quantity = 2,
                Genre = (Genres)4,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book3 = new Book
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book4 = new Book
            {
                Title = "Fate",
                Description = "A fate book",
                Quantity = 13,
                Genre = (Genres)9,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book5 = new Book
            {
                Title = "longer than all the others for no reason",
                Description = "A book that is way longer than all the others just in case it breaks something",
                Quantity = 3,
                Genre = (Genres)3,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };

            BookDTO bookDTO1 = new BookDTO
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = book1.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };
            BookDTO bookDTO2 = new BookDTO
            {
                Title = "Sherlock Holmes",
                Description = "A crime book",
                Quantity = 2,
                Genre = book2.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };
            BookDTO bookDTO3 = new BookDTO
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = book3.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };
            BookDTO bookDTO4 = new BookDTO
            {
                Title = "Fate",
                Description = "A fate book",
                Quantity = 13,
                Genre = book4.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };
            BookDTO bookDTO5 = new BookDTO
            {
                Title = "longer than all the others for no reason",
                Description = "A book that is way longer than all the others just in case it breaks something",
                Quantity = 3,
                Genre = book5.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };

            IEnumerable<Book> actuallBooks = new List<Book>() { book1, book2, book3, book4, book5, };

            BookDTO[] expectedBooks = { bookDTO1, bookDTO2, bookDTO3, bookDTO4, bookDTO5 };
            PageDTO dto = new PageDTO(expectedBooks, 6);
            var expectedResult = new Response<PageDTO>(HttpStatusCode.OK, dto);

            DateTime now = DateTime.UtcNow;
            bookRepository.GetLastestBookDate().Returns(Task.FromResult(now));
            bookRepository.GetBooksForLastTwoWeeksAsync(1, 5, now).Returns(Task.FromResult(actuallBooks));
            bookRepository.GetNumOfBooksForLastTwoWeeksAsync(now).Returns(Task.FromResult(6));
            authorRepository.GetByIdAsync(4).ReturnsForAnyArgs(Task.FromResult(new Author { FirstName = "Henry", LastName = "Smith" }));

            var result = await this.bookService.GetBooksForLastTwoWeeksAsync(1, 5);


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<PageDTO>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content.recordsTotal, Is.EqualTo(6));
            Assert.That(result.Content.data.Length, Is.EqualTo(expectedBooks.Length));
            Assert.That(result.Content.data.Length, Is.EqualTo(actuallBooks.Count()));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(result.Content.data[i].Title, Is.EqualTo(expectedResult.Content.data[i].Title));
                Assert.That(result.Content.data[i].Description, Is.EqualTo(expectedResult.Content.data[i].Description));
                Assert.That(result.Content.data[i].Quantity, Is.EqualTo(expectedResult.Content.data[i].Quantity));
                Assert.That(result.Content.data[i].Genre, Is.EqualTo(expectedResult.Content.data[i].Genre));
                Assert.That(result.Content.data[i].AuthorFirstName, Is.EqualTo(expectedResult.Content.data[i].AuthorFirstName));
                Assert.That(result.Content.data[i].AuthorLastName, Is.EqualTo(expectedResult.Content.data[i].AuthorLastName));
            }

        }

        [Test]
        public async Task When_GettingStatics_Expect_CorrectBooksAndUsers()
        {
            bookRepository.GetBookNumberAsync().Returns(24);
            userService.GetNumOfUsersAsync().Returns(13);

            var statistics = await this.bookService.GetStatisticsAsync();
            StatisticDTO dto = new StatisticDTO(24, 17, 13);
            Response<StatisticDTO> expectedResult = new Response<StatisticDTO>(HttpStatusCode.OK, dto);

            Assert.That(statistics, Is.Not.Null);
            Assert.That(statistics, Is.TypeOf<Response<StatisticDTO>>());
            Assert.That(statistics.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(statistics.Content, Is.Not.Null);
            Assert.That(statistics.Content.countAllBooks, Is.EqualTo(expectedResult.Content.countAllBooks));
            Assert.That(statistics.Content.countAllGenres, Is.EqualTo(expectedResult.Content.countAllGenres));
            Assert.That(statistics.Content.countAllUsers, Is.EqualTo(expectedResult.Content.countAllUsers));
        }

        [Test]
        public async Task When_GettingAll_WithNoBookOnPage_Expect_Null()
        {
            var books = await this.bookService.GetAllBooksInPageAsync(9, 9);
            var listOfBooks = new List<BookDTO>();
            Response<BookDTO[]> bookDTO = new Response<BookDTO[]>(HttpStatusCode.OK, listOfBooks.ToArray());
            Assert.AreEqual(books.StatusCode, bookDTO.StatusCode);
            Assert.AreEqual(books.Content, bookDTO.Content);
        }

        [Test]
        public async Task When_GettingAll_WithBookOnPage_Expect_AtLeastOneBook()
        {
            var books = await this.bookService.GetAllBooksInPageAsync(1, 5);
            Assert.That(books, Is.Not.Null);
        }

        [Test]
        public async Task When_AllBooksToBeReturnInTwoWeekAsync_NoBookFoundShouldRetunBadRequest()
        {
            IList<Book> actuallBooks = new List<Book>();
            bookRepository.GetAllBooksToBeReturnInTwoWeekAsync().Returns(Task.FromResult(actuallBooks));

            var expectedResult = new Response<IList<BookDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_BOOKS_TO_RETURN_TWO_WEEKS_MESSAGE);
            var actual = await bookService.GetAllBooksToBeReturnInTwoWeekAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_AllBooksToBeReturnInTwoWeekAsync_FailedImageGet_ShouldReturnFailedDependency()
        {
            Book book1 = new Book
            {
                Title = "The Witcher 1",
                Description = "Test 1",
                Quantity = 3,
                Genre = (Genres)7,
                ImageName = imageFile,
                Author = new Author
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov"
                }
            };

            Book book2 = new Book
            {
                Title = "The Witcher 2",
                Description = "Test 2",
                Quantity = 3,
                ImageName = imageFile,
                Genre = (Genres)7,
                Author = new Author
                {
                    FirstName = "Georgi",
                    LastName = "Petrov"
                }
            };

            getImageResult = null;
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));
            IList<Book> actuallBooks = new List<Book>() { book1, book2 };
            bookRepository.GetAllBooksToBeReturnInTwoWeekAsync().ReturnsForAnyArgs(actuallBooks);

            var expectedResult = new Response<IList<BookDTO>>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR);
            var actual = await bookService.GetAllBooksToBeReturnInTwoWeekAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_AllBooksToBeReturnInTwoWeekAsync_ValidData_ShouldReturnOk()
        {
            Book book1 = new Book
            {
                Id = 1,
                Title = "The Witcher 1",
                Description = "Test 1",
                Quantity = 3,
                Genre = Genres.Action,
                ImageName = imageFile,
                Author = new Author
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov"
                }
            };
            
            Book book2 = new Book
            {
                Id = 2,
                Title = "The Witcher 2",
                Description = "Test 2",
                Quantity = 3,
                ImageName = imageFile,
                Genre = Genres.Action,
                Author = new Author
                {
                    FirstName = "Georgi",
                    LastName = "Petrov"
                }
            };

            BookDTO bookDTO1 = new BookDTO
            {
                Id = 1,
                Title = "The Witcher 1",
                Description = "Test 1",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 3,
                AuthorFirstName = "Ivan",
                AuthorLastName = "Ivanov"
            };

            BookDTO bookDTO2 = new BookDTO
            {
                Id = 2,
                Title = "The Witcher 2",
                Description = "Test 2",
                Genre = "Action",
                ImageName = getImageResult,
                Quantity = 3,
                AuthorFirstName = "Georgi",
                AuthorLastName = "Petrov"
            };

            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));
            
            IList<Book> actuallBooks = new List<Book>() { book1, book2 };
            IList<BookDTO> expectedBooks = new List<BookDTO>() { bookDTO1, bookDTO2 };
            bookRepository.GetAllBooksToBeReturnInTwoWeekAsync().ReturnsForAnyArgs(actuallBooks);

            var expectedResult = new Response<IList<BookDTO>>(HttpStatusCode.OK, expectedBooks);
            var actual = await bookService.GetAllBooksToBeReturnInTwoWeekAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content, Is.Not.Null);

            for (int i = 0; i < 2; i++)
            {
                Assert.That(actual.Content[i].Id, Is.EqualTo(expectedResult.Content[i].Id));
                Assert.That(actual.Content[i].Title, Is.EqualTo(expectedResult.Content[i].Title));
                Assert.That(actual.Content[i].Description, Is.EqualTo(expectedResult.Content[i].Description));
                Assert.That(actual.Content[i].Genre, Is.EqualTo(expectedResult.Content[i].Genre));
                Assert.That(actual.Content[i].ImageName, Is.EqualTo(expectedResult.Content[i].ImageName));
                Assert.That(actual.Content[i].Quantity, Is.EqualTo(expectedResult.Content[i].Quantity));
                Assert.That(actual.Content[i].AuthorFirstName, Is.EqualTo(expectedResult.Content[i].AuthorFirstName));
                Assert.That(actual.Content[i].AuthorLastName, Is.EqualTo(expectedResult.Content[i].AuthorLastName));
            }
        }

        [Test]
        public async Task When_GetAllBooksDelayedReturnAsync_NoBookFoundShouldRetunBadRequest()
        {
            IList<Book> actuallBooks = new List<Book>();
            bookRepository.GetAllBooksDelayedReturnAsync().Returns(Task.FromResult(actuallBooks));

            var expectedResult = new Response<IList<BookDelayedDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_BOOKS_DELAYED_RETURN);
            var actual = await bookService.GetAllBooksDelayedReturnAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_GetAllBooksDelayedReturnAsync_FailedImageGet_ShouldReturnFailedDependency()
        {
            Book book1 = new Book
            {
                Title = "The Witcher 1",
                Description = "Test 1",
                Quantity = 3,
                Genre = (Genres)7,
                ImageName = imageFile,
                Author = new Author
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov"
                }
            };

            Book book2 = new Book
            {
                Title = "The Witcher 2",
                Description = "Test 2",
                Quantity = 3,
                ImageName = imageFile,
                Genre = (Genres)7,
                Author = new Author
                {
                    FirstName = "Georgi",
                    LastName = "Petrov"
                }
            };

            getImageResult = null;
            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));
            IList<Book> actuallBooks = new List<Book>() { book1, book2 };
            bookRepository.GetAllBooksDelayedReturnAsync().ReturnsForAnyArgs(actuallBooks);

            var expectedResult = new Response<IList<BookDelayedDTO>>(HttpStatusCode.FailedDependency, null, GlobalConstants.GET_IMAGE_ERROR);
            var actual = await bookService.GetAllBooksDelayedReturnAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_GetAllBooksDelayedReturnAsync_ValidData_ShouldReturnOk()
        {
            Book book1 = new Book
            {
                Id = 1,
                Title = "The Witcher 1",
                Description = "Test 1",
                Quantity = 3,
                Genre = Genres.Action,
                ImageName = imageFile,
                User = new User
                {
                    FirstName = "Ivan",
                    Lastname = "Ivanov",
                    Email = "test@abv.bg",
                    PhoneNumber = "0893100100"
                }
            };

            Book book2 = new Book
            {
                Id = 2,
                Title = "The Witcher 2",
                Description = "Test 2",
                Quantity = 3,
                ImageName = imageFile,
                Genre = Genres.Action,
                User = new User
                {
                    FirstName = "Georgi",
                    Lastname = "Petrov",
                    Email = "test2@abv.bg",
                    PhoneNumber = "0893200200"
                }
            };

            BookDelayedDTO bookDTO1 = new BookDelayedDTO
            {
                Id = 1,
                Title = "The Witcher 1",
                Genre = "Action",
                ImageName = getImageResult,
                UserFirstName = "Ivan",
                UserLastName = "Ivanov",
                Email = "test@abv.bg",
                PhoneNumber = "0893100100"
            };

            BookDelayedDTO bookDTO2 = new BookDelayedDTO
            {
                Id = 2,
                Title = "The Witcher 2",
                Genre = "Action",
                ImageName = getImageResult,
                UserFirstName = "Georgi",
                UserLastName = "Petrov",
                Email = "test2@abv.bg",
                PhoneNumber = "0893200200"
            };

            A.CallTo(() => fileService.GetImageFromAzureStorage(A<string>.Ignored)).Returns(Task.FromResult(getImageResult));

            IList<Book> actuallBooks = new List<Book>() { book1, book2 };
            IList<BookDelayedDTO> expectedBooks = new List<BookDelayedDTO>() { bookDTO1, bookDTO2 };
            bookRepository.GetAllBooksDelayedReturnAsync().ReturnsForAnyArgs(actuallBooks);

            var expectedResult = new Response<IList<BookDelayedDTO>>(HttpStatusCode.OK, expectedBooks);
            var actual = await bookService.GetAllBooksDelayedReturnAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content, Is.Not.Null);

            for (int i = 0; i < 2; i++)
            {
                Assert.That(actual.Content[i].Id, Is.EqualTo(expectedResult.Content[i].Id));
                Assert.That(actual.Content[i].Title, Is.EqualTo(expectedResult.Content[i].Title));
                Assert.That(actual.Content[i].ImageName, Is.EqualTo(expectedResult.Content[i].ImageName));
                Assert.That(actual.Content[i].Genre, Is.EqualTo(expectedResult.Content[i].Genre));
                Assert.That(actual.Content[i].UserFirstName, Is.EqualTo(expectedResult.Content[i].UserFirstName));
                Assert.That(actual.Content[i].UserLastName, Is.EqualTo(expectedResult.Content[i].UserLastName));
                Assert.That(actual.Content[i].Email, Is.EqualTo(expectedResult.Content[i].Email));
                Assert.That(actual.Content[i].PhoneNumber, Is.EqualTo(expectedResult.Content[i].PhoneNumber));
            }
        }

        [Test]
        public async Task When_SearchingByTitle_Expect_CorrectBooks()
        {
            authorRepository.GetByIdAsync(4).ReturnsForAnyArgs(Task.FromResult(new Author { FirstName = "Henry", LastName = "Smith" }));
            Book book1 = new Book
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book2 = new Book
            {
                Title = "Sherlock Holmes",
                Description = "A crime book",
                Quantity = 2,
                Genre = (Genres)4,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book3 = new Book
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book4 = new Book
            {
                Title = "Fate",
                Description = "A fate book",
                Quantity = 13,
                Genre = (Genres)9,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book5 = new Book
            {
                Title = "longer than all the others for no reason",
                Description = "A book that is way longer than all the others just in case it breaks something",
                Quantity = 3,
                Genre = (Genres)3,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };

            BookDTO bookDTO1 = new BookDTO
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = book1.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };
            BookDTO bookDTO2 = new BookDTO
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = book3.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };

            IEnumerable<Book> actuallBooks = new List<Book>() { book1, book2, book3, book4, book5, };

            BookDTO[] expectedBooks = { bookDTO1, bookDTO2 };
            PageDTO dto = new PageDTO(expectedBooks, 6);
            var expectedResult = new Response<PageDTO>(HttpStatusCode.OK, dto);

            bookRepository.GetBooksAsync().Returns(Task.FromResult(actuallBooks));


            var result = await this.bookService.Search(1, 5, "Title", "haRry Jackson", null);


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<PageDTO>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content.recordsTotal, Is.EqualTo(2));
            Assert.That(result.Content.data.Length, Is.EqualTo(expectedBooks.Length));
            for (int i = 0; i < result.Content.data.Length; i++)
            {
                Assert.That(result.Content.data[i].Title, Is.EqualTo(expectedResult.Content.data[i].Title));
                Assert.That(result.Content.data[i].Description, Is.EqualTo(expectedResult.Content.data[i].Description));
                Assert.That(result.Content.data[i].Quantity, Is.EqualTo(expectedResult.Content.data[i].Quantity));
                Assert.That(result.Content.data[i].Genre, Is.EqualTo(expectedResult.Content.data[i].Genre));
                Assert.That(result.Content.data[i].AuthorFirstName, Is.EqualTo(expectedResult.Content.data[i].AuthorFirstName));
                Assert.That(result.Content.data[i].AuthorLastName, Is.EqualTo(expectedResult.Content.data[i].AuthorLastName));
            }
        }

        [Test]
        public async Task When_SearchingByDescriptionWithoutWholeWords_Expect_ToStillReturnCorrectBooks()
        {
            authorRepository.GetByIdAsync(4).ReturnsForAnyArgs(Task.FromResult(new Author { FirstName = "Henry", LastName = "Smith" }));
            Book book1 = new Book
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book2 = new Book
            {
                Title = "Sherlock Holmes",
                Description = "A crime book",
                Quantity = 2,
                Genre = (Genres)4,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book3 = new Book
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book4 = new Book
            {
                Title = "Fate",
                Description = "A fate book",
                Quantity = 13,
                Genre = (Genres)9,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book5 = new Book
            {
                Title = "longer than all the others for no reason",
                Description = "A book that is way longer than all the others just in case it breaks something",
                Quantity = 3,
                Genre = (Genres)3,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };

            BookDTO bookDTO1 = new BookDTO
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = book1.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };
            BookDTO bookDTO2 = new BookDTO
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = book3.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith"
            };

            IEnumerable<Book> actuallBooks = new List<Book>() { book1, book2, book3, book4, book5, };

            BookDTO[] expectedBooks = { bookDTO1, bookDTO2 };
            PageDTO dto = new PageDTO(expectedBooks, 6);
            var expectedResult = new Response<PageDTO>(HttpStatusCode.OK, dto);

            bookRepository.GetBooksAsync().Returns(Task.FromResult(actuallBooks));


            var result = await this.bookService.Search(1, 5, "Description", "gree ma", null);


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<PageDTO>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content.recordsTotal, Is.EqualTo(2));
            Assert.That(result.Content.data.Length, Is.EqualTo(expectedBooks.Length));
            for (int i = 0; i < result.Content.data.Length; i++)
            {
                Assert.That(result.Content.data[i].Title, Is.EqualTo(expectedResult.Content.data[i].Title));
                Assert.That(result.Content.data[i].Description, Is.EqualTo(expectedResult.Content.data[i].Description));
                Assert.That(result.Content.data[i].Quantity, Is.EqualTo(expectedResult.Content.data[i].Quantity));
                Assert.That(result.Content.data[i].Genre, Is.EqualTo(expectedResult.Content.data[i].Genre));
                Assert.That(result.Content.data[i].AuthorFirstName, Is.EqualTo(expectedResult.Content.data[i].AuthorFirstName));
                Assert.That(result.Content.data[i].AuthorLastName, Is.EqualTo(expectedResult.Content.data[i].AuthorLastName));
            }
        }

        [Test]
        public async Task When_SearchingWithWrongSearchType_Expect_ToSearchByDescriptionByDefault()
        {
            authorRepository.GetByIdAsync(4).ReturnsForAnyArgs(Task.FromResult(new Author { FirstName = "Henry", LastName = "Smith" }));
            Book book1 = new Book
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book2 = new Book
            {
                Title = "Sherlock Holmes",
                Description = "A crime book",
                Quantity = 2,
                Genre = (Genres)4,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book3 = new Book
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = (Genres)7,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book4 = new Book
            {
                Title = "Fate",
                Description = "A fate book",
                Quantity = 13,
                Genre = (Genres)9,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };
            Book book5 = new Book
            {
                Title = "longer than all the others for no reason",
                Description = "A book that is way longer than all the others just in case it breaks something",
                Quantity = 3,
                Genre = (Genres)3,
                AuthorId = 5,
                Author = authorRepository.GetByIdAsync(5).Result,
                ImageName = "test"
            };

            BookDTO bookDTO1 = new BookDTO
            {
                Title = "Harry Potter",
                Description = "A magic book",
                Quantity = 3,
                Genre = book1.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith",
                ImageName = "test"
            };
            BookDTO bookDTO2 = new BookDTO
            {
                Title = "Percy Jackson",
                Description = "A greek book",
                Quantity = 5,
                Genre = book3.Genre.ToString(),
                AuthorFirstName = "Henry",
                AuthorLastName = "Smith",
                ImageName = "test"
            };

            IEnumerable<Book> actuallBooks = new List<Book>() { book1, book2, book3, book4, book5, };

            BookDTO[] expectedBooks = { bookDTO1, bookDTO2 };
            PageDTO dto = new PageDTO(expectedBooks, 6);
            var expectedResult = new Response<PageDTO>(HttpStatusCode.OK, dto);

            bookRepository.GetBooksAsync().Returns(Task.FromResult(actuallBooks));


            var result = await this.bookService.Search(1, 5, "ator", "gree magic reason henry", null);


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Response<PageDTO>>());
            Assert.That(result.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content.recordsTotal, Is.EqualTo(2));
            Assert.That(result.Content.data.Length, Is.EqualTo(expectedBooks.Length));

            for (int i = 0; i < result.Content.data.Length; i++)
            {
                Assert.That(result.Content.data[i].Title, Is.EqualTo(expectedResult.Content.data[i].Title));
                Assert.That(result.Content.data[i].Description, Is.EqualTo(expectedResult.Content.data[i].Description));
                Assert.That(result.Content.data[i].Quantity, Is.EqualTo(expectedResult.Content.data[i].Quantity));
                Assert.That(result.Content.data[i].Genre, Is.EqualTo(expectedResult.Content.data[i].Genre));
                Assert.That(result.Content.data[i].AuthorFirstName, Is.EqualTo(expectedResult.Content.data[i].AuthorFirstName));
                Assert.That(result.Content.data[i].AuthorLastName, Is.EqualTo(expectedResult.Content.data[i].AuthorLastName));
            }
        }

    }
}
