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
        public Task<FormResult> RegisterAsync(string email, string username, string password);
        public Task<FormResult> LoginAsync(string email, string password);
        public Task LogoutAsync();
        public Task<bool> CheckAuthAsync();
    }
}
