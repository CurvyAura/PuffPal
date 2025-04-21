using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class DailyLog
    {
        public DateTime Date { get; set; }
        public int LogID { get; set; }
        public string CravingLevel { get; set; } //very low, low, medium, high saved to database as int (1-4)

        public void AddLog()
        {
            // Add logic to save the log to the database
        }
        public void DeleteLog()
        {
            // Add logic to delete the log from the database
        }
        public void UpdateLog()
        {
            // Add logic to update the log in the database
        }
        public void GetLog()
        {
            // Add logic to retrieve the log from the database
        }

    }
}
