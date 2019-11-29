using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class Friend
    {
        public long FriendID { get; set; }
        [ForeignKey("Sender")]
        public long SenderID { get; set; }
        public User Sender { get; set; }
        [ForeignKey("Receiver")]
        public long ReceiverID { get; set; }
        public User Receiver { get; set; }
        public string Status { get; set; }
    }
}
