using API.Common;
using API.DataAccess.Models;
using API.Repositories.Contracts;
using API.Services.DTOs;
using API.Services.Mappers;
using API.Services.Requests;
using API.Services.Responses;
using API.Services.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Service
{
    public class RequestService : IRequestService
    {
        private readonly UserManager<User> userManager;
        private readonly IBookService bookService;
        private readonly IBookRepository bookRepository;
        private readonly IRequestRepository requestRepository;
        public RequestService(UserManager<User> userManager, IBookRepository bookRepository, IBookService bookService, IRequestRepository requestRepository)
        {
            this.userManager = userManager;
            this.bookService = bookService;
            this.bookRepository = bookRepository;
            this.requestRepository = requestRepository;
        }

        public async Task<Response<IList<RequestDTO>>> GetAllRequestsAsync()
        {
            var allRequest = await requestRepository.GetAllNewRequest();

            if (allRequest.Count == 0)
            {
                return (new Response<IList<RequestDTO>>(HttpStatusCode.NotFound, null, GlobalConstants.NO_NEW_REQUEST_MESSAGE));
            }

            var allRequestDto = allRequest.Select(book => Mapper.MapFrom(book)).ToList();

            return new Response<IList<RequestDTO>>(HttpStatusCode.OK, allRequestDto);
        }

        public async Task<Response<RequestDTO>> UpdateRequestByIdAsync(UpdateRequest updateRequest)
        {
            if (updateRequest.RequestApproved != true)
            {
                var rejectRequest = await requestRepository.GetRequestByUserIdAndBookId(updateRequest.UserId, updateRequest.BookId);
                
                if (rejectRequest == null)
                {
                    return (new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_NOT_FOUND_MESSAGE));
                }

                DeleteRequest(rejectRequest);
                await requestRepository.SaveAsync();
                var rejectDto = Mapper.MapFrom(rejectRequest);

                return new Response<RequestDTO>(HttpStatusCode.OK, rejectDto);
            }

            var checkDuplicateRequest = await requestRepository.CheckForDuplicateRequest(updateRequest.UserId, updateRequest.BookId);

            if (checkDuplicateRequest != null)
            {
                return (new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.DUPLICATE_REQUEST_MESSAGE));
            }

            var approvedRequest = await requestRepository.GetRequestByUserIdAndBookId(updateRequest.UserId, updateRequest.BookId);

            if (approvedRequest == null)
            {
                return (new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_NOT_FOUND_MESSAGE));
            }

            if (approvedRequest.Book.Quantity == 0)
            {
                return (new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.OUT_OF_STOCK_MESSAGE));
            }

            approvedRequest.RequestApproved = true;
            approvedRequest.Book.Quantity -= 1;
            approvedRequest.User.Books.Add(approvedRequest.Book);
            approvedRequest.DateToReturnBook = DateTime.UtcNow.AddDays(GlobalConstants.RENT_BOOK_PERIOD);

            requestRepository.Update(approvedRequest);
            DeleteRequest(approvedRequest);
            
            await requestRepository.SaveAsync();

            var requestDto = Mapper.MapFrom(approvedRequest);

            return new Response<RequestDTO>(HttpStatusCode.OK, requestDto);
        }

        private void DeleteRequest(Request request)
        {
            request.IsDeleted = true;
            request.DeletedOn = DateTime.UtcNow;
        }
        public async Task<Response<RequestDTO>> CreateRequestAsync(RequestModel requestForm)
        {
            var currentUser = await userManager.FindByEmailAsync(requestForm.UserEmail);
            if (currentUser == null)
            {
                return new Response<RequestDTO>(HttpStatusCode.NotFound, null, GlobalConstants.USER_NOT_FOUND);
            }

            var currentBook = await bookService.GetBookByIdAsync(requestForm.BookId);
            if (currentBook == null)
            {
                return new Response<RequestDTO>(HttpStatusCode.NotFound, null, GlobalConstants.BOOK_NOT_FOUND_ERROR);
            }

            var bookEntity = await bookRepository.GetBookByTitleAndAuthorNamesAsync(currentBook.Content.Title, currentBook.Content.AuthorFirstName, currentBook.Content.AuthorLastName);

            var requestEntity = Mapper.MapFromRequest(currentUser, bookEntity);

            //Checking if request already exists
            var checkRequest = await requestRepository.GetRequestByIdsAsync(requestEntity.UserId, requestEntity.BookId);
            if (checkRequest != null)
            {
                return new Response<RequestDTO>(HttpStatusCode.BadRequest, null, GlobalConstants.REQUEST_DUPLICATE_ERROR);
            }

            await requestRepository.InsertAsync(requestEntity);
            await requestRepository.SaveAsync();

            var requestDTO = new RequestDTO();

            requestDTO.UserId = requestEntity.UserId;
            requestDTO.BookId = requestEntity.BookId;

            return new Response<RequestDTO>(HttpStatusCode.OK, requestDTO);
        }
    }
}
