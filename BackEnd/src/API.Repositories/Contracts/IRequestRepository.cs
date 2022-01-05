using API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repositories.Contracts
{
    public interface IRequestRepository : IBaseRepository<Request>
    {
        Task<IList<Request>> GetAllNewRequest();
        Task<Request> GetRequestByUserIdAndBookId(string userId, int bookId);
        Task<Request> CheckForDuplicateRequest(string userId, int bookId);
        Task<Request> GetRequestByIdsAsync(string userId, int bookId);
    }
}
