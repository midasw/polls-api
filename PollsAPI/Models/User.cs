using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Activated { get; set; }
        [NotMapped]
        public string Token { get; set; }
        public ICollection<PollVote> PollVotes { get; set; }
        public ICollection<PollUser> PollUsers { get; set; }
        public ICollection<Friend> SentRequests { get; set; }
        public ICollection<Friend> ReceivedRequests { get; set; }
    }
}
