using System;

namespace PuffPal.Models
{
    public class ProgressTracker : ITrackable
    {
        public int UserId
        {
            get => _userId;
            set
            {
                if (value <= 0) throw new ArgumentException("UserId must be greater than 0.");
                _userId = value;
            }
        }
        private int _userId;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("StartDate cannot be in the future.");
                _startDate = value;
            }
        }
        private DateTime _startDate;

        public int InitialPuffsPerDay { get; set; }
        public int CurrentPuffsPerDay { get; set; }
        public int GoalPuffsPerDay { get; set; } = 0; // Default goal is 0 puffs/day

        public string TrackProgress()
        {
            int daysElapsed = (DateTime.Now - StartDate).Days;
            return $"User {UserId} has reduced from {InitialPuffsPerDay} to {CurrentPuffsPerDay} puffs/day in {daysElapsed} days.";
        }

        public double CalculateReductionPercentage()
        {
            if (InitialPuffsPerDay < 0 || CurrentPuffsPerDay < 0)
                throw new InvalidOperationException("Puffs per day cannot be negative.");

            if (InitialPuffsPerDay == 0) return 100;

            return ((double)(InitialPuffsPerDay - CurrentPuffsPerDay) / InitialPuffsPerDay) * 100;
        }

        public bool HasReachedGoal()
        {
            return CurrentPuffsPerDay <= GoalPuffsPerDay;
        }
    }
}