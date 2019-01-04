using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
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

        public bool AddTime(SolveTime time, string userId)
        {
            User user = this.Context
                .Users
                .FirstOrDefault(u => u.Id == userId);

            if(user == null || time == null)
            {
                return false;
            }

            user.SolveTimes
                .Add(time);

            this.Context.SaveChanges();
            return true;
        }

        public bool AddTimes(List<SolveTime> times, string userId)
        {
            User user = this.Context
               .Users
               .FirstOrDefault(u => u.Id == userId);

            if(user == null)
            {
                return false;
            }

            var userTimes = user
               .SolveTimes;
               //TODO?

            foreach (var time in times)
            {
                if(time != null)
                {
                    userTimes.Add(time);
                }
            }

            this.Context.SaveChanges();
            return true;
        }

        public List<SolveTime> GetAllTimes()
        {
            List<SolveTime> times = this
                .Context
                .SolveTimes
                .ToList();

            return times;
        }

        public List<SolveTime> GetAllUserTimes(string username)
        {
            User user = this.Context
                .Users
                .FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                var times = this.Context
                    .Users
                    .Include(u=>u.SolveTimes)
                    .FirstOrDefault(u => u.Id == user.Id)
                    .SolveTimes
                    .ToList();

                return times;
            }

            return new List<SolveTime>();
        }
    }
}
