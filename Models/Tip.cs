using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class Tip : Motivation
    {
        public int TipID { get; set; }

        public override void DisplayMessage()
        {
            Console.WriteLine($"Tip of the Day: {Title}\n{TextBody}");
        }
    }
}
