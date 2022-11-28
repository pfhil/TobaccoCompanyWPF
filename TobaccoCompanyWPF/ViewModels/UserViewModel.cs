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
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            this.User=user;

            this.IsManager = user.Roles.FirstOrDefault(role => role.Name == "Manager") != null;
            this.IsAdmin = user.Roles.FirstOrDefault(role => role.Name == "Admin") != null;
            this.IsSuperAdmin = user.Roles.FirstOrDefault(role => role.Name == "SuperAdmin") != null;
        }

        public User User { get; }

        public int Id 
        { 
            get => this.User.Id;
            set => this.User.Id = value;
        }

        public string Name
        {
            get => this.User.Name;
            set => this.User.Name = value;
        }

        public string Login 
        {
            get => this.User.Login;
            set => this.User.Login = value;
        }

        public DateTime DateBirth 
        {
            get => this.User.DateBirth;
            set => this.User.DateBirth = value;
        }

        public bool IsAdmin { get; set; }

        public bool IsManager { get; set; }

        public bool IsSuperAdmin { get; set; }
    }
}
