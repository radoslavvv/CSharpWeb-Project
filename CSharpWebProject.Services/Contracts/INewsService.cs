using System.Collections.Generic;
using CSharpWebProject.Models.EntityModels;

namespace CSharpWebProject.Services
{
    public interface INewsService
    {
        List<NewsPost> GetAllPosts();
    }
}