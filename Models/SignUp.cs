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
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }
    }
}
