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

        public FirebaseService()
        {
            _client = new Firebase.Database.FirebaseClient("https://puffpal-fadb9-default-rtdb.firebaseio.com/");
        }

        public async Task SendTestPuffAsync(string message)
        {
            await _client
                .Child("testPuffs")
                .PostAsync(new { message = message, timestamp = DateTime.UtcNow });
        }

        public async Task SaveUserProfileAsync(string userid, DateTime quitDay) 
        {
            string jsonquitdate = JsonSerializer.Serialize(quitDay.ToString("o"));
            await _client
                .Child("users")
                .Child(userid)
                .Child("QuitDate")
                .PutAsync(jsonquitdate);
        }

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

        public async Task<DateTime?> GetLastPuffTimeAsync(string userId)
        {
            var lastPuffTime = await _client
                .Child("users")
                .Child(userId)
                .Child("lastPuffTime")
                .OnceSingleAsync<DateTime?>();

            return lastPuffTime;
        }

        public async Task SaveLastPuffTimeAsync(string userId, DateTime timestamp)
        {
            await _client
                .Child("users")
                .Child(userId)
                .Child("lastPuffTime")
                .PutAsync(timestamp);
        }

        public async Task<Dictionary<string, int>> GetWeeklyPuffDataAsync(string userId)
        {
            // Get the current date
            DateTime today = DateTime.Now;

            // Create a dictionary to store puff data for the past 7 days
            Dictionary<string, int> weeklyPuffData = new Dictionary<string, int>();

            // Loop through the past 7 days
            for (int i = 0; i < 7; i++)
            {
                // Calculate the date for each day
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

        public async Task<DateTime?> GetQuitDateAsync(string userId)
        {
            try
            {
                // Retrieve the quit date from Firebase
                var quitDateString = await _client
                    .Child("users")
                    .Child(userId)
                    .Child("QuitDate")
                    .OnceSingleAsync<string>();

                if (!string.IsNullOrEmpty(quitDateString))
                {
                    // Parse the quit date string into a DateTime object
                    return DateTime.Parse(quitDateString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving quit date: {ex.Message}");
            }

            // Return null if no quit date is found or an error occurs
            return null;
        }

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