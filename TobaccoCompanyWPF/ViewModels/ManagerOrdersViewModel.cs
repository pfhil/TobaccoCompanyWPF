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
    public class ManagerOrdersViewModel : BaseViewModel
    {
        public ManagerOrdersViewModel()
        {
            this.GetProductCommand = new AsyncCommand<object>(async orderObject =>
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    using TobaccoCompanyContext context = new TobaccoCompanyContext();
                    var order = context.Orders.First(order => order.Id == (orderObject as OrderViewModel).Id);

                    order.ManagerMarkProductIsReceived = !order.ManagerMarkProductIsReceived;
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

                    order.ManagerMarkIsPaid = !order.ManagerMarkIsPaid;
                    context.Orders.Update(order);
                    context.SaveChanges();
                });
            });

            this.CloseOrderCommand = new AsyncCommand<object>(async orderObject =>
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    using TobaccoCompanyContext context = new TobaccoCompanyContext();
                    var order = context.Orders.First(order => order.Id == (orderObject as OrderViewModel).Id);

                    order.OrderState = OrderState.Close;
                    context.Orders.Update(order);
                    context.SaveChanges();
                });

                (orderObject as OrderViewModel).State = OrderState.Close;
            });
        }
        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context = new TobaccoCompanyContext();

                var orders = context.Orders
                        .Include(order => order.CashReceipts)
                        .ThenInclude(cashReceipt => cashReceipt.Product)
                        .Include(order => order.Client)
                        .ToList();

                var tempOrders = new ObservableCollection<OrderViewModel>();

                orders.ForEach(order => tempOrders.Add(new OrderViewModel(order)));

                this.Orders = new ObservableCollection<OrderViewModel>(tempOrders);
            });
        }

        public ObservableCollection<OrderViewModel> Orders { get; set; }

        public IAsyncCommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());
        public ICommand GetProductCommand { get; }
        public ICommand PayProductCommand { get; }
        public ICommand CloseOrderCommand { get; }
    }
}
