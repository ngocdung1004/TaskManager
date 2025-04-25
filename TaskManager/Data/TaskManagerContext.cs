using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalProgress> GoalProgresses { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Goal configuration
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Goal>()
                .Property(g => g.ProgressPercentage)
                .HasDefaultValue(0);

            // GoalProgress configuration
            modelBuilder.Entity<GoalProgress>()
                .HasOne(gp => gp.Goal)
                .WithMany(g => g.GoalProgresses)
                .HasForeignKey(gp => gp.GoalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reminder configuration
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Goal)
                .WithMany(g => g.Reminders)
                .HasForeignKey(r => r.GoalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 