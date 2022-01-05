using API.Common;
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
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> Create([FromForm] CreateBookRequest userInput) 
        {
            var result = await bookService.CreateBookAsync(userInput);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return BadRequest(result.ErrorMessage);
        }
    
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await bookService.GetBookByIdAsync(id);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> Update([FromForm] UpdateBookRequest updateBookRequest)
        {
            var result = await bookService.UpdateBookAsync(updateBookRequest);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await bookService.DeleteBookByIdAsync(id);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetTwoWeeks([FromQuery] int page, int booksPerPage)
        {
            var result = await bookService.GetBooksForLastTwoWeeksAsync(page, booksPerPage);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await bookService.GetStatisticsAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int page, int booksPerPage)
        {
            var result = await bookService.GetAllBooksInPageAsync(page, booksPerPage);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("returned/pending")]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> GetAllBooksToReturnInTwoWeeks()
        {
            var result = await bookService.GetAllBooksToBeReturnInTwoWeekAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("returned/delayed")]
        [Authorize(Roles = Roles.Librarian)]
        public async Task<IActionResult> GetAllBooksDelayedReturn()
        {
            var result = await bookService.GetAllBooksDelayedReturnAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(int page, int booksPerPage, string searchType, string search, string genres)
        {
            var result = await bookService.Search(page, booksPerPage, searchType, search, genres);
            if (result.StatusCode == 200)
            {
                return Ok(result.Content);
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
