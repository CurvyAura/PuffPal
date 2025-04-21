using System;

namespace PuffPal.Models
{
    public enum CravingLevel
    {
        VeryLow = 1,
        Low = 2,
        Medium = 3,
        High = 4
    }

    public class DailyLog
    {
        public int LogID { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public CravingLevel Craving { get; set; }
        public string Notes { get; set; }

        public string GetAdvice()
        {
            return Craving switch
            {
                CravingLevel.VeryLow => "You're doing great! Keep it up.",
                CravingLevel.Low => "Mild craving — maybe go for a walk or sip some water.",
                CravingLevel.Medium => "Moderate craving — try a distraction like music or calling a friend.",
                CravingLevel.High => "Strong craving — take deep breaths, ride it out. You’ve got this.",
                _ => "Stay strong. You're making progress every day!"
            };
        }
        public void AddLog()
        {
            // Add logic to save the log to the database
        }
        public void DeleteLog()
        {
            // Add logic to delete the log from the database
        }
        public void UpdateLog()
        {
            // Add logic to update the log in the database
        }
        public void GetLog()
        {
            // Add logic to retrieve the log from the database
        }

    }
}
