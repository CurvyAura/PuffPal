using System;

namespace PuffPal.Models
{
    public class Achievement
    {
        public int AchievementID { get; set; } // Unique identifier for the achievement
        public string? Title { get; set; } // Title of the achievement
        public DateTime DateAchieved { get; set; } // Date the achievement was earned
        public string? Description { get; set; } // Description of the achievement
    }
}