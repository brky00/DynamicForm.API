namespace DynamicForm.API.Models.SubmissionFolder
{
    public class Submission
    {
        public int Id { get; set; }
        public int FormId { get; set; }
     
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public List<SubmissionAnswer> Answers { get; set; } = new();
    }
}
