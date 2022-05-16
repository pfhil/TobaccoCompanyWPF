using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using TobaccoCompanyWPF.Models.CurrentPrincipal;
using TobaccoCompanyWPF.Models.Services;

namespace TobaccoCompanyWPF.Views.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var principal = value as CurrentPrincipal;
            if (principal != null)
            {
                return principal.IsInRole((string)parameter) ? Visibility.Visible : Visibility.Collapsed;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
