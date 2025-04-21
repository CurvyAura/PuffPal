using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public abstract class Motivation
    {
        public string? Title { get; set; }
        public string? TextBody { get; set; }
        public abstract string DisplayMessage();
    }
}
