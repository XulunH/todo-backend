using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public class UpdateTaskDto
{
    public Guid Id { get; set; } //immutable 
    public DateTime CreatedDate { get; set;} //immutable

    [Required]
    public DateTime? DueDate { get; set;}

    [Required]
    public string TaskDescription { get; set; } = "";
    public bool Completed {get; set;}
}