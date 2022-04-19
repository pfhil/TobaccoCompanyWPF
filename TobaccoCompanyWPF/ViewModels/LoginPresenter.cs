using Microsoft.AspNetCore.Identity;
using PropertyChanged;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.Views.Windows;

namespace TobaccoCompanyWPF.ViewModels
{
    public class LoginPresenter : PresenterBase
    {
        public ICommand OpenRegistrationWindowCommand => new Command(_ =>
        {
            this.HideWindowAction?.Invoke();

            var registrationWindow = new RegistrationWindow(this);
            registrationWindow.ShowDialog();
        });
        public IAsyncCommand TryLoginCommand => new AsyncCommand(() => LoginTestAsync());

        public bool DialogHostIsOpen { get; set; } = false;
        public User User { get; set; }

        public LoginPresenter()
        {
            this.User = new User();
            User.Login = "";
            User.Password = "";
        }

        private async Task LoginTestAsync()
        {
            this.DialogHostIsOpen = true;

            using var context = await Task.Run(() => new TobaccoCompanyContext());
            User? user = await context.Users.FirstOrDefaultAsync(user => user.Login == this.User.Login);
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, this.User.Password);
                this.DialogHostIsOpen = false;
                if (passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    ShowErrorMessageAction?.Invoke("Введен неправильный пароль");
                }
            }
            else
            {
                this.DialogHostIsOpen = false;
                ShowErrorMessageAction?.Invoke("Аккаунта с введенным именем не существует");
            }
        }
    }
}
