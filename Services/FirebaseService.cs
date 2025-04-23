using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            await _client
                .Child("users")
                .Child("userid")
                .Child("name").PutAsync(name);

            await _client
                .Child("users")
                .Child("userid")
                .Child("quitDay").PutAsync(quitDay);
        }

        // Put database service funcs below (also finish the ones that arent done)
        public async Task SavePuffAsync(int userId, DateTime timestamp)
        {
            // Logic to save puff data to the database
            // Example: Save to Firebase or another database
        }

        public async Task<List<int>> GetDailyPuffDataAsync(int userId)
        {
            // Logic to fetch daily puff data from the database
            // Example: Fetch from Firebase or another database
            return new List<int> { 23, 12, 5, 26, 0 }; // Example data
        }
    }
}
