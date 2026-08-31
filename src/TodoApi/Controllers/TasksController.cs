using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? completed,
        [FromQuery(Name = "sort_by")] string? sortBy)
    {
        return Ok(await _service.GetAllAsync(completed, sortBy));
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id )
    {
        var task = await _service.GetByIdAsync(id);
        return task is null ? NotFound(NotFoundMessage(id)) : Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
    {
        if (dto.Id != Guid.Empty && dto.Id != id)
            return BadRequest(new ErrorResponse
            {
                Message = "The id in the request body does not match the id in the URL."
            });

        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(NotFoundMessage(id)) : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _service.DeleteAsync(id);
        return deleted is false ? NotFound(NotFoundMessage(id)) : Ok();
    }

    private static ErrorResponse NotFoundMessage(Guid id ) => new()
    {
        Message = $"Task with id {id} was not found." 
    };

}
