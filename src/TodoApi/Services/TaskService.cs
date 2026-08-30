using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Services;


public class TaskService : ITaskService
{
    private readonly AppDbContext _db;
    public TaskService(AppDbContext db) => _db = db;

    public static IQueryable<TaskItem> ApplySort(IQueryable<TaskItem> query, string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query.OrderBy(t => t.CreatedDate);

        var raw = sortBy.Trim();
        var descending = raw.StartsWith('-');
        var field = raw.TrimStart('+','-').Trim();

        return (field.ToLowerInvariant(), descending) switch
        {

            ("duedate", false)     => query.OrderBy(t => t.DueDate),
            ("duedate", true)      => query.OrderByDescending(t => t.DueDate),
            ("createddate", false) => query.OrderBy(t => t.CreatedDate),
            ("createddate", true)  => query.OrderByDescending(t => t.CreatedDate),
            _                      => query.OrderBy(t => t.CreatedDate)
        };
    }
    private static TaskResponseDto ToDto(TaskItem t) => new()
    {
        Id = t.Id,
        TaskDescription = t.TaskDescription,
        CreatedDate = t.CreatedDate,
        DueDate = t.DueDate,
        Completed = t.Completed
    };
    

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(bool? completed, string? sortBy)
    {
        IQueryable<TaskItem> query = _db.Tasks;

        if(completed.HasValue)
        {
            query = query.Where(t => t.Completed == completed.Value);

        }
        var items = await ApplySort(query, sortBy).ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<TaskResponseDto?> GetByIdAsync(Guid id)
    {
        var item= await _db.Tasks.FindAsync(id);

        return item is null ? null : ToDto(item);       
    } 

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        var item = new TaskItem
        {
            Id = Guid.NewGuid(),
            TaskDescription = dto.TaskDescription,
            CreatedDate = DateTime.UtcNow,
            DueDate = dto.DueDate,
            Completed = dto.Completed

        };

        _db.Tasks.Add(item);
        await _db.SaveChangesAsync();
        return ToDto(item);
    }

    public async Task<TaskResponseDto?> UpdateAsync(Guid id, UpdateTaskDto dto)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item is null) return null;

        item.TaskDescription = dto.TaskDescription;
        item.DueDate = dto.DueDate;
        item.Completed = dto.Completed;

        await _db.SaveChangesAsync();
        return ToDto(item);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _db.Tasks.FindAsync(id);
        
        if (item is null) return false;

        _db.Tasks.Remove(item);
        await _db.SaveChangesAsync();
        return true;

    }
}