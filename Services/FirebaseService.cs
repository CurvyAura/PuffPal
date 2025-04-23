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

        public async Task SaveUserProfileAsync(string userid, string name, DateTime quitDay)
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

        public async Task SavePuffAsync(string userId, DateTime timestamp)
        {
            string today = timestamp.ToString("yyyy-MM-dd");

            // Get the current puff count for today
            var puffData = await _client
                .Child("dailyPuffs")
                .Child(userId)
                .Child(today)
                .OnceSingleAsync<int?>();

            int currentCount = puffData ?? 0;

            // Increment the puff count
            await _client
                .Child("dailyPuffs")
                .Child(userId)
                .Child(today)
                .PutAsync(currentCount + 1);
        }


        public async Task<int> GetDailyPuffCountAsync(string userId)
        {
            //string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string today = DateTime.Now.ToString("yyyy-MM-dd");


            // Retrieve the puff count for today
            var puffData = await _client
                .Child("dailyPuffs")
                .Child(userId) 
                .Child(today)
                .OnceSingleAsync<int?>();

            return puffData ?? 0;
        }


        // Retrieve daily puff data for a user  

        //•	Purpose: Retrieve all daily puff counts for a user.
        //•	Logic:
        //•	Fetch all puff data for the user from the dailyPuffs/{ userId} path.
        //•	Convert the data into a list of daily puff counts.

        public async Task<List<int>> GetDailyPuffDataAsync(int userId)
        {
            var puffData = await _client
                .Child("dailyPuffs")
                .Child(userId.ToString())
                .OnceAsync<KeyValuePair<string, int>>();

            // Convert the data into a list of daily puff counts  
            var dailyPuffCounts = puffData
                .Select(static p => p.Object.Value)
                .ToList();

            return dailyPuffCounts;
        }

        public async Task<DateTime?> GetLastPuffTimeAsync(string userId)
        {
            // Retrieve the serialized last puff time for the user
            var serializedTimestamp = await _client
                .Child("lastPuffTime")
                .Child(userId)
                .OnceSingleAsync<string>();

            // Deserialize the timestamp back into a DateTime object
            if (DateTime.TryParse(serializedTimestamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out var lastPuffTime))
            {
                return lastPuffTime;
            }

            return null; // Return null if the timestamp is not valid
        }


        public async Task SaveLastPuffTimeAsync(string userId, DateTime timestamp)
        {
            // Serialize the DateTime to an ISO 8601 string format
            string serializedTimestamp = timestamp.ToString("o"); // "o" is the round-trip format (ISO 8601)

            // Save the serialized timestamp as the last puff time for the user
            await _client
                .Child("lastPuffTime")
                .Child(userId)
                .PutAsync(serializedTimestamp);
        }

    }
}
