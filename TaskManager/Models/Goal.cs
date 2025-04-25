using System;
using System.Collections.Generic;

namespace TaskManager.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public GoalType GoalType { get; set; }
        public GoalStatus Status { get; set; }
        public DateTime? TargetDate { get; set; }
        public int ProgressPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<GoalProgress> GoalProgresses { get; set; }
        public ICollection<Reminder> Reminders { get; set; }
    }

    public enum GoalType
    {
        ShortTerm,
        LongTerm
    }

    public enum GoalStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
} 