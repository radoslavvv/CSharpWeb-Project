using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class Winner
    {
        public int  Id { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int Place { get; set; }
    }
}
