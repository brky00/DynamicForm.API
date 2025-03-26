using DynamicForm.API.Models.SubmissionFolder;
namespace DynamicForm.API.Models;

public class Form
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string UserId { get; set; }
    public List<Field> Fields { get; set; } = new();
    public List<Submission> Submissions { get; set; } = new();
}
