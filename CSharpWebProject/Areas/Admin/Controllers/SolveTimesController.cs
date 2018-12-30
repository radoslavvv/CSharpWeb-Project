﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;

namespace CSharpWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SolveTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolveTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SolveTimes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SolveTimes.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
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

            return View(solveTime);
        }

        // GET: Admin/SolveTimes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id");
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
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id", solveTime.UserId);
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
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id", solveTime.UserId);
            return View(solveTime);
        }

        // POST: Admin/SolveTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Result,Type,UserId,Date")] SolveTime solveTime)
        {
            if (id != solveTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solveTime);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.RubikUsers, "Id", "Id", solveTime.UserId);
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

            return View(solveTime);
        }

        // POST: Admin/SolveTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solveTime = await _context.SolveTimes.FindAsync(id);
            _context.SolveTimes.Remove(solveTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolveTimeExists(int id)
        {
            return _context.SolveTimes.Any(e => e.Id == id);
        }
    }
}