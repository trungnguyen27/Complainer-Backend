using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    [Route("api/Channels")]
    public class ChannelsController : Controller
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _appDbContext;

        public ChannelsController(IChannelRepository channelRepository, IUserRepository userRepository, AppDbContext appDbContext)
        {
            _channelRepository = channelRepository;
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }

        //// GET: api/Channels
        [HttpGet]
        public IEnumerable<Channel> GetChannels()
        {
            return _channelRepository.Channels;
        }

        //// GET: api/Channels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChannel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = _appDbContext.Channels.Include(c=> c.Feedbacks).SingleOrDefault(m => m.Id == id);
         

            if (channel == null)
            {
                return NotFound();
            }

            return Ok(channel);
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetChannelByUserId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = _channelRepository.Channels.Where(m => m.UserId == id);


            if (channel == null)
            {
                return NotFound();
            }

            return Ok(channel);
        }

        [HttpGet("Access/{id}")]
        public async Task<IActionResult> GetChannelByAccessCode([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = _channelRepository.Channels.SingleOrDefault(m => m.AccessCode == id);


            if (channel == null)
            {
                return NotFound();
            }

            return Ok(channel);
        }

        [HttpGet("Statistic/{id}")]
        public async Task<IActionResult> GetChannelStatistic([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = _appDbContext.Channels
                .Include(c => c.Feedbacks)
                .ThenInclude(fb => fb.Votes)
                .SingleOrDefault(c=> c.Id == id);

            if (channel == null)
            {
                return NotFound();
            }

            var votes = new List<Vote>();
            int upVote = 0;
            int downVote = 0;
            
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            foreach (var feedback in channel.Feedbacks)
            {
                votes.Concat(feedback.Votes.Where(v => v.CreatedDate == todaysDate));
            }

            foreach(var vote in votes)
            {
                if (vote.VoteStatus == 1) upVote++;
                if (vote.VoteStatus == 0) downVote++;
            }

            ChannelStatistic stats = new ChannelStatistic
            {
                TodayUpvotes = upVote,
                TodayDownvotes = downVote,
                TodayFeedbacks = channel.Feedbacks.Where(fb => fb.CreatedDate.Date == todaysDate.Date).Count(),
                TotalFeedbacks = channel.Feedbacks.Count(),
            };
            
            return Ok(stats);
        }


        // PUT: api/Channels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannel([FromRoute] int id, [FromBody] Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != channel.Id)
            {
                return BadRequest();
            }

            _appDbContext.Entry(channel).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(channel);
        }

        // POST: api/Channels
        [HttpPost]
        public async Task<IActionResult> PostChannel([FromBody] Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _channelRepository.Add(channel);
            }catch(SqlException exception)
            {
                return BadRequest(exception);
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return CreatedAtAction("GetChannel", new { id = channel.Id }, channel);
        }

        //// DELETE: api/Channels/5
        [HttpDelete("{id}")]
        public IActionResult DeleteChannel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _channel = _channelRepository.Channels.SingleOrDefault(m => m.Id == id);
            if (_channel == null)
            {
                return NotFound();
            }

            _channelRepository.Delete(_channel);

            return Ok(_channel);
        }

        private bool ChannelExists(int id)
        {
            return _channelRepository.Channels.Any(e => e.Id == id);
        }
    }
}