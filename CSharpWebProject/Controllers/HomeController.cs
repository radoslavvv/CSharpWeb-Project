using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;

namespace CSharpWebProject.Controllers
{
    public class HomeController : Controller
    {
        private IAchievementsService achievementsService;
        public HomeController(IAchievementsService achievementsService)
        {
            this.achievementsService = achievementsService;
        }

        public IActionResult Index()
        {
            //this.achievementsService.SeedDabase();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
