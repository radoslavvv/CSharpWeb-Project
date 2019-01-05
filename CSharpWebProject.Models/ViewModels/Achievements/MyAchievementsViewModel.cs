using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class MyAchievementsViewModel
    {
        public MyAchievementsViewModel()
        {
            this.CompetitionAchivements = new List<AchievementViewModel>();
            this.TimesAchivements = new List<AchievementViewModel>();
            this.UserAchievements = new List<AchievementViewModel>();
        }
        public List<AchievementViewModel> CompetitionAchivements { get; set; }

        public List<AchievementViewModel> TimesAchivements { get; set; }

        public List<AchievementViewModel> UserAchievements { get; set; }
    }
}
