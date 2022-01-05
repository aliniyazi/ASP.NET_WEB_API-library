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
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Author> GetAuthorByAuthorNamesAsync(string firstName, string lastName)
        {
            return await this.dbContext.Authors
                .FirstOrDefaultAsync(b => b.FirstName == firstName && b.LastName == lastName);
        }
    }
}
