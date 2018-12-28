using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSharpWebProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> RubikUsers { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<SolveTime> SolveTimes { get; set; }

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
                .HasForeignKey(av => av.UserId);

            builder
                .Entity<User>()
                .HasMany(a => a.Achievements)
                .WithOne(av => av.User)
                .HasForeignKey(av => av.UserId);

            //builder
            //    .Entity<UsersArticles>()
            //    .HasKey(ua => new { ua.UserId, ua.ArticleId });

            base.OnModelCreating(builder);
        }
    }
}

