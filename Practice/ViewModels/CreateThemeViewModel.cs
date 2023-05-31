using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels
{
    public class CreateThemeViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Введите формулировку темы!")]
        [Display(Name="Формулировка темы")]
        public string ThemeFormulation { get; set; }


        [Required(ErrorMessage = "Введите ссылку на материалы!")]
        [Display(Name="Ссылка на материалы")]
        public string MaterialsLink { get; set; }
    }
}
