using System.Collections.Generic;
using CSharpWebProject.Models.EntityModels;

namespace CSharpWebProject.Services
{
    public interface INewsService
    {
        List<NewsPost> GetAllPosts();

        bool AddNews(NewsPost post);

        bool DeleteNews(int postId);

        NewsPost GetNewsById(int postId);
    }
}