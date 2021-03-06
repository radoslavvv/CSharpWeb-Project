﻿using System;
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
        private readonly ICompetitionsService competitionsService;
        private readonly IUsersService usersService;
        private readonly IAchievementsService achievementsService;
        private readonly ITimesService timesService;

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

        public async Task<IActionResult> MyCompetitions(int page = 1)
        {
            User user = this.usersService.GetUserByUsername(this.User.Identity.Name);
            int pageSize = 5;

            List<CompetitionViewModel> competitions = user.Competitions.Select(c => new CompetitionViewModel()
            {
                Competitors = c.Competition.Competitors,
                Description = c.Competition.Description.Substring(0, 50) + "...",
                EndDate = c.Competition.EndDate.ToString("dd/MM/yyyy"),
                Id = c.Competition.Id,
                Name = c.Competition.Name,
                StartDate = c.Competition.StartDate.ToString("dd/MM/yyyy"),
                IsOpen = c.Competition.IsOpen
            }).ToList();

            PaginatedList<CompetitionViewModel> myCompetitions = await PaginatedList<CompetitionViewModel>.CreateAsync(competitions, page, pageSize);

            return View("MyCompetitionsList", myCompetitions);
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
            this.achievementsService.CheckForTimesAchievements(username);
            return true;
        }

        public IActionResult Timer(int id)
        {
            Competition competition = this.competitionsService.GetCompetitionById(id);
            if (competition == null)
            {
                return NotFound();
            }

            string userId = User.Identity.Name;
            if (competition.Competitors.Any(c => c.User.UserName == userId) && competition.IsOpen)
            {
                CompetitionViewModel competitionViewModel = new CompetitionViewModel()
                {
                    Name = competition.Name,
                    Description = competition.Description,
                    Competitors = competition.Competitors,
                    EndDate = competition.EndDate.ToString("dd/MM/yyyy"),
                    StartDate = competition.StartDate.ToString("dd/MM/yyyy"),
                    Id = competition.Id
                };

                return View(competitionViewModel);
            }

            return RedirectToAction("Index");
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
                    Description = c.Description.Substring(0, 50) + "...",
                    Competitors = c.Competitors,
                    EndDate = c.EndDate.ToString("dd/MM/yyyy"),
                    StartDate = c.StartDate.ToString("dd/MM/yyyy"),
                    Name = c.Name
                }).ToList();

            List<CompetitionViewModel> closedCompetitions = this.competitionsService.GetAllClosedCompetitions().Select(c => new CompetitionViewModel()
            {
                Id = c.Id,
                Description = c.Description.Substring(0, 50) + "...",
                Competitors = c.Competitors,
                EndDate = c.EndDate.ToString("dd/MM/yyyy"),
                StartDate = c.StartDate.ToString("dd/MM/yyyy"),
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
            if (competition == null)
            {
                return NotFound();
            }

            List<CompetitorViewModel> competititonComeptitors = competition
                .Competitors
                .Select(c => new CompetitorViewModel()
                {
                    Name = c.User.UserName,
                    BestTime = c.BestTime == null ? "N/A" : c.BestTime.Result.ToString("mm:ss:fff"),
                    BestTimeDate = c.BestTime == null ? "N/A" : c.BestTime.Date.ToString("dd/MM/yyyy")
                })
                .ToList();

            int pageSize = 5;
            int competitorsCount = this.competitionsService.GetCompetitionCompetitors(id).Count();
            bool userIsInCompetition = competition.Competitors.Any(c => c.User.UserName == User.Identity.Name);
            bool userHasBestTime = false;
            if (userIsInCompetition)
            {
                userHasBestTime = competition.Competitors.FirstOrDefault(c => c.User.UserName == User.Identity.Name).BestTime == null;
            }

            ViewBag.CompetitorsCount = competitorsCount;
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
                    IsOpen = competition.IsOpen,
                    UserIsInCompetition = userIsInCompetition,
                    CompetitorHasBestTime = userHasBestTime

                };

                return View("OpenCompetitionDetails", result);
            }
            else
            {
                List<CompetitorViewModel> sortedCompetitors = competititonComeptitors.OrderBy(c => DateTime.ParseExact(c.BestTime, "mm:ss:fff", CultureInfo.InvariantCulture).TimeOfDay).ToList();
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