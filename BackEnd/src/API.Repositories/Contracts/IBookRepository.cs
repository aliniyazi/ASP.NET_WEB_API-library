using API.DataAccess.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace API.Repositories.Contracts
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<Book> GetBookByTitleAndAuthorNamesAsync(string title, string firstName, string lastName);
        Task<IEnumerable<Book>> GetBooksForLastTwoWeeksAsync(int page, int booksPerPage, DateTime latestBook);
        Task<int> GetBookNumberAsync();
        Task<DateTime> GetLastestBookDate();
        Task<Book[]> GetBooksByPageAsync(int page, int booksPerPage);
        Task<int> GetNumOfBooksForLastTwoWeeksAsync(DateTime latestBook);
        Task<IList<Book>> GetAllBooksToBeReturnInTwoWeekAsync();
        Task<IList<Book>> GetAllBooksDelayedReturnAsync();
        Task<IEnumerable<Book>> GetBooksAsync();
    }
}
