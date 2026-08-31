using System.ComponentModel.DataAnnotations;
namespace TodoApi.Dtos;


public class CreateTaskDto
{   
    [Required]
    public string TaskDescription { get; set; } = "";

    [Required]
    public DateTime? DueDate { get; set;}
    public bool Completed { get; set; }
}