using Microsoft.AspNetCore.Identity;
using PropertyChanged;
using System.Threading;
using System.Windows;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.CurrentPrincipal;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;
using TobaccoCompanyWPF.Views.Windows;

namespace TobaccoCompanyWPF.ViewModels
{
    public class LoginViewModel : BaseWindowViewModel
    {
        public ICommand OpenRegistrationWindowCommand => new Command(_ =>
        {
            this.HideWindowAction?.Invoke();

            var registrationWindow = new RegistrationWindow(this);
            registrationWindow.ShowDialog();
        });
        public ICommand OpenSettingsCommand => new Command(_ =>
        {
            this.HideWindowAction?.Invoke();

            var settingsWindow = new SettingsWindow(this);
            settingsWindow.ShowDialog();
        });
        public IAsyncCommand TryLoginCommand => new AsyncCommand(() => LoginTestAsync());

        public User User { get; set; }

        public IDialogService dialogService { get; set; }

        public LoginViewModel()
        {
            this.dialogService = new DefaultDialogService();
            this.User = new User();
        }

        private async Task LoginTestAsync()
        {
            var loginSucccess = false;
            try
            {
                loginSucccess = await DoActionWithLoadingAsync(() =>
                {
                    using var context = new TobaccoCompanyContext();
                    User? user = context.Users.Include(us => us.Roles).FirstOrDefault(user => user.Login == this.User.Login);
                    if (user != null)
                    {
                        var passwordHasher = new PasswordHasher<User>();
                        PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, this.User.Password);
                        if (passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                        {
                            //#error если выкенет ошибку, что навряд ли, то в текущее поле пароля попадет хеш
                            this.User = user;
                            return true;
                        }
                    }
                    return false;
                });
            }
            catch (Exception)
            {
                this.DialogHostLoadingIsOpen = false;
                this.dialogService.ShowMessage("Произошла ошибка взаимодействия с базой данных. Зайдите в настройки и удостовретесь в правильности подключения к базе данных");
                return;
            }
            
            if (!loginSucccess)
            {
                ShowErrorMessageAction?.Invoke("Неверное имя аккаунта или пароль. Пожалуйста, проверьте их и повторите попытку.");
            }
            else
            {
                this.HideWindowAction?.Invoke();
                var currentPrincipal = new CurrentPrincipal(this.User);

                NavigationStore navigationStore = new NavigationStore();
                var mainWindow = new MainWindow();
                var maindWindowViewModel = new MainViewModel(navigationStore, currentPrincipal, this)
                {
                    OpenWindowAction = () => mainWindow.Show(),
                    HideWindowAction = () => mainWindow.Hide(),
                    MinimizeWindowAction = () => SystemCommands.MinimizeWindow(mainWindow),
                    CloseWindowAction = shutDown => 
                    {
                        if (shutDown.HasValue && shutDown.Value)
                            System.Environment.Exit(0);
                        else
                        {
                            SystemCommands.CloseWindow(mainWindow);
                            this.OpenWindowAction?.Invoke();
                        }
                    },
                };
                mainWindow.DataContext = maindWindowViewModel;
                mainWindow.Show();
            }
        }
    }
}
