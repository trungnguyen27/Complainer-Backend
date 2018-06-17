using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly AppDbContext _appDbContext;
        public ChannelRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Channel channel)
        {
            Channel _channel = _appDbContext.Channels.SingleOrDefault(c => c.AccessCode == channel.AccessCode);
            User _user = _appDbContext.Users.SingleOrDefault(u => u.Id == channel.UserId);
            if (_channel == null)
            {
                _appDbContext.Channels.Add(channel);
                _appDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Channel Already Exists");
            }
        }

        public void Delete(Channel channel)
        {
            _appDbContext.Channels.Remove(channel);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Channel> Channels => _appDbContext.Channels;
    }
}
