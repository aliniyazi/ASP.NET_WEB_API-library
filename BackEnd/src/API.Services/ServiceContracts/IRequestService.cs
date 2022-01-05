using API.DataAccess.Models;
using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.ServiceContracts
{
    public interface IRequestService
    {
        Task<Response<IList<RequestDTO>>> GetAllRequestsAsync();
        Task<Response<RequestDTO>> UpdateRequestByIdAsync(UpdateRequest updateRequest);
        Task<Response<RequestDTO>> CreateRequestAsync(RequestModel requestForm);
    }
}
