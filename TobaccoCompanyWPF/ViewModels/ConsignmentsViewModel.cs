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
    public class ConsignmentsViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;

        public ConsignmentsViewModel(NavigationStore navigationStore)
        {
            this.navigationStore=navigationStore;
            this.OpenCreateConsignmentViewCommand = new NavigateCommand<ConsignmentCreateViewModel>(
                new NavigationService<ConsignmentCreateViewModel>(navigationStore,
                () => new ConsignmentCreateViewModel(navigationStore)));
        }

        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context = new TobaccoCompanyContext();
                var consign = context.Consignments.Include(c => c.Invoices).ThenInclude(i => i.Product).ToList();
                ObservableCollection<ConsignmentStack> consignmentStacks = new ObservableCollection<ConsignmentStack>();
                consign.ForEach(consign => consignmentStacks.Add(new ConsignmentStack(consign)));
                this.Consignments = consignmentStacks;
            });
        }

        public ObservableCollection<ConsignmentStack> Consignments { get; set; }

        public ICommand OpenCreateConsignmentViewCommand { get; }
        public IAsyncCommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());
    }

    public class ConsignmentStack
    {
        public ConsignmentStack(Consignment consignment)
        {
            this.Id = consignment.Id;
            this.Date = consignment.Date;

            this.Invoices = "";

            consignment.Invoices.ForEach(invoice => this.Invoices += $"{invoice.Product.Name} - {invoice.Amount}\n");
        }
        public int Id { get; private set; }
        public DateTime Date { get; set; }
        public string Invoices { get; set; }
    }
}
