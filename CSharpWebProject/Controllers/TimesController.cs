using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;
using CSharpWebProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSharpWebProject.Controllers
{
    [Authorize]
    public class TimesController : Controller
    {
        private readonly ITimesService timesService;
        private readonly IUsersService usersService;
        private readonly IAchievementsService achievementsService;

        public TimesController(ITimesService timesService, IUsersService usersService, IAchievementsService achievementsService)
        {
            this.timesService = timesService;
            this.usersService = usersService;
            this.achievementsService = achievementsService;
        }

        [HttpPost]
        public IActionResult AddTimes(string times, string timeType = "Practice")
        {
            string[] result = JsonConvert.DeserializeObject<string[]>(times);

            string username = this.User.Identity.Name;
            string userId = this.usersService.GetUserIdByUsername(username);

            List<SolveTime> solveTimes = result.Select(t => new SolveTime()
            {
                Result = DateTime.ParseExact(t, "mm:ss:fff", CultureInfo.InvariantCulture),
                UserId = userId,
                Date = DateTime.Now,
                Type = timeType
            }).ToList();

            this.timesService.AddTimes(solveTimes, userId);
            this.achievementsService.CheckForTimesAchievements(username);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All(int page = 1)
        {
            string username = this.User.Identity.Name;
            string userId = this.usersService.GetUserIdByUsername(username);
            List<ListSolveTime> solveTimes = this.timesService
                .GetAllUserTimes(username)
                .OrderByDescending(t => t.Date)
                 .Select(t => new ListSolveTime()
                 {
                     Time = t.Result.ToString("mm:ss:fff"),
                     Date = t.Date.ToString("dd/MM/yyyy"),
                     TimeType = t.Type
                 }).ToList();


            int pageSize = 10;
            return View("MySolveTimesList", await PaginatedList<ListSolveTime>.CreateAsync(solveTimes, page, pageSize));
        }

        [HttpGet]
        public IActionResult Best()
        {
            string username = this.User.Identity.Name;
            string userId = this.usersService.GetUserIdByUsername(username);
            List<SolveTime> solveTimes = this.timesService.GetAllUserTimes(username);

            //string bestTime = solveTimes
            //    .Select(s => DateTime.ParseExact(s.Result, "mm:ss:fff", CultureInfo.InvariantCulture)).Min()
            //    .ToString("mm:ss:fff");

            ListSolveTimeCollection result = new ListSolveTimeCollection()
            {
                ListSolveTimes = solveTimes
                        .Select(t => new ListSolveTime()
                        {
                            Time = t.Result.ToString("mm:ss:fff"),
                            Date = t.Date.ToString("dd/MM/yyyy")
                        }).ToList(),
            };

            return View("MySolveTimesList", result);
        }
        // GET: Times/Create
        public ActionResult Create()
        {
            return View();
        }

    }
}