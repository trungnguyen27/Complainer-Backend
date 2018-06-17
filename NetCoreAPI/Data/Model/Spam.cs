using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class Spam : BaseModel
    {
        public string UserId { get; set; }
        public int CommentId { get; set; }
        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
