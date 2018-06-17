using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface IVoteRepository
    {
        void Add(Vote vote);
        void Update(Vote vote);
        void Delete(int vote);
        IEnumerable<Vote> Votes { get; }
    }
}
