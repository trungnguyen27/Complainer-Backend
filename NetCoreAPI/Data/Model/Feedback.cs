using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class Feedback : BaseModel
    {

        public string UserId { get; set; }
        public int ChannelId { get; set; }
        public string Content{get;set;}
        public string Location { get; set; }
        public int Action { get; set; }
        public virtual User User { get; set; }
        public virtual Channel Channel { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public string AttachImage { get; set; }
    }
}
