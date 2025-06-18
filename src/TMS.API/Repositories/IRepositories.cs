using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(Priority? priority, Status? status, int skip, int take, CancellationToken cancellationToken);
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken);
        Task<TaskItem?> UpdateAsync(TaskItem task, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}