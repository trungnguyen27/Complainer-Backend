using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Repositories
{
    public class SpamRepository : ISpamRepository
    {
        private readonly AppDbContext _appDbContext;
        public SpamRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Spam> Spams => _appDbContext.Spams;

        public void Add(Spam channel)
        {
            throw new NotImplementedException();
        }

        public void Delete(Spam channel)
        {
            throw new NotImplementedException();
        }
    }
}
