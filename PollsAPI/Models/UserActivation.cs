using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class UserActivation
    {
        public long ID { get; set; }
        [ForeignKey("User")]
        public long UserID { get; set; }
        public User User { get; set; }
        public string Guid { get; set; }
    }
}
