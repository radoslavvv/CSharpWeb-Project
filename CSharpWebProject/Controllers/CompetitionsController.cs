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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSharpWebProject.Controllers
{
    [Authorize]
    public class CompetitionsController : Controller
    {
        private ICompetitionsService competitionsService;
        private IUsersService usersService;
        private IAchievementsService achievementsService;
        private ITimesService timesService;
        public CompetitionsController(ICompetitionsService competitionsService, IUsersService usersService, IAchievementsService achievementsService, ITimesService timesService)
        {
            this.competitionsService = competitionsService;
            this.usersService = usersService;
            this.achievementsService = achievementsService;
            this.timesService = timesService;
        }

        public IActionResult Join(int id)
        {
            string username = this.User.Identity.Name;
            User user = this.usersService.GetUserByUsername(username);

            this.competitionsService.JoinUser(id, user);
            this.achievementsService.CheckForCompetitionAchievements(username);

            return RedirectToAction("Timer", new { id = id });
        }

        public IActionResult ListMyCompetitions()
        {
            User user = this.usersService.GetUserByUsername(this.User.Identity.Name);

            List<CompetitionViewModel> competitions = user.Competitions.Select(c => new CompetitionViewModel()
            {
                Competitors = c.Competitors,
                Description = c.Description,
                EndDate = c.EndDate,
                Id = c.Id,
                Name = c.Name,
                StartDate = c.StartDate,
                IsOpen = c.IsOpen
            }).ToList();

            return View("MyCompetitionsList", competitions);
        }

        public bool AddTimes(string times, string timeType)
        {
            string[] result = JsonConvert.DeserializeObject<string[]>(times);

            string username = this.User.Identity.Name;
            string userId = this.usersService.GetUserIdByUsername(username);

            List<CompetiveSolveTime> competitionTimes = result.Select(t => new CompetiveSolveTime()
            {
                Result = DateTime.ParseExact(t, "mm:ss:fff", CultureInfo.InvariantCulture),
                UserId = userId,
                Date = DateTime.Now,
                Type = timeType
            }).ToList();

            Competition competition = this.competitionsService.GetCompetitionByName(timeType);
            this.competitionsService.AddTimes(competitionTimes, userId, competition.Id);

            List<SolveTime> solveTimes = competitionTimes.Select(s => new SolveTime()
            {
                Date = s.Date,
                Result = s.Result,
                Type = s.Type,
                UserId = s.UserId
            }).ToList();

            this.timesService.AddTimes(solveTimes, userId);
            return true;
        }

        public IActionResult Timer(int id)
        {
            Competition competition = this.competitionsService.GetCompetitionById(id);

            CompetitionViewModel competitionViewModel = new CompetitionViewModel()
            {
                Name = competition.Name,
                Description = competition.Description,
                Competitors = competition.Competitors,
                EndDate = competition.EndDate,
                StartDate = competition.StartDate,
                Id = competition.Id
            };

            return View(competitionViewModel);
        }

        public IActionResult Leave(int id)
        {
            string username = this.User.Identity.Name;
            User user = this.usersService.GetUserByUsername(username);

            this.competitionsService.RemoveUser(id, user);
            Competition competition = this.competitionsService.GetCompetitionById(id);
            
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,StartDate,EndDate,Sponsor,IsOpen")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                this.competitionsService.CreateCompetition(competition);
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        public IActionResult Index()
        {
            List<CompetitionViewModel> openCompetitions = this.competitionsService.GetAllOpenCompetitions()
                .Select(c => new CompetitionViewModel()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Competitors = c.Competitors,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate,
                    Name = c.Name
                }).ToList();

            List<CompetitionViewModel> closedCompetitions = this.competitionsService.GetAllClosedCompetitions().Select(c => new CompetitionViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                Competitors = c.Competitors,
                EndDate = c.EndDate,
                StartDate = c.StartDate,
                Name = c.Name
            }).ToList();

            CompetitionsIndexViewModel result = new CompetitionsIndexViewModel()
            {
                OpenCompetitions = openCompetitions,
                ClosedCompetitions = closedCompetitions
            };

            return View(result);
        }


        public async Task<IActionResult> Details(int id, int page = 1)
        {
            Competition competition = this.competitionsService.GetCompetitionById(id);

            List<CompetitorViewModel> competititonComeptitors = competition
                .Competitors
                .Select(c => new CompetitorViewModel()
                {
                    Name = c.User.UserName,
                    BestTime = c.BestTime == null ? "N/A" : c.BestTime.Result.ToString("mm:ss:fff"),
                    BestTimeDate = c.BestTime == null ? "N/A" : c.BestTime.Date.ToString("dd/MM/yyyy")
                })
                .ToList();

            int pageSize = 3;

            CompetitionDetailsViewModel result;
            if (competition.IsOpen)
            {
                PaginatedList<CompetitorViewModel> competitorsPage = await PaginatedList<CompetitorViewModel>.CreateAsync(competititonComeptitors, page, pageSize);

                result = new CompetitionDetailsViewModel()
                {
                    Competitors = competitorsPage,
                    Description = competition.Description,
                    EndDate = competition.EndDate,
                    Id = competition.Id,
                    Name = competition.Name,
                    Sponsor = competition.Sponsor,
                    StartDate = competition.StartDate,
                    IsOpen = competition.IsOpen
                };

                return View("OpenCompetitionDetails", result);
            }
            else
            {
                List<CompetitorViewModel> sortedCompetitors = competititonComeptitors.OrderBy(c => c.BestTime).ToList();
                List<CompetitorViewModel> winners = sortedCompetitors.Take(3).ToList();

                PaginatedList<CompetitorViewModel> competitorsPage = await PaginatedList<CompetitorViewModel>.CreateAsync(sortedCompetitors.ToList(), page, pageSize);

                result = new CompetitionDetailsViewModel()
                {
                    Competitors = competitorsPage,
                    Description = competition.Description,
                    EndDate = competition.EndDate,
                    Id = competition.Id,
                    Name = competition.Name,
                    Sponsor = competition.Sponsor,
                    StartDate = competition.StartDate,
                    IsOpen = competition.IsOpen,
                    Winners = winners
                };

                ViewBag.Place = ((page - 1) * pageSize) + 1;

                return View("ClosedCompetitionDetails", result);
            }
        }
    }
}