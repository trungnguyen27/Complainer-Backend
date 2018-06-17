using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string DomainUrl { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<Comment> Comments{ get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Spam> Spams { get; set; }
    }
}
