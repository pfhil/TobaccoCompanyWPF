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

        public Courier? Courier { get; set; }

        public string? Address { get; set; }

        public DateTime Date { get; set; }

        public DateTime? DateOrderReceipt { get; set; }

        public bool IsPaid { get; set; }

        public bool ProductIsReceived { get; set; }

        public PaymentType PaymentType { get; set; }

        public TypeDelivery TypeDelivery { get; set; }

        public StateOrder StateOrder { get; set; }

        public List<Product> Products { get; set; } = new();

        public List<CashReceipt> CashReceipts { get; set; }

    }

    public enum PaymentType
    {
        Cash,
        BankCard
    }

    public enum TypeDelivery
    {
        Courier,
        Pickup
    }

    public enum StateOrder
    {
        Delivered,
        AwaitingReceipt,
        Executed
    }
}
