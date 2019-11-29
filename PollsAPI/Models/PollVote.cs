using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class PollVote
    {
        [Key]
        public long VoteID { get; set; }
        [ForeignKey("Answer")]
        public long AnswerID { get; set; }
        public PollAnswer Answer { get; set; }
        [ForeignKey("User")]
        public long UserID { get; set; }
        public User User { get; set; }
    }
}
