using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public class UpdateTaskDto
{
    public Guid Id { get; set; } //immutable 
    public DateTime CreatedDate { get; set;} //immutable

    [Required]
    public DateTime? DueDate { get; set;}

    private string _taskDescription = "";

    [Required]
    public string TaskDescription
    {
        get => _taskDescription;
        set => _taskDescription = value?.Trim() ?? "";
    }
    public bool Completed {get; set;}
}