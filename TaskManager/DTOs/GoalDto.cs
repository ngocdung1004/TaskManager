using System;
using TaskManager.Models;

namespace TaskManager.DTOs
{
    public class GoalDto
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
    }

    public class CreateGoalDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public GoalType GoalType { get; set; }
        public DateTime? TargetDate { get; set; }
    }

    public class UpdateGoalDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public GoalType GoalType { get; set; }
        public GoalStatus Status { get; set; }
        public DateTime? TargetDate { get; set; }
        public int ProgressPercentage { get; set; }
    }
} 