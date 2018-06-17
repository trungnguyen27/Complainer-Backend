using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _appDbContext;
        public VoteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Vote> Votes => _appDbContext.Votes;

        public void Add(Vote vote)
        {
            var _vote = _appDbContext.Votes.SingleOrDefault(v => v.UserId == vote.UserId && v.FeedbackId == vote.FeedbackId);
            if(_vote != null)
            {
                if(_vote.VoteStatus == vote.VoteStatus)
                {
                    vote.VoteStatus = -1; 
                }
                throw new Exception("Duplicate");
            }
            _appDbContext.Votes.Add(vote);
            _appDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var _vote = _appDbContext.Votes.SingleOrDefault(v => v.Id == id);
            if(_vote!= null)
            {
                _appDbContext.Votes.Remove(_vote);
                _appDbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void Update(Vote vote)
        {
            var _vote = _appDbContext.Votes.SingleOrDefault(v => v.UserId == vote.UserId && v.FeedbackId == vote.FeedbackId);
            _appDbContext.Votes.Update(vote);
        }
    }
}
