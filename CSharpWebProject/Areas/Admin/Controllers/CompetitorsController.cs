using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models;
using CSharpWebProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CSharpWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CompetitorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Competitors
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 15;
            var competitors = _context.Competitors.Include(c => c.Competition).Include(c => c.User).ToList();

            PaginatedList<Competitor> result = await PaginatedList<Competitor>.CreateAsync(competitors, page, pageSize);

            return View(result);
        }

        // GET: Admin/Competitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitor = await _context.Competitors
                .Include(c => c.Competition)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitor == null)
            {
                return NotFound();
            }

            return View(competitor);
        }

        // GET: Admin/Competitors/Create
        public IActionResult Create()
        {
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id");
            return View();
        }

        // POST: Admin/Competitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CompetitionId,SubmittedTimes")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitor.CompetitionId);
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id", competitor.UserId);
            return View(competitor);
        }

        // GET: Admin/Competitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitor = await _context.Competitors.FindAsync(id);
            if (competitor == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitor.CompetitionId);
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id", competitor.UserId);
            return View(competitor);
        }

        // POST: Admin/Competitors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CompetitionId,SubmittedTimes")] Competitor competitor)
        {
            if (id != competitor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitorExists(competitor.Id))
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
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitor.CompetitionId);
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id", competitor.UserId);
            return View(competitor);
        }

        // GET: Admin/Competitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitor = await _context.Competitors
                .Include(c => c.Competition)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitor == null)
            {
                return NotFound();
            }

            return View(competitor);
        }

        // POST: Admin/Competitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competitor = await _context.Competitors.FindAsync(id);
            _context.Competitors.Remove(competitor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitorExists(int id)
        {
            return _context.Competitors.Any(e => e.Id == id);
        }
    }
}
