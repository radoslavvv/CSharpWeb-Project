using CSharpWebProject.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class CompetitionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Competitor> Competitors { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsOpen { get; set; }

        public string Sponsor { get; set; }
    }
}
