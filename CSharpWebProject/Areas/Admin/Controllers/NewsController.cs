using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;
using Microsoft.AspNetCore.Authorization;
using CSharpWebProject.Models.ViewModels;

namespace CSharpWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/NewsPosts
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "News", new { area = "" });
        }

        // GET: Admin/NewsPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.Posts
                .Include(n => n.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // GET: Admin/NewsPosts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/NewsPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Date")] NewsPost newsPost)
        {
            if (ModelState.IsValid)
            {
                newsPost.AuthorId = this._context.Users.FirstOrDefault(u => u.UserName == "admin@admin.admin").Id;
                _context.Add(newsPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "News", new { area = "" });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", newsPost.AuthorId);
            return View(newsPost);
        }

        // GET: Admin/NewsPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.Posts.FindAsync(id);
            if (newsPost == null)
            {
                return NotFound();
            }
            NewsPostEditViewModel result = new NewsPostEditViewModel()
            {
                AuthorId = newsPost.AuthorId,
                Content = newsPost.Content,
                Date = newsPost.Date,
                Id = newsPost.Id,
                Title = newsPost.Title
            };

            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "UserName", newsPost.AuthorId);
            return View(result);
        }

        // POST: Admin/NewsPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,Content,Date")] NewsPostEditViewModel newsPost)
        {
            if (id != newsPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    NewsPost result = _context.Posts.FirstOrDefault(p => p.Id == newsPost.Id);
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsPostExists(newsPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "News", new { area = "" });
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "UserName", newsPost.AuthorId);
            return View(newsPost);
        }

        // GET: Admin/NewsPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.Posts
                .Include(n => n.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            NewsPostViewModel result = new NewsPostViewModel()
            {
                AuthorName = newsPost.Author.UserName,
                Content = newsPost.Content,
                Date = newsPost.Date.ToString("dd/MM/yyyy"),
                Id = newsPost.Id,
                Title = newsPost.Title,
            };

            return View(result);
        }

        // POST: Admin/NewsPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsPost = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(newsPost);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "News", new { area = "" });
        }

        private bool NewsPostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
