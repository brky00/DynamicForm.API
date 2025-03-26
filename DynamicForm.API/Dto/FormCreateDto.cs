using DynamicForm.API.Models;

namespace DynamicForm.API.Dto
{
    public class FormCreateDto
    {
        public string Title { get; set; }
        public List<FieldDto> Fields { get; set; }
    }
}
