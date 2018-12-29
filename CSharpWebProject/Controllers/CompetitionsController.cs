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
    public class CompetitionsController : Controller
    {
        private ICompetitionsService competitionsService;
        private IUsersService usersService;
        public CompetitionsController(ICompetitionsService competitionsService, IUsersService usersService)
        {
            this.competitionsService = competitionsService;
            this.usersService = usersService;
        }

        public IActionResult Join(int id)
        {
            string username = this.User.Identity.Name;
            User user = this.usersService.GetUserByUsername(username);

            this.competitionsService.JoinUser(id, user);

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
                    CompetitorsCount = c.Competitors.Count,
                    EndDate = c.EndDate,
                    StartDate = c.StartDate,
                    Name = c.Name
                }).ToList();

            List<CompetitionViewModel> closedCompetitions = this.competitionsService.GetAllClosedCompetitions().Select(c => new CompetitionViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                CompetitorsCount = c.Competitors.Count,
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

            List<CompetitorViewModel> competititonComeptitors = competition.Competitors.Select(c => new CompetitorViewModel()
            {
                Name = c.User.UserName,
                BestTime = c.BestTime == null ? "N/A" : c.BestTime.Result.ToString("mm:ss:fff"),
                BestTimeDate = c.BestTime == null ? "N/A" : c.BestTime.Date.ToString("dd/MM/yyyy")
            }).ToList();

            int pageSize = 10;
            PaginatedList<CompetitorViewModel> competitorsPage = await PaginatedList<CompetitorViewModel>.CreateAsync(competititonComeptitors, page, pageSize);

            CompetitionDetailsViewModel result = new CompetitionDetailsViewModel()
            {
                Competitors = competitorsPage,
                Description = competition.Description,
                EndDate = competition.EndDate,
                Id = competition.Id,
                Name = competition.Name,
                Sponsor = competition.Sponsor,
                StartDate = competition.StartDate,
            };
            return View(result);
        }
    }
}