using System.Collections.Generic;
using CSharpWebProject.Models.EntityModels;

namespace CSharpWebProject.Services
{
    public interface ITimesService
    {
        void AddTime(SolveTime time, string userId);
        void AddTimes(List<SolveTime> times, string userId);
        List<SolveTime> GetAllTimes(string username);
    }
}