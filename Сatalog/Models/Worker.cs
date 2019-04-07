using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Сatalog.Enum;

namespace Сatalog.Models
{
    [Table("Workers")]
    public partial class Worker
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Недопустимый год")]
        [Display(Name = "Номер подразделения")]
        public int DeptId { get; set; }

        [Required(ErrorMessage = "Укажите ФИО сотрудника")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Должность")]
        public int PositionId { get; set; }

        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Некорректный номер телефона")]
        [Display(Name = "Телефонный номер")]
        public string TelephoneNumber { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        

        // TODO: enum
        [Display(Name = "Пол")]
        public int GenderType { get; set; }
    }
}
