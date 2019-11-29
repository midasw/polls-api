using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollsAPI.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PollsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollVoteController : ControllerBase
    {
        private readonly PollsContext _context;

        public PollVoteController(PollsContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PollVote>> PostVote(PollVote vote)
        {
            _context.PollVotes.Add(vote);
            await _context.SaveChangesAsync();

            return Ok(vote);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PollVote>> DeleteVote(long id)
        {
            var vote = await _context.PollVotes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.PollVotes.Remove(vote);
            await _context.SaveChangesAsync();

            return vote;
        }
    }
}
