using API.Common;
using API.Services.DTOs;
using API.Services.Requests;
using API.Services.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService requestService;

        public RequestController(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        [HttpGet("all")]
        
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await requestService.GetAllRequestsAsync();
            
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }
            
            return NotFound(result.ErrorMessage);
        }

        [HttpPost("bookRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] RequestModel requestForm)
        {
            var result = await requestService.CreateRequestAsync(requestForm);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("update")]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> Update([FromBody]UpdateRequest updateRequest)
        {
            var result = await requestService.UpdateRequestByIdAsync(updateRequest);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
