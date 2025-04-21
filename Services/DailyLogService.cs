using System;
using System.Collections.Generic;
using System.Linq;
using PuffPal.Models;

namespace PuffPal.Services
{
    public class DailyLogService
    {
        private readonly List<DailyLog> dailyLogs;

        public DailyLogService()
        {
            dailyLogs = new List<DailyLog>();
        }

        public void AddLog(DailyLog log)
        {
            log.LogID = dailyLogs.Count + 1; // Auto-generate ID
            dailyLogs.Add(log);
        }

        public void DeleteLog(int logId)
        {
            var log = dailyLogs.FirstOrDefault(l => l.LogID == logId);
            if (log != null)
            {
                dailyLogs.Remove(log);
            }
        }

        public void UpdateLog(DailyLog updatedLog)
        {
            var log = dailyLogs.FirstOrDefault(l => l.LogID == updatedLog.LogID);
            if (log != null)
            {
                log.Date = updatedLog.Date;
                log.PuffsTaken = updatedLog.PuffsTaken;
                log.Mood = updatedLog.Mood;
                log.Notes = updatedLog.Notes;
            }
        }

        public DailyLog GetLogById(int logId)
        {
            return dailyLogs.FirstOrDefault(l => l.LogID == logId);
        }

        public List<DailyLog> GetLogsByUserId(int userId)
        {
            return dailyLogs.Where(l => l.UserID == userId).ToList();
        }

        public List<DailyLog> GetAllLogs()
        {
            return dailyLogs;
        }
    }
}