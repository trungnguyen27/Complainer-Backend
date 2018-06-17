using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _appDbContext;
        public FeedbackRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(Feedback feedback)
        {
        
        }

        public void Delete(Feedback feedback)
        {
            _appDbContext.Feedbacks.Remove(feedback);
            _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Feedback> Feedbacks => _appDbContext.Feedbacks;
    }
}
