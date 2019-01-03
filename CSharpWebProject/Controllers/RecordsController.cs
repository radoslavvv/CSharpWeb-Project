using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;
using CSharpWebProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebProject.Controllers
{
    public class RecordsController : Controller
    {
        private readonly ITimesService timesService;
        public RecordsController(ITimesService timesService)
        {
            this.timesService = timesService;
        }

        public IActionResult Index()
        {
            List<RecordSolveTimeViewModel> records = this.timesService
                .GetAllTimes()
                .Where(t => t.Type != "Practice")
                .OrderBy(r => r.Result.TimeOfDay)
                .Take(11)
                .Select(s => new RecordSolveTimeViewModel()
                {
                    Date = s.Date.ToString("dd/MM/yyyy"),
                    Result = s.Result.ToString("mm:ss.fff"),
                    Type = s.Type,
                    Username = s.User.UserName,
                    Id = s.Id
                })
                .ToList();

            return View(records);
        }
    }
}