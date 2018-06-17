using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;

namespace NetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Votes")]
    public class VotesController : Controller
    {
        private readonly IVoteRepository _voteRepository;
        private readonly AppDbContext _appDbContext;

        public VotesController(IVoteRepository voteRepository, AppDbContext appDbContext)
        {
            _voteRepository = voteRepository;
            _appDbContext = appDbContext;
        }

        // GET: api/Votes
        [HttpGet]
        public IEnumerable<Vote> GetVotes()
        {
            return _voteRepository.Votes;
        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vote = _voteRepository.Votes.Where(v => v.Id == id);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }


        // GET: api/Votes/5
        [HttpGet("/Feedback/{id}")]
        public async Task<IActionResult> GetVoteByFeedbackId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vote = _voteRepository.Votes.Where(v=> v.FeedbackId == id);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        // GET: api/Votes/5
        [HttpGet("/Channel/{id}")]
        public async Task<IActionResult> GetVoteByChannelId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var vote = _voteRepository.Votes.Where(v => v.Feedback.ChannelId == id);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        // PUT: api/Votes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote([FromRoute] int id, [FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vote.Id)
            {
                return BadRequest();
            }

            _appDbContext.Entry(vote).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(vote);
        }

        // POST: api/Votes
        [HttpPost]
        public async Task<IActionResult> PostVote([FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var _vote = _appDbContext.Votes.SingleOrDefault(v => v.UserId == vote.UserId && v.FeedbackId == vote.FeedbackId);
                if (_vote != null)
                {
                    if (_vote.VoteStatus == vote.VoteStatus)
                    {
                        _vote.VoteStatus = -1;
                    }
                    else
                    {
                        _vote.VoteStatus = vote.VoteStatus;
                    }
                    return await PutVote(_vote.Id, _vote);
                }
                _appDbContext.Votes.Add(vote);
                _appDbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                return BadRequest(ModelState);
            }
            

            return CreatedAtAction("GetVote", new { id = vote.Id }, vote);
        }

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _voteRepository.Delete(id);
            }catch(KeyNotFoundException ex)
            {
                return NotFound();
            }

            return Ok(id);
        }

        private bool VoteExists(int id)
        {
            return _appDbContext.Votes.Any(e => e.Id == id);
        }
    }
}