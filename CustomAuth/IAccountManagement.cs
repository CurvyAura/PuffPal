// This interface defines methods for managing user accounts, including registration, login, logout, and authentication checks.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuffPal.Models;

namespace PuffPal.CustomAuth
{
    public interface IAccountManagement
    {
        // Registers a new user with the provided email, username, and password.
        public Task<FormResult> RegisterAsync(string email, string username, string password);

        // Logs in a user with the provided email and password.
        public Task<FormResult> LoginAsync(string email, string password);

        // Logs out the currently authenticated user.
        public Task LogoutAsync();

        // Checks if the current user is authenticated.
        public Task<bool> CheckAuthAsync();
    }
}