using System.Collections.Generic;
using System.Linq;
using PuffPal.Models;

namespace PuffPal.Services
{
    public class UserService
    {
        private readonly List<User> users;

        public UserService()
        {
            users = new List<User>();
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public User GetUserById(int userId)
        {
            return users.FirstOrDefault(u => u.UserId == userId); // Checks list for matching userId
        }
    }
}