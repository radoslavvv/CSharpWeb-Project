using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class MyAchievementsViewModel
    {
        public List<AchievementViewModel> CompetitionAchivements { get; set; }

        public List<AchievementViewModel> TimesAchivements { get; set; }

        public List<AchievementViewModel> UserAchievements { get; set; }
    }
}
