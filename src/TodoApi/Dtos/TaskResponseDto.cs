namespace TodoApi.Dtos;

public class TaskResponseDto
{
    public Guid Id { get; set; }
    public string TaskDescription { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool Completed { get; set; }

}