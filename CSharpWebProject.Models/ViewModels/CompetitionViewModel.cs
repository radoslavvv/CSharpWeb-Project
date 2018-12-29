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

        public int CompetitorsCount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
