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
using Microsoft.AspNetCore.Authorization;
using CSharpWebProject.Models.ViewModels;
using System.Globalization;

namespace CSharpWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class SolveTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolveTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SolveTimes
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            var solveTimes = _context.SolveTimes.Include(s => s.User).Select(s => new SolveTimeViewModel()
            {

                Date = s.Date.ToString("dd/MM/yyyy"),
                Id = s.Id,
                Result = s.Result.ToString("mm:ss:fff"),
                Type = s.Type,
                Username = s.User.UserName
            })
            .ToList();

            PaginatedList<SolveTimeViewModel> competitorsPage = await PaginatedList<SolveTimeViewModel>.CreateAsync(solveTimes, page, pageSize);

            return View(competitorsPage);
        }

        // GET: Admin/SolveTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solveTime = await _context.SolveTimes
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solveTime == null)
            {
                return NotFound();
            }

            SolveTimeViewModel result = new SolveTimeViewModel()
            {
                Date = solveTime.Date.ToString("dd/MM/yyyy"),
                Id = solveTime.Id,
                Result = solveTime.Result.ToString("mm:ss:fff"),
                Type = solveTime.Type,
                Username = solveTime.User.UserName
            };

            return View(result);
        }

        // GET: Admin/SolveTimes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Admin/SolveTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Result,Type,UserId,Date")] SolveTime solveTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solveTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", solveTime.UserId);
            return View(solveTime);
        }

        // GET: Admin/SolveTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solveTime = await _context.SolveTimes.FindAsync(id);
            if (solveTime == null)
            {
                return NotFound();
            }

            SolveTimeCreateViewModel result = new SolveTimeCreateViewModel()
            {
                Date = solveTime.Date,
                UserId = solveTime.UserId,
                Type = solveTime.Type,
                Result = solveTime.Result.ToString("mm:ss:fff"),
                Id = solveTime.Id
            };

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", solveTime.UserId);
            return View(result);
        }

        // POST: Admin/SolveTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Result,Type,UserId,Date")] SolveTimeCreateViewModel solveTime)
        {
            if (id != solveTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SolveTime result = new SolveTime()
                    {
                        Date = solveTime.Date,
                        Id = solveTime.Id,
                        Result = DateTime.ParseExact(solveTime.Result, "mm:ss:fff", CultureInfo.InvariantCulture),
                        Type = solveTime.Type,
                        UserId = solveTime.UserId
                    };

                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolveTimeExists(solveTime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "SolveTimes");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", solveTime.UserId);
            return View(solveTime);
        }

        // GET: Admin/SolveTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solveTime = await _context.SolveTimes
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solveTime == null)
            {
                return NotFound();
            }

            SolveTimeViewModel result = new SolveTimeViewModel()
            {
                Date = solveTime.Date.ToString("dd/MM/yyyy"),
                Id = solveTime.Id,
                Result = solveTime.Result.ToString("mm:ss:fff"),
                Type = solveTime.Type,
                Username = solveTime.User.UserName
            };

            return View(result);
        }

        // POST: Admin/SolveTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solveTime = await _context.SolveTimes.FindAsync(id);
            _context.SolveTimes.Remove(solveTime);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "SolveTimes");
        }

        private bool SolveTimeExists(int id)
        {
            return _context.SolveTimes.Any(e => e.Id == id);
        }
    }
}
