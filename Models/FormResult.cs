// This class represents the result of a form operation, such as login or registration.
// It includes a flag indicating success and a list of error messages, if any.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class FormResult
    {
        // Indicates whether the operation was successful.
        public bool Succeeded { get; set; }

        // A list of error messages, if the operation failed.
        public string[] ErrorList { get; set; } = Array.Empty<string>();
    }
}