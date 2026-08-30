using TodoApi.Dtos;

namespace TodoApi.Services;


public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllAsync(bool? completed, string? sortBy);
    Task<TaskResponseDto?> GetByIdAsync(Guid id);
    Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
    Task<TaskResponseDto?> UpdateAsync(Guid id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(Guid id);


}