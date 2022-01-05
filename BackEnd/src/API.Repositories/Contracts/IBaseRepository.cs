using API.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories.Contracts
{
    public interface IBaseRepository<T> : IAsyncDisposable where T : class, IDeletable
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<T> InsertAsync(T obj);
        T Update(T obj);
        Task<T> DeleteAsync(object id);
        Task SaveAsync();
    }
}
