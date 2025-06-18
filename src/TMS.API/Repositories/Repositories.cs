using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(
            Priority? priority, 
            Status? status, 
            int skip, 
            int take, 
            CancellationToken cancellationToken)
        {
            var query = _context.Tasks.AsQueryable();

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            return await query
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tasks.FindAsync(id, cancellationToken);
        }

        public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);
            return task;
        }

        public async Task<TaskItem?> UpdateAsync(TaskItem task, CancellationToken cancellationToken)
        {
            var existing = await _context.Tasks.FindAsync(task.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(task);
            await _context.SaveChangesAsync(cancellationToken);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}