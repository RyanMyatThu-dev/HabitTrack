using System;

namespace HabitTracker.Application.DTOs
{
    public class HabitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int CurrentStreak { get; set; }
        public int CompletionCount { get; set; }
        public bool IsCompletedToday { get; set; }
    }

    public class CreateHabitRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "🔥";
        public string Color { get; set; } = "#4F46E5";
    }
}
