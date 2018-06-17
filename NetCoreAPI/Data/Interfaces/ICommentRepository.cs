using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface ICommentRepository
    {
        void Add(Comment channel);
        void Delete(Comment channel);
        IEnumerable<Comment> Comments { get; }
    }
}
