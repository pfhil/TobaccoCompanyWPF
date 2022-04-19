using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.Models.Entity
{
    public class Invoice
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Consignment Consignment { get; set; }
        public int ConsignmentId { get; set; }

        public int Amount { get; set; }
    }
}
