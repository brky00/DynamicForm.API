namespace DynamicForm.API.Dto
{
    public class FieldDto
    {
        public string Type { get; set; }
        public string Label { get; set; }
        public string? ExtraValue { get; set; }
        public bool Required { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
      
        public List<string>? Options { get; set; }
    }

}
