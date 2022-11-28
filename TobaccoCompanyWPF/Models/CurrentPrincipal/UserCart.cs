using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoCompanyWPF.Models.Entity;

namespace TobaccoCompanyWPF.Models.CurrentPrincipal
{
    public class UserCart
    {
        public List<ProductStack> ProductStack = new List<ProductStack>();
    }

    public class ProductStack
    {
        public Product product { get; set; }

        public int Amount { get; set; }

        public ProductStack(Product product, int amount)
        {
            this.Amount = amount;
            this.product = product;
        }
    }
}
