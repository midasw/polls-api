using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class DBInitializer
    {
        public static void Initialize(PollsContext context)
        {
            context.Database.EnsureCreated();
            // Look for any verkiezingen.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            context.Users.AddRange(
                new User { Name = "Test", Password = "test", Email = "test.test@thomasmore.be", Activated = true }
            //  new User { Name = "Midas", Password = "1234", Email = "midas.wouters@gmail.com", Activated = true }
            );

            context.SaveChanges();
/*
            //context.UserActivations.AddRange(new UserActivation { UserID = 5, Guid = "Lorem ipsum" });
            context.Polls.AddRange(
                new Poll { OwnerID = 1, Name = "Lorem ipsum dolor sit amet?" },
                new Poll { OwnerID = 1, Name = "Consectetur adipiscing elit?" },
                new Poll { OwnerID = 2, Name = "Wat doen we met oudjaar?" },
                new Poll { OwnerID = 2, Name = "Wanneer gaan we fitten?" },
                new Poll { OwnerID = 2, Name = "Welk huisdier gaan we in huis halen?" },
                new Poll { OwnerID = 2, Name = "Wie is de knapste?" }
            );
            context.PollAnswers.AddRange(
                new PollAnswer { PollID = 3, Answer = "Tijdspiegel" },
                new PollAnswer { PollID = 3, Answer = "Kiss Me" },
                new PollAnswer { PollID = 3, Answer = "Hofke" },
                new PollAnswer { PollID = 3, Answer = "10r" }
            );
            context.SaveChanges();*/
        }
    }
}
