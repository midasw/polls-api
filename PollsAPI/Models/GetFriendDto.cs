using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class GetFriendDto
    {
        public long FriendID { get; set; }
        public GetUserDto Sender { get; set; }
        public GetUserDto Receiver { get; set; }
        public string Status { get; set; }
    }
}
