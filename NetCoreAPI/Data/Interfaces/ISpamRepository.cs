using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface ISpamRepository
    {
        void Add(Spam channel);
        void Delete(Spam channel);
        IEnumerable<Spam> Spams { get; }
    }
}
