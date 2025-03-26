
namespace DynamicForm.API.Models.SubmissionFolder
{
    public class SubmissionAnswer
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
  
        public int FieldId { get; set; }
        public string Value { get; set; }
    }
}
