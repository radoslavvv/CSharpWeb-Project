using CSharpWebProject.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using CSharpWebProject.Models;

namespace CSharpWebProject.Models.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PaginatedList<CompetitorViewModel> Competitors { get; set; }

        public List<CompetitorViewModel> Winners { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Sponsor { get; set; }

        public bool IsOpen { get; set; }

        public bool UserIsInCompetition { get; set; }

        public bool CompetitorHasBestTime { get; set; }
    }
}
