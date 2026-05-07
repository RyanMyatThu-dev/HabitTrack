using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Application.Services
{
    public interface IHabitService
    {
        Task<IEnumerable<HabitDto>> GetActiveHabitsAsync();
        Task CreateHabitAsync(CreateHabitRequest request);
        Task ToggleHabitCompletionAsync(Guid habitId, DateTime date);
        Task DeleteHabitAsync(Guid habitId);
    }

    public class HabitService : IHabitService
    {
        private readonly IHabitRepository _repository;

        public HabitService(IHabitRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<HabitDto>> GetActiveHabitsAsync()
        {
            var habits = await _repository.GetAllAsync();
            var today = DateTime.UtcNow.Date;

            return habits.Where(h => !h.IsArchived).Select(h => new HabitDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Icon = h.Icon,
                Color = h.Color,
                IsCompletedToday = h.Logs.Any(l => l.Date.Date == today && l.IsCompleted),
                CurrentStreak = CalculateStreak(h),
                CompletionCount = h.Logs.Count(l => l.IsCompleted)
            });
        }

        public async Task CreateHabitAsync(CreateHabitRequest request)
        {
            var habit = new Habit
            {
                Name = request.Name,
                Description = request.Description,
                Icon = request.Icon,
                Color = request.Color
            };
            await _repository.AddAsync(habit);
        }

        public async Task DeleteHabitAsync(Guid habitId)
        {
            await _repository.DeleteAsync(habitId);
        }

        public async Task ToggleHabitCompletionAsync(Guid habitId, DateTime date)
        {
            var logs = await _repository.GetLogsByHabitIdAsync(habitId);
            var existingLog = logs.FirstOrDefault(l => l.Date.Date == date.Date);

            if (existingLog != null)
            {
                await _repository.RemoveLogAsync(existingLog.Id);
            }
            else
            {
                await _repository.AddLogAsync(new HabitLog
                {
                    HabitId = habitId,
                    Date = date.Date,
                    IsCompleted = true
                });
            }
        }

        private int CalculateStreak(Habit habit)
        {
            if (!habit.Logs.Any()) return 0;

            var dates = habit.Logs
                .Where(l => l.IsCompleted)
                .Select(l => l.Date.Date)
                .OrderByDescending(d => d)
                .Distinct()
                .ToList();

            if (!dates.Any()) return 0;

            var today = DateTime.UtcNow.Date;
            var yesterday = today.AddDays(-1);

            // If not completed today or yesterday, streak is broken
            if (dates[0] < yesterday) return 0;

            int streak = 0;
            DateTime current = dates[0];

            for (int i = 0; i < dates.Count; i++)
            {
                if (i == 0 || dates[i] == dates[i - 1].AddDays(-1))
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return streak;
        }
    }
}
