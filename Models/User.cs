﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime QuitDate { get; set; }
        public int PuffsPerDay { get; set; }

        public int DaysSinceQuit ()
        {
            return (DateTime.Now - QuitDate).Days;
        }
    }



    public class UserManager
    {
        private List<User> users;
        public UserManager()
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
    }
}

