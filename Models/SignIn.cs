// This class represents the data required for a user to sign in.
// It includes properties for the user's email and password, both of which are required.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class SignIn
    {
        // The user's password, required for authentication.
        [Required]
        public string Password { get; set; } = string.Empty;

        // The user's email address, required for authentication.
        [Required]
        public string Email { get; set; }
    }
}