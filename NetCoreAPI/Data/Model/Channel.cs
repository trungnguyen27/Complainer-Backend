using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class Channel : BaseModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string StorageUrl { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string About { get; set; }
        public string AccessCode { get; set; }
        
        public virtual User User { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
