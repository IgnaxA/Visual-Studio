using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Initials { get; set; }

    }
}
