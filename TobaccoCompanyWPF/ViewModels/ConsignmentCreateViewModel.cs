using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.MVVM.Commands;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;

namespace TobaccoCompanyWPF.ViewModels
{
    public class ConsignmentCreateViewModel : BaseViewModel
    {
        public ConsignmentCreateViewModel(NavigationStore navigationStore)
        {
            var navigationService = new NavigationService<ConsignmentsViewModel>(
                navigationStore,
                () => new ConsignmentsViewModel(navigationStore));
            
            this.AddInvoiceCommand = new Command(_ =>
            {
                var removedProduct = this.Products.First(product => product.Name == this.SelectedProduct);
                var invoice = new Invoice()
                {
                    Amount = this.Amount,
                    Product = removedProduct,
                };
                this.Invoices.Add(invoice);
                this.Products.Remove(removedProduct);
            });

            this.DeleteInvoiceCommand = new Command(invoiceObj =>
            {
                var invoice = invoiceObj as Invoice;
                this.Invoices.Remove(invoice);
                this.Products.Add(invoice.Product);
            });

            this.CreateConsignmentCommand = new AsyncCommand(() => CreateConsignmentAsync(navigationService));

            this.CancelCommand = new Command(_ => navigationService.Navigate());
        }

        private async Task CreateConsignmentAsync(NavigationService<ConsignmentsViewModel> navigationService)
        {
            if (this.ValidateSelf())
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    using TobaccoCompanyContext context = new TobaccoCompanyContext();

                    Consignment consignment = new Consignment();
                    consignment.Date = DateTime.Now;

                    foreach (var invoice in this.Invoices)
                    {
                        invoice.Product = context.Products.First(product => product.Name == invoice.Product.Name);
                    }
                    consignment.Invoices = this.Invoices.ToList();

                    context.Consignments.Add(consignment);
                    context.SaveChanges();
                });
                navigationService.Navigate();
            }
        }

        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context = new TobaccoCompanyContext();
                this.Products = new ObservableCollection<Product>(context.Products.ToList());
            });
        }

        public ObservableCollection<Invoice> Invoices { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; }
        public string SelectedProduct { get; set; }
        public int Amount { get; set; } = 1;

        public ICommand AddInvoiceCommand { get; }
        public ICommand CreateConsignmentCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteInvoiceCommand { get; }
        public IAsyncCommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());
    }
}
