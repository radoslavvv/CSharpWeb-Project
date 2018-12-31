using System.Collections.Generic;
using CSharpWebProject.Models.EntityModels;
using CSharpWebProject.Models.ViewModels;

namespace CSharpWebProject.Services
{
    public interface IAchievementsService
    {
        bool AddAchievement(string achievementName, string userName);
        bool CheckForTimesAchievements(string username);
        bool CheckForCompetitionAchievements(string username);
        MyAchievementsViewModel GetAllAchievements(string username);
        void SeedDabase();
    }
}