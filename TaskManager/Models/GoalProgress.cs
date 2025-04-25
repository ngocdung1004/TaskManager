using System;

namespace TaskManager.Models
{
    public class GoalProgress
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public GoalStatus StatusChange { get; set; }
        public int? ProgressValue { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Goal Goal { get; set; }
    }
} 