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
    public class AdminViewModel : BaseViewModel
    {
        public AdminViewModel(CurrentPrincipal currentPrincipal)
        {
            this.CurrentPrincipal=currentPrincipal;

            this.ToggleManagerRoleCommand = new AsyncCommand<object>(async userObject =>
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    ToggleRole("Manager", userObject!);
                });
            });

            this.ToggleAdminRoleCommand = new AsyncCommand<object>(async userObject =>
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    ToggleRole("Admin", userObject!);
                });
            });

        }

        private void ToggleRole(string RoleName, object userObject)
        {
            using TobaccoCompanyContext context = new TobaccoCompanyContext();

            var user = context.Users.Include(user => user.Roles).First(user => user.Id == (userObject as UserViewModel).Id);
            var managerRole = user.Roles.FirstOrDefault(role => role.Name == RoleName);

            if (managerRole != null)
            {
                user.Roles.Remove(managerRole);
            }
            else
            {
                user.Roles.Add(context.Roles.First(role => role.Name == RoleName));
            }
            context.Users.Update(user);
            context.SaveChanges();
        }

        public ObservableCollection<UserViewModel> Users { get; set; }

        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context = new TobaccoCompanyContext();

                var tempUsers = new ObservableCollection<UserViewModel>();
                context.Users.Include(user => user.Roles).Where(user => user.Id != this.CurrentPrincipal.User.Id).ToList().ForEach(user => tempUsers.Add(new UserViewModel(user)));

                this.Users = tempUsers;
            });
        }

        public IAsyncCommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());
        public ICommand ToggleManagerRoleCommand { get; }
        public ICommand ToggleAdminRoleCommand { get; }

        public CurrentPrincipal CurrentPrincipal { get; }
    }
}
