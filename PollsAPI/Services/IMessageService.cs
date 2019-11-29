using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Services
{
    public interface IMessageService
    {
        void Send(string email, string subject, string message);
        void SendActivationMail(string email, string name, Guid guid);
    }
}
