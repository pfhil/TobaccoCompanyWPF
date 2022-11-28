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
    public class OrderViewModel : BaseViewModel
    {
        public OrderViewModel(Order order)
        {
            this.Order=order;
            decimal sum = 0;
            Order.CashReceipts.ForEach(receipt => sum += receipt.Product.Price * receipt.Amount);
            this.SumPrice = sum;
        }

        private Order Order { get; }

        public int Id 
        {
            get => Order.Id;
            set => Order.Id = value;
        }

        public User Client
        {
            get => Order.Client;
            set => Order.Client = value;
        }

        public List<CashReceipt> CashReceipts => Order.CashReceipts;

        public DateTime Date
        {
            get => Order.Date;
            set => Order.Date= value;
        }

        public string? Address 
        {
            get => Order.Address;
            set => Order.Address = value;
        }

        public DateTime? DateReceived 
        {
            get => Order.DateReceived;
            set => Order.DateReceived = value;
        }

        public DateTime? RealDateReceived
        {
            get => Order.RealDateReceived;
            set => Order.RealDateReceived = value;
        }

        public bool UserMarkIsPaid 
        {
            get => Order.UserMarkIsPaid;
            set => Order.UserMarkIsPaid = value;
        }

        public bool UserMarkProductIsReceived
        {
            get => Order.UserMarkProductIsReceived;
            set => Order.UserMarkProductIsReceived = value;
        }

        public bool ManagerMarkIsPaid
        {
            get => Order.ManagerMarkIsPaid;
            set => Order.ManagerMarkIsPaid = value;
        }

        public bool ManagerMarkProductIsReceived
        {
            get => Order.ManagerMarkProductIsReceived;
            set => Order.ManagerMarkProductIsReceived = value;
        }

        public PaymentType PaymentType 
        {
            get => Order.PaymentType;
            set => Order.PaymentType = value;
        }

        public ReceivingMethod ReceivingMethod
        {
            get => Order.ReceivingMethod;
            set => Order.ReceivingMethod = value;
        }

        public decimal SumPrice { get; }

        public OrderState State 
        {
            get => Order.OrderState;
            set => Order.OrderState = value;
        }
    }
}
