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
    [Route("api/Comments")]
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly INotificationHub _hub;
        public CommentsController(AppDbContext context, INotificationHub hub)
        {
            _context = context;
            _hub = hub;
        }

        // GET: api/Comments
        [HttpGet]
        public IEnumerable<Comment> GetComments()
        {
            return _context.Comments;
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _context.Comments.SingleOrDefaultAsync(m => m.Id == id);

            var commentResponse = GetCommentResponse(comment);

            if (commentResponse == null)
            {
                return NotFound();
            }

            return Ok(commentResponse);
        }

        // GET: api/Comments/5
        [HttpGet("Feedback/{id}")]
        public IActionResult GetCommentByFeedback([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = _context.Comments.Include(c=> c.Feedback).Where(m => m.FeedbackId == id);

            var responses = new ObservableCollection<Comment>();

            foreach(var comment in comments)
            {
                responses.Add(GetCommentResponse(comment));
            }

            if (responses.Count == 0)
            {
                return NotFound();
            }

            return Ok(responses);
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment([FromRoute] int id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            var feedback = _context.Feedbacks.Include(f => f.Comments).SingleOrDefault(f => f.Id == comment.FeedbackId);
            var channel = _context.Channels.SingleOrDefault(c => c.Id == feedback.ChannelId);

            var subscribedUsers = feedback.Comments.Select(c => c.UserId);

            if(channel.UserId == comment.UserId) // official reply
            {
                var tags = subscribedUsers.Distinct().ToArray();
                await _hub.SendNotification(_hub.GetOfficialReplyToast(channel, feedback, comment), tags);
            }

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _context.Comments.SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

        private CommentResponse GetCommentResponse(Comment comment)
        {
            try
            {
                var commentResponse = new CommentResponse(comment);
                commentResponse.OfficialReplied = comment.UserId == comment.Feedback.UserId;
                commentResponse.IsSpam = _context.Spams.Where(s => s.CommentId == comment.Id).Count() > 10;
                return commentResponse;
            }catch(Exception)
            {
                return null;
            }
        }
    }
}