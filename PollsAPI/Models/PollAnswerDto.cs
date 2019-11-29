using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class PollAnswerDto
    {
        public long AnswerID { get; set; }
        public string Answer { get; set; }
        public List<PollVoteDto> Votes { get; set; }
    }
}
