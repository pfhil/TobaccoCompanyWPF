using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.Models.Entity
{
    public class Order
    {
        public int Id { get; set; }

        public User Client { get; set; }

        public string? Address { get; set; }

        public DateTime Date { get; set; }

        public DateTime? DateReceived { get; set; }

        public DateTime? RealDateReceived { get; set; }

        public bool UserMarkIsPaid { get; set; }

        public bool UserMarkProductIsReceived { get; set; }

        public bool ManagerMarkIsPaid { get; set; }

        public bool ManagerMarkProductIsReceived { get; set; }

        public PaymentType PaymentType { get; set; }

        public ReceivingMethod ReceivingMethod { get; set; }

        public OrderState OrderState { get; set; }

        public List<Product> Products { get; set; } = new();

        public List<CashReceipt> CashReceipts { get; set; } = new();

    }

    public enum PaymentType
    {
        [Description("Наличными")]
        Cash,
        [Description("Банковской картой")]
        BankCard
    }

    public enum ReceivingMethod
    {
        [Description("Доставка")]
        Delivery,
        [Description("Самовывоз")]
        Pickup
    }

    public enum OrderState
    {
        [Description("Открыт")]
        Open,
        [Description("Закрыт")]
        Close
    }
}
