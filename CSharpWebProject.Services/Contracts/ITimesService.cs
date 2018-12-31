using System.Collections.Generic;
using CSharpWebProject.Models.EntityModels;

namespace CSharpWebProject.Services
{
    public interface ITimesService
    {
        bool AddTime(SolveTime time, string userId);
        bool AddTimes(List<SolveTime> times, string userId);
        List<SolveTime> GetAllUserTimes(string username);
        List<SolveTime> GetAllTimes();
    }
}