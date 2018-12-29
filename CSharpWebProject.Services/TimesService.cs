using CSharpWebProject.Data;
using CSharpWebProject.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Services
{
    public class TimesService : Service, ITimesService
    {
        public TimesService(ApplicationDbContext context) : base(context)
        {
        }

        public void AddTime(SolveTime time, string userId)
        {
            this.Context
                .Users
                .FirstOrDefault(u => u.Id == userId)
                .SolveTimes
                .Add(time);

            this.Context.SaveChanges();
        }

        public void AddTimes(List<SolveTime> times, string userId)
        {
            var userTimes = this.Context
               .Users
               .FirstOrDefault(u => u.Id == userId)
               .SolveTimes;

            foreach (var time in times)
            {
                userTimes.Add(time);
            }

            this.Context.SaveChanges();
        }

        public List<SolveTime> GetAllTimes(string username, string puzzleType)
        {
            //var user = this.Context.Users.FirstOrDefault(u => u.Id == username);

            //if(user != null)
            //{
            //    var times = this.Context
            //        .Users
            //        .FirstOrDefault(u => u.Id == user.Id)
            //        .SolveTimes
            //        .Where(s => s.PuzzleType == puzzleType)
            //        .ToList();


            //}
            return null;
        }
    }
}
