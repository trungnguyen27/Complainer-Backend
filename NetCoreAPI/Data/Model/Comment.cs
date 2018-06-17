using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class Comment : BaseModel
    {
        public string UserId { get; set; }
        public int FeedbackId { get; set; }
        public string Content { get; set; }
        public virtual User User { get; set; }
        public virtual Feedback Feedback { get; set; }
        public ICollection<Spam> Spams { get; set; }
    }
}
