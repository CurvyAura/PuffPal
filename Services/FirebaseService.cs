// This class provides methods to interact with Firebase Realtime Database.
// It includes functionality for saving and retrieving user data, puff data, and goals.

using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PuffPal.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient _client;

        // Initializes the Firebase client with the database URL.
        public FirebaseService()
        {
            _client = new Firebase.Database.FirebaseClient("https://puffpal-fadb9-default-rtdb.firebaseio.com/");
        }

        // Sends a test puff message to the Firebase database.
        public async Task SendTestPuffAsync(string message)
        {
            await _client
                .Child("testPuffs")
                .PostAsync(new { message = message, timestamp = DateTime.UtcNow });
        }

        // Saves the user's quit date to the Firebase database.
        public async Task SaveUserProfileAsync(string userid, DateTime quitDay)
        {
            string jsonquitdate = JsonSerializer.Serialize(quitDay.ToString("o"));
            await _client
                .Child("users")
                .Child(userid)
                .Child("QuitDate")
                .PutAsync(jsonquitdate);
        }

        // Saves a puff event for the user on a specific date.
        public async Task SavePuffAsync(string userId, DateTime timestamp)
        {
            string today = timestamp.ToString("yyyy-MM-dd");

            // Get the current puff count for today
            var puffData = await _client
               .Child("users")
               .Child(userId)
               .Child("dailyPuffs")
               .OnceSingleAsync<Dictionary<string, int>>();

            int currentCount = puffData != null && puffData.ContainsKey(today) ? puffData[today] : 0;

            // Increment the puff count
            await _client
                .Child("users")
                .Child(userId)
                .Child("dailyPuffs")
                .Child(today)
                .PutAsync(currentCount + 1);
        }

        // Retrieves the puff count for the current day for a specific user.
        public async Task<int> GetDailyPuffCountAsync(string userId)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            var puffData = await _client
                .Child("users")
                .Child(userId)
                .Child("dailyPuffs")
                .Child(today)
                .OnceSingleAsync<int?>();

            return puffData ?? 0;
        }

        // Retrieves all daily puff data for a specific user.
        public async Task<List<int>> GetDailyPuffDataAsync(string userId)
        {
            var puffDict = await _client
                .Child("users")
                .Child(userId)
                .Child("dailyPuffs")
                .OnceSingleAsync<Dictionary<string, int>>();

            if (puffDict == null || puffDict.Count == 0)
                return new List<int>();

            return puffDict.Values.ToList();
        }

        // Retrieves the timestamp of the user's last puff event.
        public async Task<DateTime?> GetLastPuffTimeAsync(string userId)
        {
            var lastPuffTime = await _client
                .Child("users")
                .Child(userId)
                .Child("lastPuffTime")
                .OnceSingleAsync<DateTime?>();

            return lastPuffTime;
        }

        // Saves the timestamp of the user's last puff event.
        public async Task SaveLastPuffTimeAsync(string userId, DateTime timestamp)
        {
            await _client
                .Child("users")
                .Child(userId)
                .Child("lastPuffTime")
                .PutAsync(timestamp);
        }

        // Retrieves puff data for the past 7 days for a specific user.
        public async Task<Dictionary<string, int>> GetWeeklyPuffDataAsync(string userId)
        {
            DateTime today = DateTime.Now;
            Dictionary<string, int> weeklyPuffData = new Dictionary<string, int>();

            // Loop through the past 7 days
            for (int i = 0; i < 7; i++)
            {
                DateTime date = today.AddDays(-i);
                string dateKey = date.ToString("yyyy-MM-dd");

                // Retrieve the puff count for the specific day
                var puffData = await _client
                    .Child("users")
                    .Child(userId)
                    .Child("dailyPuffs")
                    .Child(dateKey)
                    .OnceSingleAsync<int?>();

                // If puffData is null, set it to zero
                if (puffData == null)
                {
                    await _client
                        .Child("users")
                        .Child(userId)
                        .Child("dailyPuffs")
                        .Child(dateKey)
                        .PutAsync(0);
                }

                // Add the puff count to the dictionary (default to 0 if no data exists)
                weeklyPuffData[dateKey] = puffData ?? 0;
            }

            return weeklyPuffData;
        }

        // Retrieves the user's quit date from the Firebase database.
        public async Task<DateTime?> GetQuitDateAsync(string userId)
        {
            try
            {
                var quitDateString = await _client
                    .Child("users")
                    .Child(userId)
                    .Child("QuitDate")
                    .OnceSingleAsync<string>();

                if (!string.IsNullOrEmpty(quitDateString))
                {
                    return DateTime.Parse(quitDateString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving quit date: {ex.Message}");
            }

            return null;
        }

        // Saves the user's daily puff goals to the Firebase database.
        public async Task SaveDailyPuffGoalsAsync(string userId, Dictionary<string, int> dailyPuffGoals)
        {
            foreach (var goal in dailyPuffGoals)
            {
                await _client
                    .Child("users")
                    .Child(userId)
                    .Child("puffGoals")
                    .Child(goal.Key) // Date as the key
                    .PutAsync(goal.Value); // Puff goal as the value
            }
        }
    }
}