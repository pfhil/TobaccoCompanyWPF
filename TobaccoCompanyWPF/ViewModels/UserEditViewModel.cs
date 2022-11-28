using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.CurrentPrincipal;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;
using TobaccoCompanyWPF.ViewModels.ValidationAttributes;

namespace TobaccoCompanyWPF.ViewModels
{
    public class UserEditViewModel : BaseViewModel
    {
        public UserEditViewModel(CurrentPrincipal currentPrincipal)
        {
            this.CurrentPrincipal=currentPrincipal;

            this.dialogService = new DefaultDialogService();

            this.UpdateUserCommand = new AsyncCommand(async () =>
            {
                if (this.ValidateSelf())
                {
                    await this.DoActionWithLoadingAsync(() =>
                    {
                        using var context = new TobaccoCompanyContext();

                        var userDB = context.Users.First(user => user.Id == this.CurrentPrincipal.User.Id);

                        var passwordHasher = new PasswordHasher<User>();
                        userDB.Password = passwordHasher.HashPassword(userDB, this.Password);
                        userDB.Name = this.CurrentPrincipal.User.Name = this.Name;
                        userDB.DateBirth = this.CurrentPrincipal.User.DateBirth = this.DateBirth;
                        userDB.Phone = this.CurrentPrincipal.User.Phone = this.Phone;
                        
                        context.Users.Update(userDB);
                        context.SaveChanges();
                    });
                    this.dialogService.ShowMessage("Учетные данные успешно изменены");
                }
            });
        }

        private string password;

        [Required(ErrorMessage = "Пароль - обязательное поле")]
        [MinLength(8, ErrorMessage = "Минимальная длинна пароля - 8 символов")]
        public string Password
        {
            get => this.password;
            set
            {
                this.password = value;
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
            get => this.CurrentPrincipal.User.Name;
            set
            {
                this.CurrentPrincipal.User.Name = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Телефонный номер - обязательное поле")]
        [Phone(ErrorMessage = "Вводимое значение должно быть телефонным номером")]
        public string Phone
        {
            get => this.CurrentPrincipal.User.Phone;
            set
            {
                this.CurrentPrincipal.User.Phone = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Дата рождения - обязательное поле")]
        [DateRange(-120, -18, ErrorMessage = "Пользователь должен быть совершеннолетним")]
        public DateTime DateBirth
        {
            get => this.CurrentPrincipal.User.DateBirth;
            set
            {
                this.CurrentPrincipal.User.DateBirth = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        public CurrentPrincipal CurrentPrincipal { get; }

        public IDialogService dialogService { get; set; }

        public ICommand UpdateUserCommand { get; }
    }
}
