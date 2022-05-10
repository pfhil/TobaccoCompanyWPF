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
        private readonly CurrentPrincipal currentUser;

        public BaseViewModel? CurrentViewModel => this.navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore, CurrentPrincipal currentUser)
        {
            this.navigationStore = navigationStore;
            this.currentUser=currentUser;
            this.NavigateProductsCommand = new NavigateCommand<ProductsViewModel>(
                new NavigationService<ProductsViewModel>(navigationStore,
                () => new ProductsViewModel(navigationStore)));

            this.NavigateCatalogCommand = new NavigateCommand<CatalogViewModel>(
                new NavigationService<CatalogViewModel>(navigationStore,
                () => new CatalogViewModel(this.currentUser)));

            this.NavigateConsignmentsCommand = new NavigateCommand<ConsignmentsViewModel>(
                new NavigationService<ConsignmentsViewModel>(navigationStore,
                () => new ConsignmentsViewModel(navigationStore)));

            this.NavigatePlaceOrderCommand = new NavigateCommand<PlaceOrderViewModel>(
                new NavigationService<PlaceOrderViewModel>(navigationStore,
                () => new PlaceOrderViewModel(this.currentUser)));

            this.navigationStore.CurrentViewModelChanged += () => base.OnPropertyChanged(nameof(this.CurrentViewModel));
        }

        public CurrentPrincipal MyProperty { get; set; }

        public ICommand NavigateProductsCommand { get; }
        public ICommand NavigateOrdersCommand { get; }
        public ICommand NavigateCatalogCommand { get; }
        public ICommand NavigateConsignmentsCommand { get; }
        public ICommand NavigatePlaceOrderCommand { get; }
    }
}
