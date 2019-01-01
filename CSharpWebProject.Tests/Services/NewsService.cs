using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Tests.Services
{
    public class NewsService : Service
    {
        public NewsService(ApplicationDbContext context) : base(context)
        {
        }

        public List<NewsPost> GetAllPosts()
        {
            List<NewsPost> posts = this.Context.Posts.ToList();

            return posts;
        }
    }
}
