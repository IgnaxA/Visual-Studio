using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Название команды:")]
        public int TeamId { get; set; }

        [Required]
        [Display(Name="Инициалы студента:")]
        public string Initials { get; set; }

        [Required]
        [Display(Name="Email студента:")]
        public string Email { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int? RoleId { get; set; }

        public int ThemeId { get; set; }
    }
}
