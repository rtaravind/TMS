using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(Priority? priority, Status? status, int skip, int take, CancellationToken cancellationToken);
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<TaskItem> CreateAsync(TaskItem task, CancellationToken cancellationToken);
        Task<TaskItem?> UpdateAsync(int id, TaskItem task, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}