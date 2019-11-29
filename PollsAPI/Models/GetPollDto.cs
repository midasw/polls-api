using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class GetPollDto
    {
        public long PollID { get; set; }
        public string Name { get; set; }
        public List<PollAnswerDto> Answers { get; set; }
    }
}
