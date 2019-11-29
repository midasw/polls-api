using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollsAPI.Models;
using PollsAPI.Services;

namespace PollsAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        private IUserService _userService;
        private IMessageService _messageService;
        public UserController(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            if (!user.Activated)
                return BadRequest(new { message = "User is not activated" });

            return Ok(user);
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel param)
        {
            if (_userService.UserExists(param.Email))
                return BadRequest(new { message = "A user with this email address already exists" });

            if (param.Password != param.PasswordConfirmation)
                return BadRequest(new { message = "Passwords don't match" });

            Guid guid = Guid.NewGuid();

            var user = _userService.Register(param.Email, param.Password, param.Name, guid);

            if (user == null)
                return BadRequest(new { message = "An error occured" });

            _messageService.SendActivationMail(param.Email, param.Name, guid);

            return Ok(user);
        }
        [HttpGet("activate/{guid}")]
        public IActionResult Activate(string guid)
        {
            var user = _userService.Activate(guid);

            if (user == null)
                return BadRequest(new { message = "Invalid activation link" });

            return Ok(user);
        }
    }
}
