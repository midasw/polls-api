using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class GetPollUserDto
    {
        public long PollUserID { get; set; }
        public bool IsOwner { get; set; }
        public GetUserDto User { get; set; }
        public long PollID { get; set; }
    }
}
