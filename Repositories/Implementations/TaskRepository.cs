using MongoDB.Driver;
using piton_taskmanagement_api.Context;
using piton_taskmanagement_api.Models;
using piton_taskmanagement_api.Repositories.Interfaces;

namespace piton_taskmanagement_api.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<TaskItem> _taskCollection;

        public TaskRepository(IMongoDbContext context)
        {
            _taskCollection = context.GetDatabase().GetCollection<TaskItem>("tasks");
        }

        public async Task<List<TaskItem>> GetAllByUserIdAsync(string userId)
        {
            return await _taskCollection.Find(t => t.OwnerId == userId).ToListAsync();
        }

        public async Task<List<TaskItem>> GetByDateRangeAsync(string userId, DateTime start, DateTime end)
        {
            return await _taskCollection.Find(t =>
                t.OwnerId == userId &&
                t.DueDate >= start &&
                t.DueDate <= end).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(string id)
        {
            return await _taskCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(TaskItem task)
        {
            await _taskCollection.InsertOneAsync(task);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            await _taskCollection.ReplaceOneAsync(t => t.Id == task.Id, task);
        }

        public async Task DeleteAsync(string id)
        {
            await _taskCollection.DeleteOneAsync(t => t.Id == id);
        }
    }
}
