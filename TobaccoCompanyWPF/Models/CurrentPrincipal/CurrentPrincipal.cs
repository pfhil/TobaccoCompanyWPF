using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TobaccoCompanyWPF.Models.Entity;

namespace TobaccoCompanyWPF.Models.CurrentPrincipal
{
    public class CurrentPrincipal
    {
        public CurrentPrincipal(User user)
        {
            this.User = user;
        }

        public UserCart UserCart { get; set; } = new UserCart();
        public User User { get; set; }

        public bool IsInRole(string role) => User?.Roles.FirstOrDefault(rol => rol.Name == role) != null;
    }
}
