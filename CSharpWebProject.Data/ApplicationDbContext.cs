using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSharpWebProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Achievement> Achievements { get; set; }

        //public DbSet<Category> Categories { get; set; }

        //public DbSet<Team> Teams { get; set; }

        //public DbSet<Video> Videos { get; set; }

        //public DbSet<UsersArticles> UsersArticles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //builder
            //    .Entity<User>()
            //    .HasMany(u => u.ArticlesVoted)
            //    .WithOne(av => av.User)
            //    .HasForeignKey(av => av.UserId);

            //builder
            //    .Entity<Article>()
            //    .HasMany(a => a.UsersVoted)
            //    .WithOne(uv => uv.Article)
            //    .HasForeignKey(uv => uv.ArticleId);

            //builder
            //    .Entity<UsersArticles>()
            //    .HasKey(ua => new { ua.UserId, ua.ArticleId });

            base.OnModelCreating(builder);
        }
    }
}

