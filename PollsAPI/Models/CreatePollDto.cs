using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class CreatePollDto
    {
        public string Name { get; set; }
        public List<string> Answers { get; set; }
    }
}
