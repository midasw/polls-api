using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class PollAnswer
    {
        [Key]
        public long AnswerID { get; set; }
        public string Answer { get; set; }
        public long PollID { get; set; }
        public Poll Poll { get; set; }
        public ICollection<PollVote> Votes { get; set; }
    }
}
