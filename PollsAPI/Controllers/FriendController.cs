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
    public class FriendController : ControllerBase
    {
        private readonly PollsContext _context;

        public FriendController(PollsContext context)
        {
            _context = context;
        }

        // GET: api/Friend/requests
        [Authorize]
        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetFriendRequests()
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            List<GetUserDto> list = _context.Friends.Where(f => f.ReceiverID == userID).Select(p => new GetUserDto()
            {
                UserID = p.Sender.UserID,
                Email = p.Sender.Email,
                Name = p.Sender.Name,
                Activated = p.Sender.Activated,
            })
            .ToList();

            return Ok(list);
        }

        [HttpPost("{email}")]
        public async Task<ActionResult<GetUserDto>> AddFriend(string email)
        {
            User receiver = _context.Users.SingleOrDefault(u => u.Email == email);

            if (receiver == null)
            {
                receiver = new User
                {
                    Email = email
                };
                _context.Users.Add(receiver);
                _context.SaveChanges();
            }

            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            User sender = _context.Users.Find(userID);

            Friend friend = new Friend
            {
                Sender = sender,
                Receiver = receiver,
                Status = "Pending"
            };

            _context.Friends.Add(friend);

            await _context.SaveChangesAsync();

            return Ok(new GetUserDto
            {
                UserID = receiver.UserID,
                Name = receiver.Name,
                Email = receiver.Email,
                Activated = receiver.Activated
            });
        }
    }
}
