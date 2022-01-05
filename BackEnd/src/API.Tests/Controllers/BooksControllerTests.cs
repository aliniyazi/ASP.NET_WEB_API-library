using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using API.Controllers;
using API.Services.ServiceContracts;
using API.Services.Requests;
using API.Services.Responses;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using API.Services.DTOs;
using System.Collections.Generic;
using API.Common;

namespace API.Tests.Controllers
{
    [TestFixture]
    public class BooksControllerTests
    {
        private IBookService bookService;
        private BooksController controller;
        private string imageName;
        [SetUp]
        public void SetUp()
        {
            bookService = Substitute.For<IBookService>();
            controller = new BooksController(bookService);
            imageName = "52284819-d7f4-4550-b66a-5dd6b3cb535d/thewithcher.jpeg";
        }

        [Test]
        public async Task When_Create_WithNotExistingBook_ShouldReturnOk()
        {
            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
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
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookService.CreateBookAsync(createBookRequest).ReturnsForAnyArgs(new Response<BookDTO>(HttpStatusCode.OK, bookDTO));
            var result = await controller.Create(createBookRequest);
            var okResult = result as OkObjectResult;
            var content = okResult.Value as BookDTO;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            Assert.That(bookDTO.Id, Is.EqualTo(content.Id));
            Assert.That(bookDTO.Title, Is.EqualTo(content.Title));
            Assert.That(bookDTO.Description, Is.EqualTo(content.Description));
            Assert.That(bookDTO.Genre, Is.EqualTo(content.Genre));
            Assert.That(bookDTO.Quantity, Is.EqualTo(content.Quantity));
            Assert.That(bookDTO.AuthorFirstName, Is.EqualTo(content.AuthorFirstName));
            Assert.That(bookDTO.AuthorLastName, Is.EqualTo(content.AuthorLastName));
        }

        [Test]
        public async Task When_Create_WithExistingBook_ShouldReturnBadRequest()
        {
            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
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
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookService.CreateBookAsync(createBookRequest).ReturnsForAnyArgs(new Response<BookDTO>(HttpStatusCode.BadRequest, bookDTO));
            var result = await controller.Create(createBookRequest);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
        }

