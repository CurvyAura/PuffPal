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
    }
}
