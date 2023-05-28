using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels
{
    public class CreateThemeViewModel
    {
        public int teacherId { get; set; }

        public int id { get; set; }

        [Required(ErrorMessage = "Введите формулировку темы!")]
        [Display(Name="Формулировка темы")]
        public string ThemeFormulation { get; set; }
    }
}
