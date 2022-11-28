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
    public class UserOrdersViewModel : BaseViewModel
    {
        public UserOrdersViewModel(CurrentPrincipal currentPrincipal)
        {
            this.CurrentPrincipal=currentPrincipal;

            this.GetProductCommand = new AsyncCommand<object>(async orderObject =>
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    using TobaccoCompanyContext context = new TobaccoCompanyContext();
                    var order = context.Orders.First(order => order.Id == (orderObject as OrderViewModel).Id);

                    order.UserMarkProductIsReceived = !order.UserMarkProductIsReceived;
                    context.Orders.Update(order);
                    context.SaveChanges();
                });
            });

            this.PayProductCommand = new AsyncCommand<object>(async orderObject =>
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    using TobaccoCompanyContext context = new TobaccoCompanyContext();
                    var order = context.Orders.First(order => order.Id == (orderObject as OrderViewModel).Id);

                    order.UserMarkIsPaid = !order.UserMarkIsPaid;
                    context.Orders.Update(order);
                    context.SaveChanges();
                });
            });
        }

        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context = new TobaccoCompanyContext();

                var orders = context.Orders
                        .Where(order => order.Client.Id == this.CurrentPrincipal.User.Id)
                        .Include(order => order.CashReceipts)
                        .ThenInclude(cashReceipt => cashReceipt.Product)
                        .ToList();

                var tempOrders = new ObservableCollection<OrderViewModel>();

                orders.ForEach(order => tempOrders.Add(new OrderViewModel(order)));

                this.Orders = new ObservableCollection<OrderViewModel>(tempOrders);
            });
        }

        public ObservableCollection<OrderViewModel> Orders { get; set; }

        public IAsyncCommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());

        public CurrentPrincipal CurrentPrincipal { get; }

        public ICommand GetProductCommand { get; }
        public ICommand PayProductCommand { get; }
    }
}
