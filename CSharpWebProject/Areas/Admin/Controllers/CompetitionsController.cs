using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using Microsoft.AspNetCore.Authorization;
using CSharpWebProject.Models.ViewModels;
using CSharpWebProject.Models;

namespace CSharpWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CompetitionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICompetitionsService competitionsService;
        private readonly IUsersService usersService;
        public CompetitionsController(ApplicationDbContext context, ICompetitionsService competitionsService, IUsersService usersService)
        {
            _context = context;
            this.competitionsService = competitionsService;
            this.usersService = usersService;
        }

        // GET: Admin/Competitions
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 15;
            var competitions = _context.Competitions.Select(c => new CompetitionViewModel()
            {
                StartDate = c.StartDate.ToString("dd/MM/yyyy"),
                Id = c.Id,
                Competitors = c.Competitors,
                Description = c.Description.Substring(0, 20) + "...",
                EndDate = c.EndDate.ToString("dd/MM/yyyy"),
                IsOpen = c.IsOpen,
                Name = c.Name,
                Sponsor = c.Sponsor
            }).ToList();

            PaginatedList<CompetitionViewModel> competitorsPage = await PaginatedList<CompetitionViewModel>.CreateAsync(competitions, page, pageSize);

            return View(competitorsPage);
        }

        // GET: Admin/Competitors
        public async Task<IActionResult> Competitors(int id, int page = 1)
        {
            int pageSize = 15;
            var competitors = _context.Competitions.FirstOrDefault(c => c.Id == id).Competitors.Select(c => new CompetitorViewModel()
            {
                Id = c.Id,
                BestTime = c.BestTime == null ? "N/A" : c.BestTime.Result.ToString("mm:ss:fff"),
                BestTimeDate = c.BestTime == null ? "N/A" : c.BestTime.Date.ToString("dd/MM/yyyy"),
                Name = c.User.UserName,
                CompetitionId = id
            }).ToList();

            PaginatedList<CompetitorViewModel> competitorsPage = await PaginatedList<CompetitorViewModel>.CreateAsync(competitors, page, pageSize);

            return View(competitorsPage);
        }

        public IActionResult RemoveUser(int competitionId, int competitorId)
        {
            User competitor = this.competitionsService
                .GetCompetitionById(competitionId)
                .Competitors
                .FirstOrDefault(c => c.Id == competitorId)
                .User;

            this.competitionsService.RemoveUser(competitionId, competitor);

            return RedirectToAction("Index");
        }

        // GET: Admin/Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // GET: Admin/Competitions/Create
        public IActionResult Create()
        {
            return View();
        }


        public IActionResult Close(int id)
        {
            this.competitionsService.CloseCompetition(id);

            return RedirectToAction("Index");
        }

        // POST: Admin/Competitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Sponsor,IsOpen")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        // GET: Admin/Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }
            return View(competition);
        }

        // POST: Admin/Competitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Sponsor,IsOpen")] Competition competition)
        {
            if (id != competition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        // GET: Admin/Competitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // POST: Admin/Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);
            _context.Competitions.Remove(competition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionExists(int id)
        {
            return _context.Competitions.Any(e => e.Id == id);
        }
    }
}
