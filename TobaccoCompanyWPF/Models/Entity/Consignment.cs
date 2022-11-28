using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.Models.Entity
{
    public class Consignment
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public List<Product> Products { get; set; } = new();

        public List<Invoice> Invoices { get; set; } = new();

    }
}
