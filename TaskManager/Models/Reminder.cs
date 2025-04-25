using System;

namespace TaskManager.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public DateTime ReminderDate { get; set; }
        public string ReminderMessage { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Goal Goal { get; set; }
    }
} 