using PollsAPI.Helpers;
using PollsAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PollsAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly PollsContext _context;
        public UserService(IOptions<AppSettings> appSettings, PollsContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public User Authenticate(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.UserID.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Name", user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;
            return user;
        }

        public bool UserExists(string email)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);
            return (user == null ? false : true);
        }

        public User Register(string email, string password, string name, Guid guid)
        {
            var userActivation = new UserActivation
            {
                User = new User()
                {
                    Email = email,
                    Password = password,
                    Name = name
                },
                Guid = guid.ToString()
            };

            _context.UserActivations.Add(userActivation);
            _context.SaveChanges();

            return userActivation.User;
        }

        public User Activate(string guid)
        {
            var userActivation = _context.UserActivations.Include(u=>u.User).SingleOrDefault(x => x.Guid == guid);

            if (userActivation == null)
                return null;

            var user = userActivation.User;

            /*            var user = _context.Users.Find(userActivation.UserID);

                        if (user == null)
                            return null;*/

            user.Activated = true;

            _context.UserActivations.Remove(userActivation);

            _context.SaveChanges();

            return user;
        }
    }
}
