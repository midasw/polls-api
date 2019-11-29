using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PollsAPI.Models
{
    public class GetUserDto
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Activated { get; set; }
    }
}
