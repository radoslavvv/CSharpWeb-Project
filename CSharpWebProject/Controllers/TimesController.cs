using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSharpWebProject.Controllers
{
    public class TimesController : Controller
    {
        private readonly ITimesService timesService;
        private readonly IUsersService usersService;

        public TimesController(ITimesService timesService, IUsersService usersService)
        {
            this.timesService = timesService;
            this.usersService = usersService;
        }

        // GET: Times
        [HttpPost]
        public IActionResult Add(string times, string puzzleType, string username)
        {
            string[] result = JsonConvert.DeserializeObject<string[]>(times);

            string userId = this.usersService.GetUserIdByUsername(username);

            List<SolveTime> solveTimes = result.Select(t => new SolveTime()
            {
                Result = t,
                PuzzleType = puzzleType,
                UserId = userId,
            }).ToList();

            this.timesService.AddTimes(solveTimes, userId);

            return View();
        }

        // GET: Times/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Times/Create
        public ActionResult Create()
        {
            return View();
        }

    }
}