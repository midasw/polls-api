using PollsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        bool UserExists(string email);
        User Register(string email, string password, string name, Guid guid);
        User RegisterInvitee(string password, string name, string guid);
        User Activate(string guid);
    }
}
