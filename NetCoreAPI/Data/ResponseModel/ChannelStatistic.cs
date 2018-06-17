using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Model
{
    public class ChannelStatistic
    {
        public int TodayDownvotes { get; set; }
        public int TodayUpvotes { get; set; }
        public int TodayFeedbacks { get; set; }
        public int comments { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
