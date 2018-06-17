using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using NetCoreAPI.Data.ResponseModel;

namespace NetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Feedbacks")]
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackRepository _context;
        private readonly AppDbContext _appDbContext;
        private readonly INotificationHub _hub;

        public FeedbacksController(IFeedbackRepository context, AppDbContext appDbContext, INotificationHub hub)
        {
            _context = context;
            _appDbContext = appDbContext;
            _hub = hub;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public IEnumerable<Feedback> GetFeedbacks()
        {
            return _context.Feedbacks;
        }

        //GET: api/Feedbacks/5
        [HttpGet("{id}")]
        public IActionResult GetFeedback([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = _appDbContext.Feedbacks.Include(f => f.Comments).Include(f => f.Votes).Include(c => c.Channel).SingleOrDefault(m => m.Id == id);

            if (feedback == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id}/{userId}")]
        public IActionResult GetFeedbackByUserId([FromRoute]int id, [FromRoute]string userId)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = _appDbContext.Feedbacks.Include(f=> f.Comments).Include(f => f.Votes).Include(c=> c.Channel).SingleOrDefault(m => m.Id == id);

            if (feedback == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(GetFeedbackStats(feedback, userId));
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // GET: api/Feedbacks/Channel/5
        [HttpGet("Channel/{id}/{userId}")]
        public IActionResult GetFeedbackByChannel([FromRoute] int id, [FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedbacks = _appDbContext.Feedbacks.Include(fb => fb.Votes).Where(m => m.ChannelId == id).Include(fb => fb.Channel);

            if (feedbacks == null)
            {
                return NotFound();
            }

            ICollection<FeedbackResponse> response = new ObservableCollection<FeedbackResponse>();
            foreach(var feedback in feedbacks)
            {
            
                response.Add(GetFeedbackStats(feedback, userId));
            }

            return Ok(response);
        }

        // GET: api/Feedbacks/User/5
        [HttpGet("User/{id}")]
        public IActionResult GetFeedbackByUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedbacks = _appDbContext.Feedbacks.Include(fb => fb.Votes).Where(m => m.UserId == id).Include(fb => fb.Channel).ToList();

            if (feedbacks == null)
            {
                return NotFound();
            }

            ICollection<FeedbackResponse> response = new ObservableCollection<FeedbackResponse>();
            foreach (var feedback in feedbacks)
            {
         
                response.Add(GetFeedbackStats(feedback, id));
            }

            return Ok(response);
        }

        // PUT: api/Feedbacks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback([FromRoute] int id, [FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedback.Id)
            {
                return BadRequest();
            }

           
            _appDbContext.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();

                var _feedback = _appDbContext.Feedbacks.Include(f => f.Channel).SingleOrDefault(f => f.Id == feedback.Id);

                var toast = _hub.GetActionChangedToast(_feedback.Channel, _feedback, null);
                if (toast != null)
                    await _hub.SendNotification(toast, new string[] { feedback.UserId });

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(feedback);
        }

        // POST: api/Feedbacks
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _appDbContext.Feedbacks.Add(feedback);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction("GetFeedbackByUserId", new { id = feedback.Id, userId = feedback.UserId }, feedback);
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = _appDbContext.Feedbacks
                .Include(fb => fb.Comments)
                .Include(fb=>fb.Votes).SingleOrDefault(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Deleted = true;
            _appDbContext.Entry(feedback).State = EntityState.Modified;

            _appDbContext.Comments.RemoveRange(feedback.Comments);
            _appDbContext.Votes.RemoveRange(feedback.Votes);
            await _appDbContext.SaveChangesAsync();

            return Ok(feedback);
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.Id == id);
        }

        private FeedbackResponse GetFeedbackStats(Feedback feedback, string userId)
        {
            if(feedback.Deleted)
            {
                FeedbackResponse response = new FeedbackResponse()
                {
                    Deleted = true,
                    Content = "Phản hồi đã bị xóa",

                    ChannelId = feedback.Channel.Id,
                    ChannelName = feedback.Channel.Name
                };
                return response;
            }

            FeedbackResponse feedbackResponse = new FeedbackResponse(feedback);
            feedbackResponse.Upvote = feedback.Votes.Where(v => v.VoteStatus == 1).Count();
            feedbackResponse.Downvote = feedback.Votes.Where(v => v.VoteStatus == 0).Count();

            feedbackResponse.IsOfficialReplied = _appDbContext.Comments.Where(c => c.UserId == feedback.Channel.UserId).Count() > 0;

            feedbackResponse.CommentCount = _appDbContext.Comments.Where(c => c.FeedbackId == feedback.Id).Count();

            feedbackResponse.UpvoteEnable = feedback.Votes.SingleOrDefault(v => v.UserId == userId && v.FeedbackId == feedback.Id && v.VoteStatus == 1) != null;
            feedbackResponse.DownvoteEnable = feedback.Votes.SingleOrDefault(v => v.UserId == userId && v.FeedbackId == feedback.Id && v.VoteStatus == 0) != null;

            feedbackResponse.ChannelId = feedback.Channel.Id;
            feedbackResponse.ChannelName = feedback.Channel.Name;
            return feedbackResponse;
        }
    }
}