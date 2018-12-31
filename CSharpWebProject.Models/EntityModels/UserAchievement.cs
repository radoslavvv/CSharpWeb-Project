using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWebProject.Models.EntityModels
{
    public class UserAchievement
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; }
    }
}
