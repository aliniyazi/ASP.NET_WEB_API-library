using API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repositories.Contracts
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<Author> GetAuthorByAuthorNamesAsync(string firstName, string lastName);
    }
}
