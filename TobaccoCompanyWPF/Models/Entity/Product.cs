using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.Models.Entity
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? FileName { get; set; }

        public List<Order> Orders { get; set; } = new();

        public List<CashReceipt> CashReceipts { get; set; }

        public List<Consignment> Consignments { get; set; }

        public List<Invoice> Invoices { get; set; } = new();

    }
}
