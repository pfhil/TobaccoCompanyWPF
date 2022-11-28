using System.Runtime.CompilerServices;

namespace TobaccoCompanyWPF.Models
{
    public class PropertyValidateModelViaNotify : INotifyDataErrorInfo
    {
        protected IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        protected virtual bool ValidateSelf(IDictionary<string, List<(Predicate<object> validationRule, object propValue, string errorText)>> clientValidationRules)
        {
            errors.Clear();
            var ValidationSuccess = ValidateSelfUsingDefaultValidation();

            var properties = this
                .GetType()
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(ValidationAttribute), false))
                .Select(propInfo => propInfo.Name)
                .ToList();

            foreach (var validationRule in clientValidationRules)
            {
                if (!errors.ContainsKey(validationRule.Key))
                {
                    foreach (var value in validationRule.Value)
                    {
                        if (value.validationRule(value.propValue))
                        {
                            errors[validationRule.Key] = new List<string>();
                            errors[validationRule.Key].Add(value.errorText);
                            if (properties.FirstOrDefault(propName => propName == validationRule.Key) == null)
                            {
                                properties.Add(validationRule.Key);
                            }
                            ValidationSuccess = false;
                        }
                    }
                }
            }

            properties.ForEach(prop => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop)));

            return ValidationSuccess;
        }

        private bool ValidateSelfUsingDefaultValidation()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();
            var ValidationSuccess = true;
            if (!Validator.TryValidateObject(this, context, results, true))
            {
                foreach (var error in results)
                {
                    var propNameWithError = error.MemberNames.First();
                    if (!errors.ContainsKey(propNameWithError))
                    {
                        errors.Add(propNameWithError, new List<string>());
                    }
                    errors[propNameWithError].Add(error.ErrorMessage);
                }
                ValidationSuccess = false;
            }

            return ValidationSuccess;
        }

        protected virtual bool ValidateSelf()
        {
            errors.Clear();

            var ValidationSuccess = ValidateSelfUsingDefaultValidation();

            this.GetType()
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(ValidationAttribute), false))
                .Select(propInfo => propInfo.Name)
                .ToList()
                .ForEach(prop => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop)));

            return ValidationSuccess;
        }

        protected virtual async void ValidatePropertyWithCustomPredicateAsync(object val, Predicate<object> validationPredicate, string errorMessage, bool useDefaultValidationBeforeCustom = true, [CallerMemberName] string propertyName = null)
        {
            if (useDefaultValidationBeforeCustom && !ValidateProperty(val, propertyName))
                return;

            if (errors.ContainsKey(propertyName))
                errors.Remove(propertyName);

            if (await Task.Run(() => validationPredicate(val)))
                errors.Add(propertyName, new List<string>() { errorMessage });

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual bool ValidateProperty(object val, [CallerMemberName] string propertyName = null)
        {
            if (errors.ContainsKey(propertyName))
                errors.Remove(propertyName);

            ValidationContext context = new ValidationContext(this)
            {
                MemberName = propertyName
            };
            List<ValidationResult> results = new();

            var validationSuccess = true;
            if (!Validator.TryValidateProperty(val, context, results))
            {
                errors[propertyName] = results.Select(x => x.ErrorMessage).ToList();
                validationSuccess = false;
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            return validationSuccess;
        }

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable? GetErrors(string propertyName)
        {
            return errors.ContainsKey(propertyName) ? errors[propertyName] : null;
        }
    }
}
