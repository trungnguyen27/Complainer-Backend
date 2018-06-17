using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<User> Users => _appDbContext.Users;

        public void Add(User channel)
        {
            throw new NotImplementedException();
        }

        public void Delete(User channel)
        {
            throw new NotImplementedException();
        }
    }
}
