using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpWebProject.Tests.Services
{
    public class TimesServiceTests
    {
        [Test]
        public void GetAllUserTimesResutsRightResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);
            string username = "Username";
            List<SolveTime> solveTimes = new List<SolveTime>()
                {
                    new SolveTime()
                    {
                        Id = 1
                    }
                };

            User user = new User()
            {
                UserName = username,
                SolveTimes = solveTimes
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            TimesService timesService = new TimesService(dbContext);

            List<SolveTime> resultSolveTimes = timesService.GetAllUserTimes(username);

            Assert.AreEqual(solveTimes, resultSolveTimes);
        }

        [Test]
        public void GetAllTimesReturnsRightResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            List<SolveTime> solveTimes = new List<SolveTime>()
            {
                new SolveTime()
                {
                    Id = 1
                },
                new SolveTime()
                {
                    Id = 2
                },
            };

            dbContext.SolveTimes.AddRange(solveTimes);
            dbContext.SaveChanges();

            TimesService timesService = new TimesService(dbContext);

            List<SolveTime> resultSolveTimes = timesService.GetAllTimes();

            Assert.AreEqual(solveTimes, resultSolveTimes);
        }

        [Test]
        public void AddTimesReturnsRightResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);

            string userId = "id";
            List<SolveTime> solveTimes = new List<SolveTime>()
            {
                new SolveTime()
                {
                    Id = 1
                },
                new SolveTime()
                {
                    Id = 2
                },
            };

            User user = new User()
            {
                Id = userId,
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            timesService.AddTimes(solveTimes, userId);
            var userSolveTimesCount = dbContext
                .Users
                .FirstOrDefault(u => u.Id == userId)
                .SolveTimes
                .Count;

            Assert.AreEqual(solveTimes.Count, userSolveTimesCount);
        }

        [Test]
        public void AddTimesReturnsFalseWhenUserDoesntExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);

            string userId = "id";
            List<SolveTime> solveTimes = new List<SolveTime>()
            {
                new SolveTime()
                {
                    Id = 1
                },
                new SolveTime()
                {
                    Id = 2
                },
            };

            User user = new User()
            {
                Id = userId,
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            bool result = timesService.AddTimes(solveTimes, "RandomId");

            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddTimesReturnsTrueWhenTimesAreAdded()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);

            string userId = "id";
            List<SolveTime> solveTimes = new List<SolveTime>()
            {
                new SolveTime()
                {
                    Id = 1
                },
                new SolveTime()
                {
                    Id = 2
                },
            };

            User user = new User()
            {
                Id = userId,
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            bool result = timesService.AddTimes(solveTimes, userId);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void AddTimeAddsTimeCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);
            string userId = "id";
            SolveTime solveTime = new SolveTime()
            {
                Id = 1
            };

            User user = new User()
            {
                Id = userId
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            timesService.AddTime(solveTime, userId);

            int userSolveTimesCount = dbContext.Users.FirstOrDefault(u => u.Id == userId).SolveTimes.Count;
            Assert.AreEqual(1, userSolveTimesCount);
        }

        [Test]
        public void AddTimeReturnsFalseWhenUserDoesntExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);
            string userId = "id";
            SolveTime solveTime = new SolveTime()
            {
                Id = 1
            };

            User user = new User()
            {
                Id = userId
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            bool result = timesService.AddTime(solveTime, "randomId");

            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddTimeReturnsFalseWhenSolveTimeIsInvalid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);
            string userId = "id";
            SolveTime solveTime = null;

            User user = new User()
            {
                Id = userId
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            bool result = timesService.AddTime(solveTime, userId);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddTimeReturnsTrueWhenTimeIsAddedCorrectlly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            ApplicationDbContext dbContext = new ApplicationDbContext(options);

            TimesService timesService = new TimesService(dbContext);
            string userId = "id";
            SolveTime solveTime = new SolveTime() {  };

            User user = new User()
            {
                Id = userId
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            bool result = timesService.AddTime(solveTime, userId);

            Assert.AreEqual(true, result);
        }
    }
}

//public bool AddTime(SolveTime time, string userId)
//{
//    User user = this.Context
//        .Users
//        .FirstOrDefault(u => u.Id == userId);

//    if (user == null || time == null)
//    {
//        return false;
//    }

//    user.SolveTimes
//        .Add(time);

//    this.Context.SaveChanges();
//    return true;
//}
//public bool AddTimes(List<SolveTime> times, string userId)
//{
//    User user = this.Context
//       .Users
//       .FirstOrDefault(u => u.Id == userId);

//    if (user == null)
//    {
//        return false;
//    }

//    List<SolveTime> userTimes = user
//       .SolveTimes
//       .ToList();

//    foreach (var time in times)
//    {
//        if (time != null)
//        {
//            userTimes.Add(time);
//        }
//    }

//    this.Context.SaveChanges();
//    return true;
//}