using API.Common;
using API.Controllers;
using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Responses;
using API.Services.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API.Tests.Controllers
{
    [TestFixture]
    public class RequestControllerTests
    {
        private IRequestService requestService;
        private RequestController requestController;

        [SetUp]
        public void SetUp()
        {
            requestService = Substitute.For<IRequestService>();
            requestController = new RequestController(requestService);
        }

        [Test]
        public async Task When_GetAllRequests_ShouldReturnOk()
        {
            var bookDTO1 = new RequestDTO
            {
                UserId = "1",
                BookId = 1,
                FirstName = "Test1",
                LastName = "TestL1",
                PhoneNumber = "0893100100",
                Email = "test@abv.bg",
                BookTitle = "TestTitle",
                Quantity = 5,
            };

            var bookDTO2 = new RequestDTO
            {
                UserId = "2",
                BookId = 2,
                FirstName = "Test2",
                LastName = "TestL2",
                PhoneNumber = "0893200200",
                Email = "test2@abv.bg",
                BookTitle = "TestTitle2",
                Quantity = 5,
            };

            IList<RequestDTO> expectedBooks = new List<RequestDTO>() { bookDTO1, bookDTO2 };

            requestService.GetAllRequestsAsync().ReturnsForAnyArgs(new Response<IList<RequestDTO>>(HttpStatusCode.OK, expectedBooks));
            var result = await requestController.GetAllRequests();
            var okResult = result as OkObjectResult;
            var content = okResult.Value as IList<RequestDTO>;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            for (int i = 0; i < 2; i++)
            {
                Assert.That(expectedBooks[i].UserId, Is.EqualTo(content[i].UserId));
                Assert.That(expectedBooks[i].BookId, Is.EqualTo(content[i].BookId));
                Assert.That(expectedBooks[i].FirstName, Is.EqualTo(content[i].FirstName));
                Assert.That(expectedBooks[i].LastName, Is.EqualTo(content[i].LastName));
                Assert.That(expectedBooks[i].PhoneNumber, Is.EqualTo(content[i].PhoneNumber));
                Assert.That(expectedBooks[i].Email, Is.EqualTo(content[i].Email));
                Assert.That(expectedBooks[i].BookTitle, Is.EqualTo(content[i].BookTitle));
                Assert.That(expectedBooks[i].Quantity, Is.EqualTo(content[i].Quantity));
            }
        }


        [Test]
        public async Task When_GetAllRequests_ShouldReturnNotFound()
        {
            requestService.GetAllRequestsAsync().ReturnsForAnyArgs(new Response<IList<RequestDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_NEW_REQUEST_MESSAGE));
            var result = await requestController.GetAllRequests();
            var notFountResult = result as NotFoundObjectResult;

            Assert.That(404, Is.EqualTo(notFountResult.StatusCode));
            Assert.That(GlobalConstants.NO_NEW_REQUEST_MESSAGE, Is.EqualTo(notFountResult.Value));
        }

        [Test]
        public async Task When_Update_ShouldReturnOk()
        {
            var updateRequest = new UpdateRequest
            {
                UserId = "1",
                BookId = 1,
                RequestApproved = true
            };

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

            requestService.UpdateRequestByIdAsync(updateRequest).ReturnsForAnyArgs(new Response<RequestDTO>(HttpStatusCode.OK, requestDto));
            var result = await requestController.Update(updateRequest);
            var okResult = result as OkObjectResult;
            var content = okResult.Value as RequestDTO;

            Assert.That(200, Is.EqualTo(okResult.StatusCode));
            Assert.That(requestDto.UserId, Is.EqualTo(content.UserId));
            Assert.That(requestDto.BookId, Is.EqualTo(content.BookId));
            Assert.That(requestDto.FirstName, Is.EqualTo(content.FirstName));
            Assert.That(requestDto.LastName, Is.EqualTo(content.LastName));
            Assert.That(requestDto.BookTitle, Is.EqualTo(content.BookTitle));
            Assert.That(requestDto.Email, Is.EqualTo(content.Email));
            Assert.That(requestDto.PhoneNumber, Is.EqualTo(content.PhoneNumber));
            Assert.That(requestDto.Quantity, Is.EqualTo(content.Quantity));
        }

        [Test]
        public async Task When_Update_ShouldReturnBadRequest()
        {
            requestService.UpdateRequestByIdAsync(new UpdateRequest()).ReturnsForAnyArgs(new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_NOT_FOUND_MESSAGE));

            var result = await requestController.Update(new UpdateRequest());
            var badRequestResult = result as BadRequestObjectResult;

            Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
            Assert.That(GlobalConstants.REQUEST_NOT_FOUND_MESSAGE, Is.EqualTo(badRequestResult.Value));
        }
        [Test]
        public async Task When_CreateRequest_Expect_ReturnOk()
        {
            this.requestService.CreateRequestAsync(Arg.Any<RequestModel>()).Returns(Task.FromResult(new Response<RequestDTO>(HttpStatusCode.OK, new RequestDTO())));
            var result = await requestController.CreateRequest(new RequestModel());

            Assert.That(result, Is.Not.Null);

            var response = result as OkObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task When_CreateRequest_Expect_ReturnBadRequest()
        {
            this.requestService.CreateRequestAsync(Arg.Any<RequestModel>()).Returns(Task.FromResult(new Response<RequestDTO>(HttpStatusCode.BadRequest, new RequestDTO())));
            var result = await requestController.CreateRequest(new RequestModel());

            Assert.That(result, Is.Not.Null);

            var response = result as BadRequestObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
