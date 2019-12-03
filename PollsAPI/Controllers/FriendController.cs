using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using PollsAPI.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PollsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly PollsContext _context;
        private IMessageService _messageService;
        private IUserService _userService;

        public FriendController(PollsContext context, IMessageService messageService, IUserService userService)
        {
            _context = context;
            _messageService = messageService;
            _userService = userService;
        }

        // PUT: api/Friend/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFriend(long id, Friend friend)
        {
            if (id != friend.FriendID)
            {
                return BadRequest();
            }
            _context.Entry(friend).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Friend/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriend(long id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Friend
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetFriends()
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            var friends = await _context.Friends.Include(v => v.Receiver).Include(v => v.Sender)
                .Where(v => (v.SenderID == userID || v.ReceiverID == userID) && v.Status == "Accepted")
                .ToListAsync();

            List<GetUserDto> users = new List<GetUserDto>();

            foreach (Friend f in friends)
            {
                User user = (f.SenderID == userID ? f.Receiver : f.Sender);

                users.Add(new GetUserDto()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Activated = user.Activated,
                    UserID = user.UserID
                });
            }

            return Ok(users.OrderBy(u => u.Name).ToList());
        }

        // GET: api/Friend/requests
        [Authorize]
        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<GetFriendDto>>> GetFriendRequests()
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            return await _context.Friends.Where(f => f.ReceiverID == userID && f.Status == "Pending").Select(f => new GetFriendDto()
            {
                FriendID = f.FriendID,
                Sender = new GetUserDto()
                {
                    UserID = f.Sender.UserID,
                    Email = f.Sender.Email,
                    Name = f.Sender.Name,
                    Activated = f.Sender.Activated
                },
                Receiver = new GetUserDto()
                {
                    UserID = f.Receiver.UserID,
                    Email = f.Receiver.Email,
                    Name = f.Receiver.Name,
                    Activated = f.Receiver.Activated
                },
                Status = f.Status
            })
            .ToListAsync();
        }

        // POST: api/Friend
        [HttpPost("{email}")]
        public async Task<ActionResult<GetUserDto>> AddFriend(string email)
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
            User sender = _context.Users.Find(userID);

            User receiver = _context.Users.SingleOrDefault(u => u.Email == email);

            if (receiver == null)
            {
                Guid guid = Guid.NewGuid();

                receiver = _userService.Register(email, null, null, guid);

                if (receiver == null)
                    return BadRequest(new { message = "An error occured" });

                _messageService.SendInvitationMail(email, sender.Name, guid);
            }

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
