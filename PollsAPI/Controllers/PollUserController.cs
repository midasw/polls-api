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
    public class PollUserController : ControllerBase
    {
        private readonly PollsContext _context;

        public PollUserController(PollsContext context)
        {
            _context = context;
        }

        // GET: api/PollUser
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<PollUser>>> GetPollUsers(long id)
        {
            return await _context.PollUsers.Where(pu => pu.PollID == id).ToListAsync();
        }
    }
}
