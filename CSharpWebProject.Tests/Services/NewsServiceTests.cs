using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Tests.Services
{
    public class NewsServiceTests
    {
        [Test]
        public void GetAllNewsReturnsCorrectResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            List<NewsPost> news = new List<NewsPost>()
            {
                new NewsPost()
                {
                    Id = 1
                },
                new NewsPost()
                {
                    Id = 2
                },
            };

            dbContext.Posts.AddRange(news);
            dbContext.SaveChanges();

            NewsService newsService = new NewsService(dbContext);

            List<NewsPost> resultNewsPosts = newsService.GetAllPosts();

            Assert.AreEqual(news, resultNewsPosts);
        }

        [Test]
        public void AddNewsActuallyAddsPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            NewsService newsService = new NewsService(dbContext);
            NewsPost post = new NewsPost();

            bool result = newsService.AddNews(post);

            int newsPostsCount = dbContext.Posts.Count();
            Assert.AreEqual(1, newsPostsCount);
        }

        [Test]
        public void AddNewsReturnsFalseWhenPostIsNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            NewsService newsService = new NewsService(dbContext);
            NewsPost post = null;
            bool result = newsService.AddNews(post);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void RemoveNewsActuallyRemovesNews()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            NewsService newsService = new NewsService(dbContext);
            int postId = 1;
            NewsPost post = new NewsPost()
            {
                Id = postId,
            };

            newsService.AddNews(post);
            newsService.DeleteNews(postId);

            int newsPostsCount = dbContext.Posts.Count();
            Assert.AreEqual(0, newsPostsCount);
        }

        [Test]
        public void RemoveNewsReturnsFalseWhenPostIdIsIncorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            NewsService newsService = new NewsService(dbContext);
            int postId = 1;
            NewsPost post = new NewsPost()
            {
                Id = postId,
            };

            newsService.AddNews(post);
            bool result = newsService.DeleteNews(15);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void GetNewsByIdReturnsCorrectResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            NewsService newsService = new NewsService(dbContext);
            int postId = 1;
            NewsPost post = new NewsPost()
            {
                Id = postId,
            };

            newsService.AddNews(post);

            NewsPost result = newsService.GetNewsById(postId);

            Assert.AreEqual(post, result);
        }

        [Test]
        public void GetNewsByIdReturnsNullWhenIdIsIncorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            NewsService newsService = new NewsService(dbContext);
            int postId = 1;
            NewsPost post = new NewsPost()
            {
                Id = postId,
            };

            newsService.AddNews(post);

            NewsPost result = newsService.GetNewsById(postId);

            Assert.AreEqual(post, result);
        }
    }
}
