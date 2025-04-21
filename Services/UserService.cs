using System.Collections.Generic;
using System.Linq;
using PuffPal.Models;

namespace PuffPal.Services
{
    public class UserService
    {
        private readonly List<UserInfo> users;

        public UserService()
        {
            users = new List<UserInfo>();
        }

        public void AddUser(UserInfo user)
        {
            users.Add(user);
        }

        public void RemoveUser(UserInfo user)
        {
            users.Remove(user);
        }

        public List<UserInfo> GetAllUsers()
        {
            return users;
        }

        public UserInfo GetUserById(int userId)
        {
            return users.FirstOrDefault(u => u.UserId == userId); // Checks list for matching userId
        }
    }
}