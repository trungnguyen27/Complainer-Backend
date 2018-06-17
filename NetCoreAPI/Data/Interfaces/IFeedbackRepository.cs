using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface IFeedbackRepository
    {
        Task Add(Feedback feedback);
        void Delete(Feedback feedback);
        IEnumerable<Feedback> Feedbacks { get; }
    }
}
