using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.MVVM.Commands;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;

namespace TobaccoCompanyWPF.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;

        public ProductsViewModel(NavigationStore navigationStore)
        {
            this.DeleteProductCommand = new Command(deleteProduct);

            this.EditProductCommand = new Command(objProduct =>
            {
                var product = objProduct as Product;
                var navService = new ParameterNavigationService<Product, ProductEditViewModel>(
                    navigationStore,
                    product => new ProductEditViewModel(navigationStore, product));

                navService.Navigate(product!);
            });

            this.OpenCreateProductViewCommand = new NavigateCommand<ProductCreateViewModel>(
                new NavigationService<ProductCreateViewModel>(navigationStore,
                () => new ProductCreateViewModel(navigationStore)));

            this.navigationStore = navigationStore;
        }

        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context =  new TobaccoCompanyContext();
                this.EditProducts = new ObservableCollection<Product>(context.Products.ToList());
            });
        }

        private void deleteProduct(object? objProduct)
        {
            var product = objProduct as Product;
            this.AskQuestiionAsync($"Вы уверены, что хотите удалить \"{product!.Name}\"?",
                new Func<Task>(async () =>
                {
                    using TobaccoCompanyContext context = await Task.Run(() => new TobaccoCompanyContext());
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    this.EditProducts.Remove(product);
                }));
        }

        public ObservableCollection<Product> EditProducts { get; set; }

        public IAsyncCommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());
        public ICommand DeleteProductCommand { get; }
        public ICommand OpenCreateProductViewCommand { get; }
        public ICommand EditProductCommand { get; }
    }
}
