using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class CompetitionsIndexViewModel
    {
        public List<CompetitionViewModel> OpenCompetitions { get; set; }

        public List<CompetitionViewModel> ClosedCompetitions { get; set; }

    }
}
