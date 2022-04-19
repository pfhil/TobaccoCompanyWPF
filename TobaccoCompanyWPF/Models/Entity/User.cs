using PropertyChanged;
using System.Collections;
using System.ComponentModel;

namespace TobaccoCompanyWPF.Models.Entity
{
    public class User : PropertyValidateModel, INotifyPropertyChanged
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Логин - обязательное поле")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль - обязательное поле")]
        [MinLength(8, ErrorMessage = "Минимальная длинна пароля - 8 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Имя - обязательное поле")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата рождения - обязательное поле")]
        [Date(ErrorMessage = "Пользователь должен быть совершеннолетним")]
        public DateTime DateBirth { get; set; } = new DateTime(2000, 1, 1);

        public List<Role> Roles { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Order> Orders { get; set; } = new();

    }

    public class DateAttribute : RangeAttribute
    {
        public DateAttribute()
          : base(typeof(DateTime), DateTime.Now.AddYears(-120).ToShortDateString(), DateTime.Now.AddYears(-18).ToShortDateString()) { }
    }
}
