using System;

namespace PuffPal.Models
{
    public class DailyLog
    {
        public int LogID { get; set; } // Unique identifier for the log
        public int UserID { get; set; } // ID of the user this log belongs to
        public DateTime Date { get; set; } // The date of the log
        public int PuffsTaken { get; set; } // Number of puffs taken on this day
        public string? Mood { get; set; } // Mood of the user (e.g., "Happy", "Stressed")
        public string? Notes { get; set; } // Additional notes for the day
    }
}