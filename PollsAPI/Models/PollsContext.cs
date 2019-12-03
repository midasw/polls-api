using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsAPI.Models
{
    public class PollsContext : DbContext
    {
        public PollsContext(DbContextOptions<PollsContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivation> UserActivations { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }
        public DbSet<PollVote> PollVotes { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<PollUser> PollUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property(p => p.Activated).HasDefaultValue(false);
            modelBuilder.Entity<UserActivation>().ToTable("UserActivation");

            modelBuilder.Entity<Poll>().ToTable("Poll");
            modelBuilder.Entity<PollAnswer>().ToTable("PollAnswer");
            modelBuilder.Entity<PollVote>().ToTable("PollVote");
            modelBuilder.Entity<PollUser>().ToTable("PollUser");
            modelBuilder.Entity<Friend>().ToTable("Friend");

            modelBuilder.Entity<PollVote>()
                .HasOne(p => p.User)
                .WithMany(p => p.PollVotes)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PollUser>()
                .HasOne(p => p.User)
                .WithMany(p => p.PollUsers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Receiver)
                .WithMany(f => f.ReceivedRequests)
                .HasForeignKey(f => f.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Sender)
                .WithMany(f => f.SentRequests)
                .HasForeignKey(f => f.SenderID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
