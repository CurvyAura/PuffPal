// This class represents the data required for a user to sign up for an account.
// It includes properties for the user's username, password, and email, all of which are required.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class SignUp
    {
        // The username chosen by the user, required for account creation.
        [Required]
        public string UserName { get; set; }

        // The user's password, required for account creation.
        [Required]
        public string Password { get; set; }

        // The user's email address, required for account creation.
        [Required]
        public string Email { get; set; }
    }
}