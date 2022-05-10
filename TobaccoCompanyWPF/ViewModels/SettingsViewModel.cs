using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;
using TobaccoCompanyWPF.Views.Windows;

namespace TobaccoCompanyWPF.ViewModels
{
    public class SettingsViewModel : BaseWindowViewModel
    {
        private IDialogService dialogService;
        public SettingsViewModel(LoginViewModel loginPresenter)
        {
            this.LoginPresenter = loginPresenter;
            this.dialogService = new DefaultDialogService();

            this.TryConnectionCommand = new AsyncCommand(() => TryConnect());
            this.RestoreDataBaseCommand = new AsyncCommand(() => RestoreDataBase());
            this.SaveConnectionStringCommand = new Command(_ =>
            {
                if (UseOnlyServerName)
                {
                    TobaccoCompanyContext.DbConnStr = ConfigurationService.ConnectionString = $"Data Source={this.ConnectionString};Initial Catalog=Tobacco_Company;Integrated Security=True";
                }
                else
                {
                    TobaccoCompanyContext.DbConnStr = ConfigurationService.ConnectionString = this.ConnectionString;
                }
            });
        }

        public async Task RestoreDataBase()
        {
            var restoreSuccess = await DoActionWithLoadingAsync(() =>
            {
                var result = true;
                try
                {
                    TobaccoCompanyContext.DbConnStr = ConfigurationService.ConnectionString;
                    using var context = new TobaccoCompanyContext(true,true);
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            });
            if (restoreSuccess)
                this.dialogService.ShowMessage("База данных пересоздана");
            else
                this.dialogService.ShowMessage("Неудалось пересоздать базу данных. Проверьте введенную строку подключения, а затем сохраните настройки и повторите попытку");
        }

        public async Task TryConnect()
        {
            var canConnect = await DoActionWithLoadingAsync(() =>
            {
                if (UseOnlyServerName)
                {
                    TobaccoCompanyContext.DbConnStr = $"Data Source={this.ConnectionString};Initial Catalog=Tobacco_Company;Integrated Security=True";
                }
                else
                {
                    TobaccoCompanyContext.DbConnStr = this.ConnectionString;
                }
                using var context = new TobaccoCompanyContext(false);
                if (context.Database.CanConnect())
                { 
                    TobaccoCompanyContext.DbConnStr = ConfigurationService.ConnectionString;
                    return true;
                }
                else
                {
                    TobaccoCompanyContext.DbConnStr = ConfigurationService.ConnectionString;
                    return false;
                }
            });
            if (canConnect)
                this.dialogService.ShowMessage("Подключение удалось");
            else
                this.dialogService.ShowMessage("Подключение не удалось");
        }

        public LoginViewModel LoginPresenter { get; }

        public string ConnectionString { get; set; } = ConfigurationService.ConnectionString;

        public bool UseOnlyServerName { get; set; } = false;

        public new ICommand CloseWindowCommand => new Command(_ =>
        {
            CloseWindowAction?.Invoke();
            LoginPresenter.OpenWindowAction?.Invoke();
        });

        public ICommand TryConnectionCommand { get; }
        public ICommand SaveConnectionStringCommand { get; }
        public ICommand RestoreDataBaseCommand { get; }

    }
}
