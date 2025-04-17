using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class Quote : Motivation
    {
        public int QuoteID { get; set; }

        public override void DisplayMessage()
        {
            Console.WriteLine($"Quote: {TextBody}"); //Temp
        }
    }
}
