using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.ServiceContracts
{
    public interface IBookService
    {
        Task<Response<BookDTO>> CreateBookAsync(CreateBookRequest userInput);
        Task<Response<BookDTO>> UpdateBookAsync(UpdateBookRequest userInput);
        Task<Response<int>> DeleteBookByIdAsync(int id);
        Task<Response<BookDTO>> GetBookByIdAsync(int id);
        Task<Response<PageDTO>> GetBooksForLastTwoWeeksAsync(int page, int booksPerPage);
        Task<Response<StatisticDTO>> GetStatisticsAsync();
        Task<Response<BookDTO[]>> GetAllBooksInPageAsync(int page, int booksPerPage);
        Task<Response<IList<BookDTO>>> GetAllBooksToBeReturnInTwoWeekAsync();
        Task<Response<IList<BookDelayedDTO>>> GetAllBooksDelayedReturnAsync();

        Task<Response<PageDTO>> Search(int page, int booksPerPage, string searchType, string search, string genres);
    }
}
