using API.DataAccess;
using API.DataAccess.Models;
using API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace API.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Book> GetBookByTitleAndAuthorNamesAsync(string title, string firstName, string lastName)
        {
            var book = await this.dbContext.Books
                .FirstOrDefaultAsync(b => b.Title == title && b.Author.FirstName == firstName && b.Author.LastName == lastName);
            return book;
        }

        public async Task<DateTime> GetLastestBookDate()
        {
            Book[] latestBook = await this.dbContext.Books.AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted)
                .Take(1)
                .ToArrayAsync();

            return latestBook[0].CreatedOn;
        }

        public async Task<IEnumerable<Book>> GetBooksForLastTwoWeeksAsync(int page, int booksPerPage, DateTime latestBook)
        {

            return await this.dbContext.Books.AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted && x.CreatedOn >= latestBook.AddDays(-14))
                .Skip((page - 1) * booksPerPage)
                .Take(booksPerPage)
                .ToListAsync();
        }

        public async Task<int> GetNumOfBooksForLastTwoWeeksAsync(DateTime latestBook)
        {
            return await this.dbContext.Books.AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => !x.IsDeleted && x.CreatedOn >= latestBook.AddDays(-14))
                .CountAsync();
        }

        public async Task<int> GetBookNumberAsync()
        {
            int bookNum = await this.dbContext.Books.Where(x => !x.IsDeleted).CountAsync();
            return bookNum;
        }

        public async Task<Book[]> GetBooksByPageAsync(int page, int booksPerPage)
        {
            var books = this.table.AsNoTracking().Include(x => x.Author).Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).Skip((page - 1) * booksPerPage).Take(booksPerPage).ToArray();
            return books;
        }

        public async Task<IList<Book>> GetAllBooksToBeReturnInTwoWeekAsync()
        {
            var books = await this.dbContext.Books
                .Include(x => x.Requests)
                .Include(x => x.Author)
                .Where(x => x.Requests.Any(r => r.RequestApproved == true && r.DateToReturnBook <= DateTime.UtcNow.AddDays(14) && r.DateToReturnBook >= DateTime.UtcNow))
                .ToListAsync();
            return books;
        }

        public async Task<IList<Book>> GetAllBooksDelayedReturnAsync()
        {
            var books = await this.dbContext.Books
               .Include(x => x.User)
               .Include(x => x.Requests)
               .Where(x => x.Requests.Any(r => r.RequestApproved == true && r.DateToReturnBook < DateTime.UtcNow))
               .ToListAsync();
            return books;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await this.dbContext.Books.AsNoTracking()
                .Include(x => x.Author)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }
    }
}
