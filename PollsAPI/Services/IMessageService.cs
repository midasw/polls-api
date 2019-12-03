using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Services
{
    public interface IMessageService
    {
        void SendActivationMail(string email, string name, Guid guid);
        void SendInvitationMail(string email, string inviterName, Guid guid);
    }
}
