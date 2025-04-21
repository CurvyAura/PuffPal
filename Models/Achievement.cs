using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class Achievement
    {
        public string Title { get; set; }
        public int AchievementID { get; set; }
        public DateTime DateAchieved { get; set; }
        public string Description { get; set; }

        public void AddAchievement()
        {
            // Add logic to save the achievement to the database
        }
        public void DeleteAchievement()
        {
            // Add logic to delete the achievement from the database
        }
        public void UpdateAchievement()
        {
            // Add logic to update the achievement in the database
        }
        public void GetAchievement()
        {
            // Add logic to retrieve the achievement from the database
        }
    }
}
