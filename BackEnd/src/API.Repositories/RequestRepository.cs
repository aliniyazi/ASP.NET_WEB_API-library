using API.DataAccess;
using API.DataAccess.Models;
using API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Request>> GetAllNewRequest()
        {
            return await this.dbContext.Requests
                .Include(x => x.User)
                .Include(x => x.Book)
                .Where(x => x.IsDeleted == false && x.RequestApproved == false)
                .ToListAsync();
        }

        public async Task<Request> GetRequestByUserIdAndBookId(string userId, int bookId)
        {
            return await this.dbContext.Requests
                .Include(x => x.Book)
                .Include(x => x.User)
                .Include(x => x.Book.Author)
                .Where(x => x.BookId == bookId && x.UserId == userId && !x.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Request> CheckForDuplicateRequest(string userId, int bookId)
        {
            return await this.dbContext.Requests
                .Where(x => x.BookId == bookId && x.UserId == userId && x.RequestApproved == true && x.DateToReturnBook > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }
        public async Task<Request> GetRequestByIdsAsync(string userId, int bookId)
        {
            Request request = await dbContext.Requests.Where(x => x.UserId == userId).Where(y => y.BookId == bookId).FirstOrDefaultAsync();
            return request;
        }
    }
}
