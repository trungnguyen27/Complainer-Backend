using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _appDbContext;
        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Comment> Comments => _appDbContext.Comments;

        public void Add(Comment channel)
        {
            throw new NotImplementedException();
        }

        public void Delete(Comment channel)
        {
            throw new NotImplementedException();
        }
    }
}
