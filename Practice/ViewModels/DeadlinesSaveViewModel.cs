using System.ComponentModel.DataAnnotations;

namespace Practice.ViewModels
{
    public class DeadlinesSaveViewModel
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Введите корректную дату дедлайна!")]
        [Display(Name = "Дата дедлайна")]
        [CustomValidation(typeof(DeadlinesSaveViewModel), "ValidateDeadlineDate")]
        public DateTime DealineDate { get; set; }


        [Required(ErrorMessage = "Введите комментарий к дедлайну!")]
        [Display(Name = "Комментарий к дедлайну")]
        public string Commentary { get; set; }

        [Required(ErrorMessage="Отметьте статус дедлайна!")]
        [Display(Name = "Статус дедлайна")]
        public int AttendanceMark { get; set; }

        public static ValidationResult ValidateDeadlineDate(DateTime deadlineDate, ValidationContext context)
        {
            if (deadlineDate <= DateTime.Now)
            {
                return new ValidationResult("Дата дедлайна должна быть позже текущей даты!");
            }

            return ValidationResult.Success;
        }
    }
}
