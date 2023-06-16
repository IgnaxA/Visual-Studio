using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels
{
    public class ConsultationSaveViewModel
    {
        public int Id { get; set; }

        [Required]
        public int DeadlineId { get; set; }

        [Required(ErrorMessage = "Введите корректную дату дедлайна!")]
        [Display(Name = "Дата дедлайна")]
        [CustomValidation(typeof(DeadlinesSaveViewModel), "ValidateDeadlineDate")]
        public DateTime ConsultationDate { get; set; }

        [Required(ErrorMessage = "Отметьте статус дедлайна!")]
        [Display(Name = "Статус дедлайна")]
        public int AttendanceMark { get; set; }

        public static ValidationResult ValidateDeadlineDate(DateTime deadlineDate, ValidationContext context)
        {
            if (deadlineDate <= DateTime.Now)
            {
                return new ValidationResult("Дата консультации должна быть позже текущей даты!");
            }

            return ValidationResult.Success;
        }
    }
}
