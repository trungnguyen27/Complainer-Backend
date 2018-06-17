using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class Vote : BaseModel
    {
        public string UserId { get; set; }
        public int FeedbackId { get; set; }
        public int VoteStatus { get; set; }
        public virtual User User { get; set; }
        public virtual Feedback Feedback { get; set; }
    }
}
