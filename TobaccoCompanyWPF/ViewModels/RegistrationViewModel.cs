using Microsoft.AspNetCore.Identity;
using PropertyChanged;
using System.Runtime.CompilerServices;
using System.Threading;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.ValidationAttributes;
using TobaccoCompanyWPF.Views.Windows;

namespace TobaccoCompanyWPF.ViewModels
{
    internal class RegistrationViewModel : BaseWindowViewModel
    {
        public IDialogService dialogService { get; set; }

        public RegistrationViewModel(LoginViewModel loginPresenter)
        {
            this.dialogService = new DefaultDialogService();
            this.LoginPresenter = loginPresenter;
        }

        public LoginViewModel LoginPresenter { get; set; }

        private User User { get; set; } = new ();

        [Required(ErrorMessage = "Логин - обязательное поле")]
        public string Login 
        {
            get => this.User.Login;
            set
            {
                this.User.Login = value;
                OnPropertyChanged();
                ValidateProperty(value);
                //base.ValidatePropertyWithCustomPredicateAsync(value, AccountExist, "Такой логин уже используется");
            }
        }

        [Required(ErrorMessage = "Телефонный номер - обязательное поле")]
        [Phone(ErrorMessage = "Вводимое значение должно быть телефонным номером")]
        public string Phone 
        {
            get => this.User.Phone;
            set
            {
                this.User.Phone = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Пароль - обязательное поле")]
        [MinLength(8, ErrorMessage = "Минимальная длинна пароля - 8 символов")]
        public string Password 
        {
            get => this.User.Password;
            set
            {
                this.User.Password = value;
                OnPropertyChanged();
                ValidateProperty(value);
                ValidateProperty(confirmPassword, nameof(this.ConfirmPassword));
            }
        }

        private string confirmPassword;

        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string ConfirmPassword 
        {
            get => confirmPassword;
            set
            {
                this.confirmPassword = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Имя - обязательное поле")]
        public string Name 
        {
            get => this.User.Name;
            set
            {
                this.User.Name = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Дата рождения - обязательное поле")]
        [DateRange(-120, -18, ErrorMessage = "Пользователь должен быть совершеннолетним")]
        public DateTime DateBirth 
        {
            get => this.User.DateBirth;
            set
            {
                this.User.DateBirth = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }


        public IAsyncCommand TryRegistrationCommand => new AsyncCommand(() => TryRegistrationAsync());

        private bool AccountExist(object value)
        {
            if (value != null && value is string login)
            {
                using var context = new TobaccoCompanyContext();
                return context.Users.FirstOrDefault(user => user.Login == login) != null;
            }
            else
            {
                return false;
            }
        }

        public async Task TryRegistrationAsync()
        {
            var dict = new Dictionary<string, List<(Predicate<object> predicate, object propValue, string errorText)>>();
            dict.Add(
                nameof(this.Login),
                new List<(Predicate<object> predicate, object propValue, string errorText)>
                {
                    (AccountExist, this.Login, "Такой логин уже используется"),
                });

            var validate = false;
            try
            {
                validate = await this.DoActionWithLoadingAsync(() => this.ValidateSelf(dict));
            }
            catch (Exception)
            {
                this.DialogHostLoadingIsOpen = false;
                this.dialogService.ShowMessage("Произошла ошибка взаимодействия с базой данных. Зайдите в настройки и удостовретесь в правильности подключения к базе данных");
                return;
            }
            if (validate)
            {
                await DoActionWithLoadingAsync(() =>
                {
                    using var context = new TobaccoCompanyContext();
                    var passwordHasher = new PasswordHasher<User>();
                    this.User.Password = passwordHasher.HashPassword(this.User, this.User.Password);

                    context.Users.Add(this.User);
                    context.SaveChanges();
                });

                CloseWindowAction?.Invoke(null);
                LoginPresenter.OpenWindowAction?.Invoke();
            }
        }

        public new ICommand CloseWindowCommand => new Command(_ =>
        {
            CloseWindowAction?.Invoke(null);
            LoginPresenter.OpenWindowAction?.Invoke();
        });
    }
}
