using API.Common;
using API.DataAccess.Models;
using API.Repositories.Contracts;
using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Responses;
using API.Services.Service;
using API.Services.ServiceContracts;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API.Tests.Services
{
    [TestFixture]
    public class RequestServiceTest
    {
        private IRequestService requestService;
        private IRequestRepository requestRepository;
        private UserManager<User> userManager;
        private IBookRepository bookRepository;
        private IBookService bookService;

        private UpdateRequest updateRequest;
        private Request request;

        [SetUp]
        public void SetUp()
        {
            requestRepository = Substitute.For<IRequestRepository>();
            userManager = A.Fake<UserManager<User>>();
            bookRepository = Substitute.For<IBookRepository>();
            bookService = Substitute.For<IBookService>();

            updateRequest = new UpdateRequest
            {
                UserId = "1",
                BookId = 1,
                RequestApproved = false,
            };
            request = new Request
            {
                UserId = "1",
                User = new User
                {
                    FirstName = "Ivan",
                    Lastname = "Ivanov",
                    Email = "test@abv.bg",
                    PhoneNumber = "0893100100"
                },
                BookId = 1,
                Book = new Book
                {
                    Title = "The Witcher 1",
                    Quantity = 5
                }
            };

            requestService = new RequestService(userManager, bookRepository, bookService, requestRepository);
        }

        [Test]
        public async Task When_GetAllRequestsAsync_WithValidData_ShouldReturnOk()
        {
            var request1 = new Request
            {
                UserId = "1",
                User = new User
                {
                    FirstName = "Ivan",
                    Lastname = "Ivanov",
                    Email = "test@abv.bg",
                    PhoneNumber = "0893100100"
                },
                BookId = 1,
                Book = new Book
                {
                    Title = "The Witcher 1",
                    Quantity = 5
                },
            };

            var request2 = new Request
            {
                UserId = "2",
                User = new User
                {
                    FirstName = "Georgi",
                    Lastname = "Ivanov",
                    Email = "test2@abv.bg",
                    PhoneNumber = "0893200200"
                },
                BookId = 2,
                Book = new Book
                {
                    Title = "The Witcher 2",
                    Quantity = 5
                },
            };

            var request3 = new Request
            {
                UserId = "3",
                User = new User
                {
                    FirstName = "Ivan",
                    Lastname = "Petrov",
                    Email = "asd@abv.bg",
                    PhoneNumber = "0893300300"
                },
                BookId = 3,
                Book = new Book
                {
                    Title = "The Witcher 3",
                    Quantity = 5
                },
            };

            var requestDto1 = new RequestDTO
            {
                UserId = "1",
                BookId = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                BookTitle = "The Witcher 1",
                Email = "test@abv.bg",
                PhoneNumber = "0893100100",
                Quantity = 5
            };

            var requestDto2 = new RequestDTO
            {
                UserId = "2",
                BookId = 2,
                FirstName = "Georgi",
                LastName = "Ivanov",
                BookTitle = "The Witcher 2",
                Email = "test2@abv.bg",
                PhoneNumber = "0893200200",
                Quantity = 5
            };

            var requestDto3 = new RequestDTO
            {
                UserId = "3",
                BookId = 3,
                FirstName = "Ivan",
                LastName = "Petrov",
                BookTitle = "The Witcher 3",
                Email = "asd@abv.bg",
                PhoneNumber = "0893300300",
                Quantity = 5
            };

            IList<Request> actuallRequests = new List<Request>() { request1, request2, request3 };
            IList<RequestDTO> expectedRequestDto = new List<RequestDTO>() { requestDto1, requestDto2, requestDto3 };

            requestRepository.GetAllNewRequest().Returns(Task.FromResult(actuallRequests));

            var expectedResult = new Response<IList<RequestDTO>>(HttpStatusCode.OK, expectedRequestDto);
            var actual = await this.requestService.GetAllRequestsAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content, Is.Not.Null);

            for (int i = 0; i < 3; i++)
            {
                Assert.That(actual.Content[i].UserId, Is.EqualTo(expectedResult.Content[i].UserId));
                Assert.That(actual.Content[i].BookId, Is.EqualTo(expectedResult.Content[i].BookId));
                Assert.That(actual.Content[i].FirstName, Is.EqualTo(expectedResult.Content[i].FirstName));
                Assert.That(actual.Content[i].LastName, Is.EqualTo(expectedResult.Content[i].LastName));
                Assert.That(actual.Content[i].BookTitle, Is.EqualTo(expectedResult.Content[i].BookTitle));
                Assert.That(actual.Content[i].Email, Is.EqualTo(expectedResult.Content[i].Email));
                Assert.That(actual.Content[i].PhoneNumber, Is.EqualTo(expectedResult.Content[i].PhoneNumber));
                Assert.That(actual.Content[i].Quantity, Is.EqualTo(expectedResult.Content[i].Quantity));
            }
        }

        [Test]
        public async Task When_GetAllRequestsAsync_WithNoData_ShouldReturnBadRequest()
        {
            IList<Request> actuallRequests = new List<Request>();
            requestRepository.GetAllNewRequest().Returns(Task.FromResult(actuallRequests));

            var expectedResult = new Response<IList<RequestDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_NEW_REQUEST_MESSAGE);
            var actual = await requestService.GetAllRequestsAsync();

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateRequestByIdAsync_WhenRequestRejected_AndNoRequestFound_ShouldReturnBadRequest()
        {
            var expectedResult = new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_NOT_FOUND_MESSAGE);
            var actual = await this.requestService.UpdateRequestByIdAsync(updateRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateRequestByIdAsync_WhenRequestRejected_AndRequestFound_ShouldReturnOk()
        {
            var requestDto = new RequestDTO
            {
                UserId = "1",
                BookId = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                BookTitle = "The Witcher 1",
                Email = "test@abv.bg",
                PhoneNumber = "0893100100",
                Quantity = 5
            };

            requestRepository.GetRequestByUserIdAndBookId(default, default).ReturnsForAnyArgs(request);

            var expectedResult = new Response<RequestDTO>(HttpStatusCode.OK, requestDto);
            var actual = await this.requestService.UpdateRequestByIdAsync(updateRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content, Is.Not.Null);
            Assert.That(actual.Content.UserId, Is.EqualTo(expectedResult.Content.UserId));
            Assert.That(actual.Content.BookId, Is.EqualTo(expectedResult.Content.BookId));
            Assert.That(actual.Content.FirstName, Is.EqualTo(expectedResult.Content.FirstName));
            Assert.That(actual.Content.LastName, Is.EqualTo(expectedResult.Content.LastName));
            Assert.That(actual.Content.BookTitle, Is.EqualTo(expectedResult.Content.BookTitle));
            Assert.That(actual.Content.Email, Is.EqualTo(expectedResult.Content.Email));
            Assert.That(actual.Content.PhoneNumber, Is.EqualTo(expectedResult.Content.PhoneNumber));
            Assert.That(actual.Content.Quantity, Is.EqualTo(expectedResult.Content.Quantity));
        }

        [Test]
        public async Task When_UpdateRequestByIdAsync_WhenRequestApproved_AndDuplicateRequestFound_ShouldReturnBadRequest()
        {
            updateRequest.RequestApproved = true;
            requestRepository.CheckForDuplicateRequest(default, default).ReturnsForAnyArgs(request);

            var expectedResult = new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.DUPLICATE_REQUEST_MESSAGE);
            var actual = await this.requestService.UpdateRequestByIdAsync(updateRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateRequestByIdAsync_WhenRequestApproved_AndNoRequestFound_ShouldReturnBadRequest()
        {
            var expectedResult = new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_NOT_FOUND_MESSAGE);
            var actual = await this.requestService.UpdateRequestByIdAsync(updateRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateRequestByIdAsync_WhenRequestApproved_BookQuantityEqualToZero_ShouldReturnBadRequest()
        {
            updateRequest.RequestApproved = true;
            request.Book.Quantity = 0;

            requestRepository.GetRequestByUserIdAndBookId(default, default).ReturnsForAnyArgs(request);
            var expectedResult = new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.OUT_OF_STOCK_MESSAGE);
            var actual = await this.requestService.UpdateRequestByIdAsync(updateRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Not.Null);
            Assert.That(actual.Content, Is.Null);
            Assert.That(actual.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
        }

        [Test]
        public async Task When_UpdateRequestByIdAsync_WhenRequestApprovedAndAllDataIsValid_ShouldReturnOk()
        {
            updateRequest.RequestApproved = true;
            var requestDto = new RequestDTO
            {
                UserId = "1",
                BookId = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                BookTitle = "The Witcher 1",
                Email = "test@abv.bg",
                PhoneNumber = "0893100100",
                Quantity = 4
            };

            requestRepository.GetRequestByUserIdAndBookId(default, default).ReturnsForAnyArgs(request);
            var expectedResult = new Response<RequestDTO>(HttpStatusCode.OK, requestDto);

            var actual = await this.requestService.UpdateRequestByIdAsync(updateRequest);

            Assert.That(actual.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actual.ErrorMessage, Is.Null);
            Assert.That(actual.Content, Is.Not.Null);
            Assert.That(actual.Content.UserId, Is.EqualTo(expectedResult.Content.UserId));
            Assert.That(actual.Content.BookId, Is.EqualTo(expectedResult.Content.BookId));
            Assert.That(actual.Content.FirstName, Is.EqualTo(expectedResult.Content.FirstName));
            Assert.That(actual.Content.LastName, Is.EqualTo(expectedResult.Content.LastName));
            Assert.That(actual.Content.BookTitle, Is.EqualTo(expectedResult.Content.BookTitle));
            Assert.That(actual.Content.Email, Is.EqualTo(expectedResult.Content.Email));
            Assert.That(actual.Content.PhoneNumber, Is.EqualTo(expectedResult.Content.PhoneNumber));
            Assert.That(actual.Content.Quantity, Is.EqualTo(expectedResult.Content.Quantity));
        }

        [Test]
        public async Task When_CreateRequestAsync_Expect_ReturnOkResult()
        {
            bookService.GetBookByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(new Response<BookDTO>(HttpStatusCode.OK, new BookDTO())));
            bookRepository.GetBookByTitleAndAuthorNamesAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(new Book()));
            requestRepository.GetRequestByIdsAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult((Request)null));

            var expectedResult = new Response<RequestDTO>(HttpStatusCode.OK, new RequestDTO());
            var actualResult = await requestService.CreateRequestAsync(new RequestModel());

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actualResult.Content, Is.TypeOf<RequestDTO>());
            Assert.That(actualResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task When_CreateRequestAsync_Expect_ReturnRequestDuplicateError()
        {
            bookService.GetBookByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(new Response<BookDTO>(HttpStatusCode.OK, new BookDTO())));
            bookRepository.GetBookByTitleAndAuthorNamesAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(new Book()));
            requestRepository.GetRequestByIdsAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new Request()));

            var expectedResult = new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_DUPLICATE_ERROR);
            var actualResult = await requestService.CreateRequestAsync(new RequestModel());

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.StatusCode, Is.EqualTo(expectedResult.StatusCode));
            Assert.That(actualResult.ErrorMessage, Is.EqualTo(expectedResult.ErrorMessage));
            Assert.That(actualResult.StatusCode, Is.EqualTo(400));

        }
    }
}
