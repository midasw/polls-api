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
        public async Task<ActionResult<IEnumerable<GetPollUserDto>>> GetPollUsers(long id)
        {
            List<GetPollUserDto> pollUsers = _context.PollUsers.Where(pu => pu.PollID == id).Select(pu => new GetPollUserDto()
            {
                PollUserID = pu.PollUserID,
                PollID = pu.PollID,
                User = new GetUserDto()
                {
                    Email = pu.User.Email,
                    Name = pu.User.Name,
                    Activated = pu.User.Activated,
                    UserID = pu.User.UserID
                }
            })
            .ToList();

            return pollUsers;

            //return await _context.PollUsers.Where(pu => pu.PollID == id).ToListAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PollUser>> PostPollUser(PollUser pu)
        {
            _context.PollUsers.Add(pu);
            await _context.SaveChangesAsync();

            return Ok(pu);
        }

    }
}
