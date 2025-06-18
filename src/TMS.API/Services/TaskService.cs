using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TMS.API.Exceptions;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository _repo;
        private readonly ILogger<TaskService> _logger;        
        public TaskService(IRepository repo, ILogger<TaskService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(
            Priority? priority, 
            Status? status, 
            int skip, 
            int take, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync(priority, status, skip, take, cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<TaskItem> CreateAsync(TaskItem task, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ValidationException("Title is required.");
            }

            var created = await _repo.AddAsync(task, cancellationToken);

            if (created.Priority == Priority.High)
                _logger.LogCritical($"High priority task created: {created.Title}");

            return created;
        }

        public async Task<TaskItem?> UpdateAsync(int id, TaskItem task, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ValidationException("Title is required.");
            }

            task.Id = id;
            var updated = await _repo.UpdateAsync(task, cancellationToken);

            if (updated != null && updated.Priority == Priority.High)
                _logger.LogCritical($"High priority task updated: {updated.Title}");

            return updated;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(id, cancellationToken);
        }
    }
}