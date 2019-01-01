using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class NewsPost
    {
        public string Title { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
