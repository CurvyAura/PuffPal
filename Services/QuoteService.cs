using System.Collections.Generic;
using System.Linq;
using PuffPal.Models;

namespace PuffPal.Services
{
    public class QuoteService
    {
        private readonly List<Quote> quotes;

        public QuoteService()
        {
            // Initialize with some sample quotes
            quotes = new List<Quote>
            {
                new Quote { QuoteID = 1, Title = "Perseverance", TextBody = "The journey of a thousand miles begins with a single step." },
                new Quote { QuoteID = 2, Title = "Motivation", TextBody = "Believe you can and you're halfway there." },
                new Quote { QuoteID = 3, Title = "Determination", TextBody = "Success is not final, failure is not fatal: It is the courage to continue that counts." }
            };
        }

        public List<Quote> GetAllQuotes()
        {
            return quotes;
        }

        public Quote GetQuoteById(int quoteId)
        {
            return quotes.FirstOrDefault(q => q.QuoteID == quoteId);
        }

        public void AddQuote(Quote quote)
        {
            quote.QuoteID = quotes.Count + 1; // Auto-generate ID
            quotes.Add(quote);
        }

        public void DeleteQuote(int quoteId)
        {
            var quote = quotes.FirstOrDefault(q => q.QuoteID == quoteId);
            if (quote != null)
            {
                quotes.Remove(quote);
            }
        }
    }
}