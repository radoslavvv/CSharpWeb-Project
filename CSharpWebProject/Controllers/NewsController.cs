using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;
using CSharpWebProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebProject.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            List<NewsPostViewModel> posts = this.newsService.GetAllPosts()
                .Select(n => new NewsPostViewModel()
                {
                    Id = n.Id,
                    AuthorName = n.Author.UserName,
                    Content = n.Content,
                    Date = n.Date.ToString("dd/MM/yyyy"),
                    Title = n.Title
                }).ToList();

            PaginatedList<NewsPostViewModel> newsPage = await PaginatedList<NewsPostViewModel>.CreateAsync(posts, page, pageSize);

            return View(newsPage);
        }
    }
}