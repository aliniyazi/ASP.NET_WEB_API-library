using API.DataAccess;
using API.DataAccess.Models;
using API.Repositories.Contracts;

namespace API.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
