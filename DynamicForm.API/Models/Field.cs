namespace DynamicForm.API.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string? ExtraValue { get; set; } //only for checkbox
        public bool Required { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }

        public List<string>? Options { get; set; }

        // Foreignkey
        public int FormId { get; set; }
   
    }


}
