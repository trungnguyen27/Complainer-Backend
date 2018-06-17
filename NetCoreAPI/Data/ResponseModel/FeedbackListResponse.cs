using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.ResponseModel
{
    public class FeedbackListResponse
    {
        public ICollection<Feedback> Feedbacks { get; set; }
        public int NumberOfUpvotes { get; set; }
        public int NumberOfDownvotes { get; set; }
        public bool IsOfficialReplied { get; set; }
    }
}
