using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpWebProject.Data;
using CSharpWebProject.Models;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;

namespace CSharpWebProject.Services
{
    public class AchievementsService : Service, IAchievementsService
    {
        public AchievementsService(ApplicationDbContext context) : base(context)
        {
        }

        public bool AddAchievement(string username, string achievementName)
        {
            Achievement achievement = this.Context.Achievements.FirstOrDefault(a => a.Name == achievementName);
            User user = this.Context.Users.FirstOrDefault(u => u.UserName == username);

            if(user == null || achievement == null)
            {
                return false;
            }

            UserAchievement ua = new UserAchievement()
            {
                UserId = user.Id,
                AchievementId = achievement.Id
            };

            if (user.Achievements.All(a => a.AchievementId != ua.AchievementId))
            {
                user.Achievements.Add(ua);
            }

            this.Context.SaveChanges();
            return true;
        }

        public bool CheckForCompetitionAchievements(string username)
        {
            User user = this.Context
                .Users
                .FirstOrDefault(u => u.UserName == username);
            if(user == null)
            {
                return false;
            }

            List<UserAchievement> userCompetitionAchievements = user.Achievements
                .Where(a => a.Achievement.Category == "Competition")
                .ToList();

            int competitionsCount = user.Competitions.Count;
            if (competitionsCount == 50)
            {
                this.AddAchievement(username, "Participate in 50 Competition");
            }
            else if (competitionsCount == 20)
            {
                this.AddAchievement(username, "Participate in 20 Competition");
            }
            else if (competitionsCount == 15)
            {
                this.AddAchievement(username, "Participate in 15 Competitions");
            }
            else if (competitionsCount == 10)
            {
                this.AddAchievement(username, "Participate in 10 Competitions");
            }
            else if (competitionsCount == 5)
            {
                this.AddAchievement(username, "Participate in 5 Competitions");
            }
            else if (competitionsCount == 1)
            {
                this.AddAchievement(username, "Participate in 1 Competition");
            }

            return true;
        }

        public bool CheckForTimesAchievements(string username)
        {
            User user = this.Context
                .Users
                .FirstOrDefault(u => u.UserName == username);

            int bestTimeInSeconds = user
                .SolveTimes
                .OrderBy(s => s.Result)
                .FirstOrDefault()
                .Result
                .Second;

            bool gotAchievement = false;
            if (bestTimeInSeconds < 5)
            {
                this.AddAchievement(username, "Achieve Time Under 5 Seconds");
            }
            if (bestTimeInSeconds < 10)
            {
                this.AddAchievement(username, "Achieve Time Under 10 Seconds");
            }
            if (bestTimeInSeconds < 15)
            {
                this.AddAchievement(username, "Achieve Time Under 15 Seconds");
            }
            if (bestTimeInSeconds < 20)
            {
                this.AddAchievement(username, "Achieve Time Under 20 Seconds");
                
            }
            if (bestTimeInSeconds < 30)
            {
                this.AddAchievement(username, "Achieve Time Under 30 Seconds");
            }
            //if (bestTimeInSeconds < 30)
            //{
            //    this.AddAchievement(username, "Achieve Time Under 30 Seconds");
            //    gotAchievement = true;
            //}
            //if (bestTimeInSeconds < 20)
            //{
            //    this.AddAchievement(username, "Achieve Time Under 20 Seconds");
            //    gotAchievement = true;
            //}
            //if (bestTimeInSeconds < 10)
            //{
            //    this.AddAchievement(username, "Achieve Time Under 10 Seconds");
            //    gotAchievement = true;
            //}

            return gotAchievement;
        }

        public void SeedDabase()
        {
            var achievements = new List<Achievement>()
            {
                new Achievement()
                {
                    Category = "Time",
                    Description = "Achieve Time Under 5 Seconds ",
                    Name= "Achieve Time Under 5 Seconds",
                },
                 new Achievement()
                {
                    Category = "Time",
                    Description = "Achieve Time Under 10 Seconds",
                    Name= "Achieve Time Under 10 Seconds",
                },
                 new Achievement()
                {
                    Category = "Time",
                    Description = "Achieve Time Under 15 Seconds",
                    Name= "Achieve Time Under 15 Seconds",
                },
                 new Achievement()
                {
                    Category = "Time",
                    Description = "Achieve Time Under 20 Seconds",
                    Name= "Achieve Time Under 20 Seconds",
                },
                 new Achievement()
                {
                    Category = "Time",
                    Description = "Achieve Time Under 30 Seconds",
                    Name= "Achieve Time Under 30 Seconds",
                },
                 new Achievement()
                {
                    Category = "Competitions",
                    Description = "Participate in 1 Competition",
                    Name= "Participate in 1 Competition",
                },
                 new Achievement()
                {
                    Category = "Competitions",
                    Description = "Participate in 5 Competitions",
                    Name= "Participate in 5 Competitions",
                },
                 new Achievement()
                {
                    Category = "Competitions",
                    Description = "Participate in 10 Competitions",
                    Name= "Participate in 10 Competitions",
                },
                 new Achievement()
                {
                    Category = "Competitions",
                    Description = "Participate in 15 Competitions",
                    Name= "Participate in 15 Competitions",
                },
                 new Achievement()
                {
                    Category = "Competitions",
                    Description = "Participate in 20 Competitions",
                    Name= "Participate in 20 Competitions",
                },
                   new Achievement()
                {
                    Category = "Competitions",
                    Description = "Participate in 50 Competitions",
                    Name= "Participate in 50 Competitions",
                },
            };

            this.Context.Achievements.AddRange(achievements);
            this.Context.SaveChanges();

        }
        public MyAchievementsViewModel GetAllAchievements(string username)
        {
            User user = this.Context.Users.FirstOrDefault(u => u.UserName == username);
            if(user == null)
            {
                return new MyAchievementsViewModel();
            }

            List<AchievementViewModel> timesAchivements = this
                .Context
                .Achievements
                .Where(c => c.Category == "Time").Select(a => new AchievementViewModel()
                {
                    Category = a.Category,
                    Description = a.Description,
                    Id = a.Id,
                    Name = a.Name,
                }).ToList();

            List<AchievementViewModel> competitionAchivements = this
               .Context
               .Achievements
               .Where(c => c.Category == "Competition").Select(a => new AchievementViewModel()
               {
                   Category = a.Category,
                   Description = a.Description,
                   Id = a.Id,
                   Name = a.Name,
               }).ToList();

            List<AchievementViewModel> usersAchievements = user.Achievements.Select(a => new AchievementViewModel()
            {
                Category = a.Achievement.Category,
                Description = a.Achievement.Description,
                Id = a.Achievement.Id,
                Name = a.Achievement.Name,
            }).ToList();

            MyAchievementsViewModel result = new MyAchievementsViewModel()
            {
                CompetitionAchivements = competitionAchivements,
                TimesAchivements = timesAchivements,
                UserAchievements = usersAchievements
            };

            return result;
        }
    }
}
