using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface IChannelRepository
    {
        void Add(Channel channel);
        void Delete(Channel channel);
        IEnumerable<Channel> Channels { get; }
    }
}
