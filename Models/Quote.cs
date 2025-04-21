using System;

namespace PuffPal.Models
{
    public class Quote : Motivation
    {
        public int QuoteID { get; set; } // Unique identifier for the quote

        public override string DisplayMessage()
        {
            return $"Quote: {TextBody}";
        }
    }
}