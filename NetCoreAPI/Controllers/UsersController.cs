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
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _appDbContext;

        public UsersController(IUserRepository userRepository, AppDbContext appDbContext)
        {
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }

        //// GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.Users;
        }

        //// GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.Users.SingleOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //// PUT: api/Users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _userRepository.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _userRepository.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //POST: api/Users
        [HttpPost]
        public IActionResult PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _user = _userRepository.Users.SingleOrDefault(u => u.Name == user.Name);

            if(_user == null)
            {
                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }

            return BadRequest("User Already Exists");
        }

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _userRepository.Users.SingleOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _userRepository.Users.Remove(user);
        //    await _userRepository.SaveChangesAsync();

        //    return Ok(user);
        //}

        private bool UserExists(string id)
        {
            return _userRepository.Users.Any(e => e.Id == id);
        }
    }
}