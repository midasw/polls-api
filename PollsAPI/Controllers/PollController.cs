﻿using System;
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
    public class PollController : ControllerBase
    {
        private readonly PollsContext _context;

        public PollController(PollsContext context)
        {
            _context = context;
        }

        // GET: api/Poll
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poll>>> GetPolls()
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
            return await _context.Polls.Where(p => p.OwnerID == userID).OrderBy(p => p.Name).ToListAsync();
        }

        // GET: api/Poll/shared
        [Authorize]
        [HttpGet("shared")]
        public async Task<ActionResult<IEnumerable<GetPollDto2>>> GetSharedPolls()
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            return await _context.PollUsers.Where(pu => pu.UserID == userID).Select(pu => new GetPollDto2() {
                PollID = pu.Poll.PollID,
                Name = pu.Poll.Name,
                Owner = new GetUserDto()
                {
                    UserID = pu.Poll.Owner.UserID,
                    Name = pu.Poll.Owner.Name,
                    Email = pu.Poll.Owner.Email,
                    Activated = pu.Poll.Owner.Activated
                }
            })
            .OrderBy(pu => pu.Name)
            .ToListAsync();
        }

        // GET: api/Poll/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPollDto>> GetPoll(long id)
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            PollUser pu = _context.PollUsers.SingleOrDefault(x => x.PollID == id && x.UserID == userID);
            Poll p = _context.Polls.SingleOrDefault(p => p.PollID == id);

            if (pu == null && p.OwnerID != userID)
                return BadRequest(new { message = "You have no permission to view this poll. Get out of here you sneaky bastard!" });

            GetPollDto dto = await _context.Polls.Where(p => p.PollID == id).Select(p => new GetPollDto()
            {
                PollID = p.PollID,
                Name = p.Name,
                Answers = p.Answers.Select(a => new PollAnswerDto()
                {
                    AnswerID = a.AnswerID,
                    Answer = a.Answer,
                    Votes = a.Votes.Select(v => new PollVoteDto()
                    {
                        VoteID = v.VoteID,
                        AnswerID = v.AnswerID,
                        User = new GetUserDto()
                        {
                            UserID = v.User.UserID,
                            Email = v.User.Email,
                            Name = v.User.Name,
                            Activated = v.User.Activated
                        }
                    })
                    .OrderBy(v => v.User.Name)
                    .ToList()
                })
                .ToList()
            })
            .FirstOrDefaultAsync();

            dto.Answers = dto.Answers.OrderByDescending(a => a.Votes.Count).ToList();
            return Ok(dto);
        }

        // POST: api/Poll
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GetPollDto>> CreatePoll([FromBody] CreatePollDto dto)
        {
            long userID = Convert.ToInt64(User.Claims.FirstOrDefault(c => c.Type == "UserID").Value);

            Poll poll = new Poll()
            {
                Name = dto.Name,
                OwnerID = userID,
                Answers = new List<PollAnswer>()
            };

            dto.Answers.ForEach(a => poll.Answers.Add(new PollAnswer()
            {
                Answer = a
            }));

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return Ok(new GetPollDto()
            {
                PollID = poll.PollID,
                Name = poll.Name
            });
        }
    }
}
