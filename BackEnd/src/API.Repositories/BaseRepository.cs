using API.DataAccess;
using API.DataAccess.Contracts;
using API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IDeletable
    {
        protected DBContext dbContext;
        protected DbSet<T> table;

        protected BaseRepository(DBContext dbContext)
        {
            this.dbContext = dbContext;
            table = dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> InsertAsync(T obj)
        {
            var element =  await table.AddAsync(obj);
            return element.Entity;
        }

        public T Update(T obj)
        {
            table.Attach(obj);
            dbContext.Entry(obj).State = EntityState.Modified;
            return obj;
        }

        public async Task<T> DeleteAsync(object id)
        {
            T existing = await table.FindAsync(id);
            existing.IsDeleted = true;
            existing.DeletedOn = DateTime.UtcNow;
            Update(existing);
            return existing;           
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }
    }
}
