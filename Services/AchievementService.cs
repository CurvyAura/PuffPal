using System;
using System.Collections.Generic;
using System.Linq;
using PuffPal.Models;

namespace PuffPal.Services
{
    public class AchievementService
    {
        private readonly List<Achievement> achievements;

        public AchievementService()
        {
            achievements = new List<Achievement>();
        }

        public void AddAchievement(Achievement achievement)
        {
            achievement.AchievementID = achievements.Count + 1; // Auto-generate ID
            achievement.DateAchieved = DateTime.Now; // Set the date achieved
            achievements.Add(achievement);
        }

        public void DeleteAchievement(int achievementId)
        {
            var achievement = achievements.FirstOrDefault(a => a.AchievementID == achievementId);
            if (achievement != null)
            {
                achievements.Remove(achievement);
            }
        }

        public void UpdateAchievement(Achievement updatedAchievement)
        {
            var achievement = achievements.FirstOrDefault(a => a.AchievementID == updatedAchievement.AchievementID);
            if (achievement != null)
            {
                achievement.Title = updatedAchievement.Title;
                achievement.Description = updatedAchievement.Description;
                achievement.DateAchieved = updatedAchievement.DateAchieved;
            }
        }

        public Achievement GetAchievementById(int achievementId)
        {
            return achievements.FirstOrDefault(a => a.AchievementID == achievementId);
        }

        public List<Achievement> GetAllAchievements()
        {
            return achievements;
        }
    }
}