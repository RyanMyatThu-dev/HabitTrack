using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Persistence
{
    public class IndexedDbHabitRepository : IHabitRepository
    {
        private readonly IJSRuntime _jsRuntime;
        private const string StoreName = "habits";

        // Use case-insensitive deserialization so JS object keys map to C# properties
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public IndexedDbHabitRepository(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<IEnumerable<Habit>> GetAllAsync()
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("indexedDbManager.getAll", StoreName);
                return JsonSerializer.Deserialize<List<Habit>>(json, _jsonOptions) ?? new List<Habit>();
            }
            catch
            {
                return new List<Habit>();
            }
        }

        public async Task<Habit?> GetByIdAsync(Guid id)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("indexedDbManager.getById", StoreName, id.ToString());
                if (string.IsNullOrEmpty(json) || json == "null") return null;
                return JsonSerializer.Deserialize<Habit>(json, _jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        public async Task AddAsync(Habit habit)
        {
            var json = JsonSerializer.Serialize(habit, _jsonOptions);
            await _jsRuntime.InvokeVoidAsync("indexedDbManager.add", StoreName, json);
        }

        public async Task UpdateAsync(Habit habit)
        {
            var json = JsonSerializer.Serialize(habit, _jsonOptions);
            await _jsRuntime.InvokeVoidAsync("indexedDbManager.update", StoreName, json);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _jsRuntime.InvokeVoidAsync("indexedDbManager.delete", StoreName, id.ToString());
        }

        public async Task AddLogAsync(HabitLog log)
        {
            var habit = await GetByIdAsync(log.HabitId);
            if (habit != null)
            {
                habit.Logs.Add(log);
                await UpdateAsync(habit);
            }
        }

        public async Task RemoveLogAsync(Guid logId)
        {
            var habits = await GetAllAsync();
            foreach (var habit in habits)
            {
                var log = habit.Logs.Find(l => l.Id == logId);
                if (log != null)
                {
                    habit.Logs.Remove(log);
                    await UpdateAsync(habit);
                    break;
                }
            }
        }

        public async Task<IEnumerable<HabitLog>> GetLogsByHabitIdAsync(Guid habitId)
        {
            var habit = await GetByIdAsync(habitId);
            return habit?.Logs ?? new List<HabitLog>();
        }
    }
}
