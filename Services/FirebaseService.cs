using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
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

        public async Task SaveUserProfileAsync(string userid, DateTime quitDay) //E
        {
            //string userId = await GetCurrentUserIdAsync(); //Retrives userid from login

            //if (string.IsNullOrEmpty(userId))
            //{
            //    throw new InvalidOperationException("User is not logged in.");
            //}
            string jsonquitdate = JsonSerializer.Serialize(quitDay.ToString("o"));
            await _client
                .Child("users")
                .Child(userid)
                .Child("QuitDate")
                .PutAsync(jsonquitdate);
        }

        // Save a puff event for the current day

        //•	Purpose: Increment the puff count for the current day.
        //•	Logic:
        //•	Use the current date(yyyy-MM-dd) as the key for the puff count.
        //•	Retrieve the current puff count for the day.
        //•	Increment the count and save it back to the database.

        public async Task SavePuffAsync(string userId, DateTime timestamp) //E
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


        // Retrieve daily puff data for a user  

        //•	Purpose: Retrieve all daily puff counts for a user.
        //•	Logic:
        //•	Fetch all puff data for the user from the dailyPuffs/{ userId} path.
        //•	Convert the data into a list of daily puff counts.

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

                // Add the puff count to the dictionary (default to 0 if no data exists)
                weeklyPuffData[dateKey] = puffData ?? 0;
            }

            return weeklyPuffData;
        }

    }
}