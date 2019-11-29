using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class PollUser
    {
        [Key]
        public long PollUserID { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
        public long PollID { get; set; }
        public Poll Poll { get; set; }
    }
}
