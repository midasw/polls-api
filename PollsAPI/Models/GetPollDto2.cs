using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class GetPollDto2
    {
        public long PollID { get; set; }
        public GetUserDto Owner { get; set; }
        public string Name { get; set; }
    }
}
