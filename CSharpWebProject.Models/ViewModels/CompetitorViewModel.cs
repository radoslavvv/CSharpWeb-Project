using CSharpWebProject.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class CompetitorViewModel
    {
        public string Name { get; set; }

        public string BestTime { get; set; }

        public string BestTimeDate { get; set; }

        public int Place { get; set; }
    }
}
