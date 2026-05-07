using HabitTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Interfaces
{
    public interface IHabitRepository
    {
        Task<IEnumerable<Habit>> GetAllAsync();
        Task<Habit?> GetByIdAsync(Guid id);
        Task AddAsync(Habit habit);
        Task UpdateAsync(Habit habit);
        Task DeleteAsync(Guid id);
        
        Task AddLogAsync(HabitLog log);
        Task RemoveLogAsync(Guid logId);
        Task<IEnumerable<HabitLog>> GetLogsByHabitIdAsync(Guid habitId);
    }
}
