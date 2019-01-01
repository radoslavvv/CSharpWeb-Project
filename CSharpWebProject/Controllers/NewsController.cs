using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            List<NewsPostViewModel> posts = this.newsService.GetAllPosts().Select(n=> new NewsPostViewModel()
            {
                AuthorName = n.Author.UserName,
                Content = n.Content.Substring(0,25) + "...",
                Date = n.Date.ToString("dd/MM/yyyy"),
                Title = n.Title
            }).ToList();

            return View(posts);
        }
    }
}