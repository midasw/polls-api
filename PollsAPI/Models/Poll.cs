using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class Poll
    {
        public long PollID { get; set; }
        [ForeignKey("Owner")]
        public long OwnerID { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        public ICollection<PollAnswer> Answers { get; set; }
    }
}
