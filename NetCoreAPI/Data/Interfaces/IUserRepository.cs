using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface IUserRepository
    {
        void Add(User channel);
        void Delete(User channel);
        IEnumerable<User> Users { get; }
    }
}
