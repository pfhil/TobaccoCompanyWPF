using System.ComponentModel;
using System.Runtime.CompilerServices;
using TobaccoCompanyWPF.Models;

namespace TobaccoCompanyWPF.ViewModels.MVVM
{
    public abstract class BaseWindowViewModel : BaseViewModel
    {
        public Action? MinimizeWindowAction { get; set; }
        public Action<bool?> CloseWindowAction { get; set; }
        public Action? OpenWindowAction { get; set; }
        public Action? HideWindowAction { get; set; }
        public Action<string>? ShowErrorMessageAction { get; set; }

        public ICommand MinimizeWindowCommand => new Command(_ => MinimizeWindowAction?.Invoke());
        public ICommand CloseWindowCommand => new Command(_ => CloseWindowAction?.Invoke(null));

    }
}
