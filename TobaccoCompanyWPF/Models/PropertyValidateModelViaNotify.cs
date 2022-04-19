using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.Models
{
    public class PropertyValidateModelViaNotify : INotifyDataErrorInfo
    {
        private IDictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        private void Validate(object val, [CallerMemberName] string propertyName = null)
        {
            if (_errors.ContainsKey(propertyName)) _errors.Remove(propertyName);

            ValidationContext context = new ValidationContext(this) { MemberName = propertyName };
            List<ValidationResult> results = new();

            if (!Validator.TryValidateProperty(val, context, results))
            {
                _errors[propertyName] = results.Select(x => x.ErrorMessage).ToList();
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }
    }

    //public static class PropertyChangedNotificationInterceptor
    //{
    //    public static void Intercept(object target, Action onPropertyChangedAction,
    //                                 string propertyName, object before, object after)
    //    {
    //        onPropertyChangedAction();
    //    }
    //}
}
