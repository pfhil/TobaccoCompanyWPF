using Microsoft.AspNetCore.Identity;
using PropertyChanged;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.Views.Windows;

namespace TobaccoCompanyWPF.ViewModels
{
    internal class RegistrationPresenter : PresenterBase
    {
        public RegistrationPresenter(LoginPresenter loginPresenter)
        {
            this.LoginPresenter = loginPresenter;
        }

        public LoginPresenter LoginPresenter { get; set; }

        public bool DialogHostIsOpen { get; set; } = false;

        public User User { get; set; } = new User();

        public IAsyncCommand TryRegistrationCommand => new AsyncCommand(() => TryRegistration());

        public async Task TryRegistration()
        {
            this.DialogHostIsOpen = true;

            using var context = await Task.Run(() => new TobaccoCompanyContext());
            User? user = await context.Users.FirstOrDefaultAsync(user => user.Login == this.User.Login);
            if (user == null)
            {
                var passwordHasher = new PasswordHasher<User>();
                this.User.Password = passwordHasher.HashPassword(this.User, this.User.Password);

                await context.Users.AddAsync(this.User);
                await context.SaveChangesAsync();
                this.DialogHostIsOpen = false;

                CloseWindowAction?.Invoke();
                LoginPresenter.OpenWindowAction?.Invoke();
            }
            else
            {
                this.DialogHostIsOpen = false;
                ShowErrorMessageAction?.Invoke("Пользователь с введенными учетными данными уже существует");
            }
        }

        public new ICommand CloseWindowCommand => new Command(_ =>
        {
            CloseWindowAction?.Invoke();
            LoginPresenter.OpenWindowAction?.Invoke();
        });
    }
}
