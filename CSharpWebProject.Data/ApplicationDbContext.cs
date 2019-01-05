using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSharpWebProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        //public DbSet<User> RubikUsers { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<SolveTime> SolveTimes { get; set; }

        public DbSet<CompetiveSolveTime> CompetiveSolveTimes { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Competitor> Competitors { get; set; }

        public DbSet<NewsPost> Posts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
                .Entity<User>()
                .HasMany(u => u.SolveTimes)
                .WithOne(av => av.User)
                .HasForeignKey(av => av.UserId)
                .OnDelete(DeleteBehavior.Cascade); ;


            builder
                .Entity<Competition>()
                .HasMany(c => c.Competitors)
                .WithOne(c => c.Competition)
                .HasForeignKey(c => c.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Competitor>()
                .HasMany(c => c.SolveTimes)
                .WithOne(s => s.Competitor)
                .HasForeignKey(c => c.CompetitorId)
                .OnDelete(DeleteBehavior.Cascade);


            builder
                .Entity<UserAchievement>()
                .HasKey(ua => new { ua.UserId, ua.AchievementId });

            builder
                .Entity<UserCompetition>()
                .HasKey(ua => new { ua.UserId, ua.CompetitionId });

            base.OnModelCreating(builder);
        }
    }
}

