using System;

namespace TaskManager.DTOs
{
    public class ReminderDto
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public DateTime ReminderDate { get; set; }
        public string ReminderMessage { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReminderDto
    {
        public int GoalId { get; set; }
        public DateTime ReminderDate { get; set; }
        public string ReminderMessage { get; set; }
    }

    public class UpdateReminderDto
    {
        public DateTime ReminderDate { get; set; }
        public string ReminderMessage { get; set; }
        public bool IsSent { get; set; }
    }
} 