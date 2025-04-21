using System.Collections.Generic;
using System.Linq;
using PuffPal.Models;

namespace PuffPal.Services
{
    public class ProgressService
    {
        private readonly List<ProgressTracker> progressTrackers = new();

        public void AddProgressTracker(ProgressTracker tracker)
        {
            progressTrackers.Add(tracker);
        }

        public ProgressTracker GetProgressByUserId(int userId) // Unsure about this, might change.
        {
            return progressTrackers.FirstOrDefault(t => t.UserId == userId);
        }

        public List<ProgressTracker> GetAllProgressTrackers()
        {
            return progressTrackers;
        }
    }
}