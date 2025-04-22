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
        [Required] public string Password { get; set; } = string.Empty;
        [Required] public string Email { get; set; }
        
    }
}
