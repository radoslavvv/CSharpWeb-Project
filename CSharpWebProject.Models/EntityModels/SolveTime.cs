﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class SolveTime
    {
        public int Id { get; set; }

        public string Result { get; set; }

        public string PuzzleType { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}