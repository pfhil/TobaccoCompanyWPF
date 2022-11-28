using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.ViewModels.Services
{
    public static class ConfigurationService
    {
        public static string ConnectionString 
        {
            get => GeneralSettings.Default.ConnectionString;
            set
            {
                GeneralSettings.Default.ConnectionString = value;
                GeneralSettings.Default.Save();
            }
        }
    }
}
