using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSharpWebProject.Models.ViewModels
{
    public class SolveTimeCreateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"\d{1,2}:\d{1,2}:\d{1,3}",ErrorMessage ="Result format must be: mm:ss:fff")]
        public string Result { get; set; }

        [Required(ErrorMessage ="You must enter Type!")]
        public string Type { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage ="You must enter Date!")]
        public DateTime Date { get; set; }
    }
}
