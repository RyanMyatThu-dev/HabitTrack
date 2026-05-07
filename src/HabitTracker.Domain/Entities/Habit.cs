using System;
using System.Collections.Generic;

namespace HabitTracker.Domain.Entities
{
    public class Habit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "🔥"; // Default emoji
        public string Color { get; set; } = "#4F46E5"; // Default indigo
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<HabitLog> Logs { get; set; } = new();
        public bool IsArchived { get; set; }
        
        // Frequency logic (could be extended)
        public int TargetDaysPerWeek { get; set; } = 7;
    }

    public class HabitLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid HabitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
