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

            List<GetUserDto> list1 = _context.Friends
                .Where(f => f.Status == "Accepted" && f.SenderID == userID)
                .Select(f => new GetUserDto()
                {
                    UserID = f.Receiver.UserID,
                    Email = f.Receiver.Email,
                    Name = f.Receiver.Name,
                    Activated = f.Receiver.Activated
                })
                .ToList();

            List<GetUserDto> list2 = _context.Friends
                .Where(f => f.Status == "Accepted" && f.ReceiverID == userID)
                .Select(f => new GetUserDto()
                {
                    UserID = f.Sender.UserID,
                    Email = f.Sender.Email,
                    Name = f.Sender.Name,
                    Activated = f.Sender.Activated
                })
                .ToList();

            /*
            var users = _context.Users.Join(_context.Friends, cu => cu.UserID, f => f.SenderID, (cu, f) => new { cu })
                .ToList();*/


            return Ok(list1.Concat(list2).ToList());
        }

        // GET: api/Friend/requests
        [Authorize]
        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<GetFriendDto>>> GetFriendRequests()
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            /*            List<GetUserDto> list = _context.Friends.Where(f => f.ReceiverID == userID && f.Status == "Pending").Select(p => new GetUserDto()
                        {
                            UserID = p.Sender.UserID,
                            Email = p.Sender.Email,
                            Name = p.Sender.Name,
                            Activated = p.Sender.Activated,
                        })
                        .ToList();*/

            //List<Friend> list =_context.Friends.Where(f => f.ReceiverID == userID && f.Status == "Pending").Include(f => f.Receiver).Include(f => f.Sender).ToList();

            List<GetFriendDto> list = _context.Friends.Where(f => f.ReceiverID == userID && f.Status == "Pending").Select(f => new GetFriendDto()
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
