using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class PollVoteDto
    {
        public long VoteID { get; set; }
        public long AnswerID { get; set; }
    //  public long UserID { get; set; }
        public GetUserDto User { get; set; }
    }
}
