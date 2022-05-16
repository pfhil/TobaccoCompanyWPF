using TobaccoCompanyWPF.Models.CurrentPrincipal;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.MVVM.Commands;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;

namespace TobaccoCompanyWPF.ViewModels
{
    public class MainViewModel : BaseWindowViewModel
    {
        private readonly NavigationStore navigationStore;

        public BaseViewModel? CurrentViewModel => this.navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore, CurrentPrincipal currentUser, LoginViewModel loginPresenter)
        {
            this.navigationStore = navigationStore;
            this.CurrentUser=currentUser;
            this.LoginPresenter=loginPresenter;
            this.LogOutCommand = new Command(_ =>
            {
                this.CloseWindowAction?.Invoke(false);
                loginPresenter.OpenWindowAction?.Invoke();
            });

            this.NavigateProductsCommand = new NavigateCommand<ProductsViewModel>(
                new NavigationService<ProductsViewModel>(navigationStore,
                () => new ProductsViewModel(navigationStore)));

            this.NavigateCatalogCommand = new NavigateCommand<CatalogViewModel>(
                new NavigationService<CatalogViewModel>(navigationStore,
                () => new CatalogViewModel(this.CurrentUser)));

            this.NavigateConsignmentsCommand = new NavigateCommand<ConsignmentsViewModel>(
                new NavigationService<ConsignmentsViewModel>(navigationStore,
                () => new ConsignmentsViewModel(navigationStore)));

            this.NavigatePlaceOrderCommand = new NavigateCommand<PlaceOrderViewModel>(
                new NavigationService<PlaceOrderViewModel>(navigationStore,
                () => new PlaceOrderViewModel(this.CurrentUser, navigationStore)));

            this.NavigateUserOrdersCommand = new NavigateCommand<UserOrdersViewModel>(
                new NavigationService<UserOrdersViewModel>(navigationStore,
                () => new UserOrdersViewModel(this.CurrentUser)));

            this.NavigateManagerOrdersCommand = new NavigateCommand<ManagerOrdersViewModel>(
                new NavigationService<ManagerOrdersViewModel>(navigationStore,
                () => new ManagerOrdersViewModel()));

            this.NavigateAdminViewCommand = new NavigateCommand<AdminViewModel>(
                new NavigationService<AdminViewModel>(navigationStore,
                () => new AdminViewModel(this.CurrentUser)));

            this.NavigateUserEditCommand = new NavigateCommand<UserEditViewModel>(
                new NavigationService<UserEditViewModel>(navigationStore,
                () => new UserEditViewModel(this.CurrentUser)));



            this.navigationStore.CurrentViewModelChanged += () => base.OnPropertyChanged(nameof(this.CurrentViewModel));
        }

        public CurrentPrincipal CurrentUser { get; set; }
        public LoginViewModel LoginPresenter { get; }
        public ICommand NavigateUserOrdersCommand { get; }
        public ICommand NavigateProductsCommand { get; }
        public ICommand NavigateOrdersCommand { get; }
        public ICommand NavigateCatalogCommand { get; }
        public ICommand NavigateConsignmentsCommand { get; }
        public ICommand NavigatePlaceOrderCommand { get; }
        public ICommand NavigateManagerOrdersCommand { get; }
        public ICommand NavigateAdminViewCommand { get; }
        public ICommand NavigateUserEditCommand { get; }
        public ICommand LogOutCommand { get; }
        public new ICommand CloseWindowCommand => new Command(_ => CloseWindowAction?.Invoke(true));
    }
}
