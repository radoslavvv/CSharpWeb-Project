using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Services
{
    public class NewsService : Service, INewsService
    {
        public NewsService(ApplicationDbContext context) : base(context)
        {
        }

        public List<NewsPost> GetAllPosts()
        {
            List<NewsPost> posts = this.Context
                .Posts
                .ToList();

            return posts;
        }

        public bool AddNews(NewsPost post)
        {
            if(post == null)
            {
                return false;
            }

            this.Context.Posts.Add(post);
            this.Context.SaveChanges();
            return true;
        }

        public bool DeleteNews(int postId)
        {
            NewsPost post = this.Context
                .Posts
                .FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return false;
            }

            this.Context.Posts.Remove(post);
            this.Context.SaveChanges();
            return true;
        }

        public NewsPost GetNewsById(int postId)
        {
            NewsPost post = this.Context
                .Posts
                .FirstOrDefault(p => p.Id == postId);

            return post;
        }
    }
}
