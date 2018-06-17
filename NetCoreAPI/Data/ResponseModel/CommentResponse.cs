using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.ResponseModel
{
    public class CommentResponse : Comment
    {
        public CommentResponse(Comment comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            FeedbackId = comment.FeedbackId;
            Content = comment.Content;
            CreatedDate = comment.CreatedDate;
        }

        public bool OfficialReplied { get; set; }
        public bool IsSpam { get; set; }
    }
}
