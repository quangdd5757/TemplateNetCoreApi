using System.ComponentModel.DataAnnotations;

namespace TemplateNetCoreApi.Core.Models
{
    public class Teacher : BaseTimeEntity // model demo
    {

        [Required(ErrorMessage = "Teacher's name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }

        public string? Subject { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