        [Test]
        public async Task When_Get_WithExistingBook_ShouldReturnOk()
        {
            var id = 1;
            var createBookRequest = new CreateBookRequest
            {
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
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
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookService.GetBookByIdAsync(id).ReturnsForAnyArgs(new Response<BookDTO>(HttpStatusCode.OK, bookDTO));
            var result = await controller.Get(id);
            var okResult = result as OkObjectResult;
            var content = okResult.Value as BookDTO;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            Assert.That(bookDTO.Id, Is.EqualTo(content.Id));
            Assert.That(bookDTO.Title, Is.EqualTo(content.Title));
            Assert.That(bookDTO.Description, Is.EqualTo(content.Description));
            Assert.That(bookDTO.Genre, Is.EqualTo(content.Genre));
            Assert.That(bookDTO.Quantity, Is.EqualTo(content.Quantity));
            Assert.That(bookDTO.AuthorFirstName, Is.EqualTo(content.AuthorFirstName));
            Assert.That(bookDTO.AuthorLastName, Is.EqualTo(content.AuthorLastName));
        }

        [Test]
        public async Task When_Get_WithNotExistingBook_ShouldReturnNotFound()
        {
            var id = 1;
            bookService.GetBookByIdAsync(id).ReturnsForAnyArgs(new Response<BookDTO>(HttpStatusCode.NotFound, new BookDTO()));
            var result = await controller.Get(id);
            var notFountResult = result as NotFoundObjectResult;

            Assert.That(404, Is.EqualTo(notFountResult.StatusCode));
        }

        [Test]
        public async Task When_Update_WithExistingBook_ShouldReturnOk()
        {
            var updateBookRequest = new UpdateBookRequest
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
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
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            bookService.UpdateBookAsync(updateBookRequest).ReturnsForAnyArgs(new Response<BookDTO>(HttpStatusCode.OK, bookDTO));
            var result = await controller.Update(updateBookRequest);
            var okResult = result as OkObjectResult;
            var content = okResult.Value as BookDTO;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            Assert.That(bookDTO.Title, Is.EqualTo(content.Title));
            Assert.That(bookDTO.Description, Is.EqualTo(content.Description));
            Assert.That(bookDTO.Genre, Is.EqualTo(content.Genre));
            Assert.That(bookDTO.Quantity, Is.EqualTo(content.Quantity));
            Assert.That(bookDTO.AuthorFirstName, Is.EqualTo(content.AuthorFirstName));
            Assert.That(bookDTO.AuthorLastName, Is.EqualTo(content.AuthorLastName));
        }

        [Test]
        public async Task When_Update_WithNotExistingBook_ShouldReturnNotFound()
        {
            bookService.UpdateBookAsync(new UpdateBookRequest()).ReturnsForAnyArgs(new Response<BookDTO>(HttpStatusCode.NotFound, new BookDTO()));

            var result = await controller.Update(new UpdateBookRequest());
            var notFountResult = result as NotFoundObjectResult;

            Assert.That(404, Is.EqualTo(notFountResult.StatusCode));
        }

        [Test]
        public async Task When_Delete_WithExistingBook_ShouldReturnOk()
        {
            var id = 1;
            bookService.DeleteBookByIdAsync(id).ReturnsForAnyArgs(new Response<int>(HttpStatusCode.OK, id));
            var result = await controller.Delete(id);
            var okResult = result as OkObjectResult;
            var content = okResult.Value as int?;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            Assert.That(id, Is.EqualTo(content));
        }

        [Test]
        public async Task When_Delete_WithNotExistingBook_ShouldReturnNotFound()
        {
            bookService.DeleteBookByIdAsync(default).ReturnsForAnyArgs(new Response<int>(HttpStatusCode.NotFound, default));
            var result = await controller.Delete(default);
            var notFountResult = result as NotFoundObjectResult;

            Assert.That(404, Is.EqualTo(notFountResult.StatusCode));
        }

        [Test]
        public async Task When_GetAllBooksToReturnInTwoWeeks_ShouldReturnOk()
        {
            var bookDTO1 = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var bookDTO2 = new BookDTO
            {
                Id = 2,
                Title = "TestTitle2",
                Description = "TestDescription2",
                Genre = "Action",
                Quantity = 2,
                AuthorFirstName = "TestFirstName2",
                AuthorLastName = "TestLastName2"
            };

            IList<BookDTO> expectedBooks = new List<BookDTO>() { bookDTO1, bookDTO2 };

            bookService.GetAllBooksToBeReturnInTwoWeekAsync().ReturnsForAnyArgs(new Response<IList<BookDTO>>(HttpStatusCode.OK, expectedBooks));
            var result = await controller.GetAllBooksToReturnInTwoWeeks();
            var okResult = result as OkObjectResult;
            var content = okResult.Value as IList<BookDTO>;
           
            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            for (int i = 0; i < 2; i++)
            {
                Assert.That(expectedBooks[i].Id, Is.EqualTo(content[i].Id));
                Assert.That(expectedBooks[i].Title, Is.EqualTo(content[i].Title));
                Assert.That(expectedBooks[i].Description, Is.EqualTo(content[i].Description));
                Assert.That(expectedBooks[i].Genre, Is.EqualTo(content[i].Genre));
                Assert.That(expectedBooks[i].Quantity, Is.EqualTo(content[i].Quantity));
                Assert.That(expectedBooks[i].AuthorFirstName, Is.EqualTo(content[i].AuthorFirstName));
                Assert.That(expectedBooks[i].AuthorLastName, Is.EqualTo(content[i].AuthorLastName));
            }
        }

        [Test]
        public async Task When_GetAllBooksToReturnInTwoWeeks_ShouldReturnNotFound()
        {
            var bookDTO1 = new BookDTO
            {
                Id = 1,
                Title = "TestTitle",
                Description = "TestDescription",
                Genre = "Action",
                Quantity = 1,
                AuthorFirstName = "TestFirstName",
                AuthorLastName = "TestLastName"
            };

            var bookDTO2 = new BookDTO
            {
                Id = 2,
                Title = "TestTitle2",
                Description = "TestDescription2",
                Genre = "Action",
                Quantity = 2,
                AuthorFirstName = "TestFirstName2",
                AuthorLastName = "TestLastName2"
            };

            IList<BookDTO> expectedBooks = new List<BookDTO>() { bookDTO1, bookDTO2 };
            bookService.GetAllBooksToBeReturnInTwoWeekAsync().ReturnsForAnyArgs(new Response<IList<BookDTO>>(HttpStatusCode.NotFound, null , GlobalConstants.NO_BOOKS_TO_RETURN_TWO_WEEKS_MESSAGE));

            var result = await controller.GetAllBooksToReturnInTwoWeeks();
            var badRequestResult = result as NotFoundObjectResult;

            Assert.That(404, Is.EqualTo(badRequestResult.StatusCode));
            Assert.That(GlobalConstants.NO_BOOKS_TO_RETURN_TWO_WEEKS_MESSAGE, Is.EqualTo(badRequestResult.Value));
        }

        [Test]
        public async Task When_GetAllBooksDelayedReturn_ShouldReturnOk()
        {
            var bookDTO1 = new BookDelayedDTO
            {
                Id = 1,
                Title = "TestTitle",
                Genre = "Action",
                ImageName = imageName,
                UserFirstName = "User1",
                UserLastName = "UserL1",
                Email = "test@abv.bg",
                PhoneNumber = "0893100100"
            };

            var bookDTO2 = new BookDelayedDTO
            {
                Id = 2,
                Title = "TestTitle2",
                Genre = "Action",
                ImageName = imageName,
                UserFirstName = "User2",
                UserLastName = "UserL2",
                Email = "test2@abv.bg",
                PhoneNumber = "0893200200"
            };

            IList<BookDelayedDTO> expectedBooks = new List<BookDelayedDTO>() { bookDTO1, bookDTO2 };

            bookService.GetAllBooksDelayedReturnAsync().ReturnsForAnyArgs(new Response<IList<BookDelayedDTO>>(HttpStatusCode.OK, expectedBooks));
            var result = await controller.GetAllBooksDelayedReturn();
            var okResult = result as OkObjectResult;
            var content = okResult.Value as IList<BookDelayedDTO>;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            for (int i = 0; i < 2; i++)
            {
                Assert.That(expectedBooks[i].Id, Is.EqualTo(content[i].Id));
                Assert.That(expectedBooks[i].Title, Is.EqualTo(content[i].Title));
                Assert.That(expectedBooks[i].Genre, Is.EqualTo(content[i].Genre));
                Assert.That(expectedBooks[i].ImageName, Is.EqualTo(content[i].ImageName));
                Assert.That(expectedBooks[i].UserFirstName, Is.EqualTo(content[i].UserFirstName));
                Assert.That(expectedBooks[i].UserLastName, Is.EqualTo(content[i].UserLastName));
                Assert.That(expectedBooks[i].Email, Is.EqualTo(content[i].Email));
                Assert.That(expectedBooks[i].PhoneNumber, Is.EqualTo(content[i].PhoneNumber));
            }
        }

        [Test]
        public async Task When_GetAllBooksDelayedReturn_ShouldReturnNotFound()
        {
            var bookDTO1 = new BookDelayedDTO
            {
                Id = 1,
                Title = "TestTitle",
                Genre = "Action",
                ImageName = imageName,
                UserFirstName = "User1",
                UserLastName = "UserL1",
                Email = "test@abv.bg",
                PhoneNumber = "0893100100"
            };

            var bookDTO2 = new BookDelayedDTO
            {
                Id = 2,
                Title = "TestTitle2",
                Genre = "Action",
                ImageName = imageName,
                UserFirstName = "User2",
                UserLastName = "UserL2",
                Email = "test2@abv.bg",
                PhoneNumber = "0893200200"
            };

            IList<BookDelayedDTO> expectedBooks = new List<BookDelayedDTO>() { bookDTO1, bookDTO2 };
            bookService.GetAllBooksDelayedReturnAsync().ReturnsForAnyArgs(new Response<IList<BookDelayedDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_BOOKS_DELAYED_RETURN));

            var result = await controller.GetAllBooksDelayedReturn();
            var badRequestResult = result as NotFoundObjectResult;

            Assert.That(404, Is.EqualTo(badRequestResult.StatusCode));
            Assert.That(GlobalConstants.NO_BOOKS_DELAYED_RETURN, Is.EqualTo(badRequestResult.Value));
        }
    }
}
