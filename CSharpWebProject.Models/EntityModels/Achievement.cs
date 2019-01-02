using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(15)]
        public string Name { get; set; }

        [Required]
        [MinLength(15)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        public virtual List<UserAchievement> Users { get; set; }
    }
}
