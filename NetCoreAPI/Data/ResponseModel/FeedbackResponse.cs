using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.ResponseModel
{
    public class FeedbackResponse : Feedback
    {
        public FeedbackResponse()
        {

        }
        public FeedbackResponse(Feedback feedback)
        {
            Id = feedback.Id;
            UserId = feedback.UserId;
            ChannelId = feedback.ChannelId;
            Content = feedback.Content;
            Location = feedback.Location;
            Action = feedback.Action;
            AttachImage = feedback.AttachImage;
            CreatedDate = feedback.CreatedDate;
            Deleted = feedback.Deleted;
        }
        public int Downvote { get; set; }
        public int Upvote { get; set; }
        
        public int CommentCount { get; set; }

        public bool UpvoteEnable { get; set; }
        public bool DownvoteEnable { get; set; }
        public bool IsOfficialReplied { get; set; }
        
        public string ChannelName { get; set; }

    }
}
