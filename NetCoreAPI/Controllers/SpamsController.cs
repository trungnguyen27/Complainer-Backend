using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Data.Model;

namespace NetCoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Spams")]
    public class SpamsController : Controller
    {
        private readonly AppDbContext _context;

        public SpamsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Spams
        [HttpGet]
        public IEnumerable<Spam> GetSpams()
        {
            return _context.Spams;
        }

        // GET: api/Spams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var spam = await _context.Spams.SingleOrDefaultAsync(m => m.Id == id);

            if (spam == null)
            {
                return NotFound();
            }

            return Ok(spam);
        }

        // PUT: api/Spams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpam([FromRoute] int id, [FromBody] Spam spam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != spam.Id)
            {
                return BadRequest();
            }

            _context.Entry(spam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpamExists(id))
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

        // POST: api/Spams
        [HttpPost]
        public async Task<IActionResult> PostSpam([FromBody] Spam spam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Spams.Add(spam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpam", new { id = spam.Id }, spam);
        }

        // DELETE: api/Spams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var spam = await _context.Spams.SingleOrDefaultAsync(m => m.Id == id);
            if (spam == null)
            {
                return NotFound();
            }

            _context.Spams.Remove(spam);
            await _context.SaveChangesAsync();

            return Ok(spam);
        }

        private bool SpamExists(int id)
        {
            return _context.Spams.Any(e => e.Id == id);
        }
    }
}