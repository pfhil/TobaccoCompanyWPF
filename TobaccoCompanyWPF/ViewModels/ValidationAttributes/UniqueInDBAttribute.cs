using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.ViewModels.ValidationAttributes
{
    public class UniqueInDBAttribute : ValidationAttribute
    {
        private readonly Predicate<object?> predicate;

        public UniqueInDBAttribute(Predicate<object?> predicate)
        {
            this.predicate = predicate;
        }

        public override bool IsValid(object? value) => predicate(value);
    }
}
