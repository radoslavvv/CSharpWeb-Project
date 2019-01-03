using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpWebProject.Tests.Services
{
    public class AchievementsServiceTests
    {
        private ApplicationDbContext dbContext;
        private AchievementsService achievementsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            this.dbContext = new ApplicationDbContext(options);
            this.achievementsService = new AchievementsService(dbContext);

            User user = new User()
            {
                Id = "id",
                UserName = "username"
            };

            Achievement achievement = new Achievement()
            {
                Id = 1,
                Name = "Participate in 1 Competition"
            };

            this.dbContext.Achievements.Add(achievement);
            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();
        }

        [Test]
        public void AddAchievementActuallyAddsAchievement()
        {
            string achievementName = "Participate in 1 Competition";
            string username = "username";

            this.achievementsService.AddAchievement(username, achievementName);

            User user = this.dbContext
                                .Users
                                .FirstOrDefault(u => u.UserName == username);

            int userAchievementsCount = user.Achievements.Count;
            Assert.AreEqual(1, userAchievementsCount);
        }

        [Test]
        public void AddAchievementReturnsFalseWhenAhievementIsInvalid()
        {
            string achievementName = "Participate in 1337 Competition";
            string username = "username";

            bool result = this.achievementsService.AddAchievement(username, achievementName);

            Assert.AreEqual(false, result);
        }


        [Test]
        public void AddAchievementReturnsFalseWhenUserIsInvalid()
        {
            string achievementName = "Participate in 1 Competition";
            string username = "usernameaaaa";

            bool result = this.achievementsService.AddAchievement(username, achievementName);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void GetAllAchievementsRetursCorrectResult()
        {
            List<UserAchievement> achievements = new List<UserAchievement>()
            {
                  new UserAchievement()
                    {
                        AchievementId = 1,
                        UserId = "id"
                    }
            };


            this.dbContext.Users.FirstOrDefault(u => u.Id == "id").Achievements = achievements;
            this.dbContext.SaveChanges();
        }

    }
}