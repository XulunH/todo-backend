using System.ComponentModel.DataAnnotations;
namespace TodoApi.Dtos;


public class CreateTaskDto
{   
    private string _taskDescription = "";

    [Required]
    public string TaskDescription
    {
        get => _taskDescription;
        set => _taskDescription = value?.Trim() ?? "";
    }

    [Required]
    public DateTime? DueDate { get; set;}
    public bool Completed { get; set; }
}