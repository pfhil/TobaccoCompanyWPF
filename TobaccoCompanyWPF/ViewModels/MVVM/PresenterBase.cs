using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TobaccoCompanyWPF.ViewModels.MVVM
{
    public abstract class PresenterBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Action? MinimizeWindowAction { get; set; }
        public Action? CloseWindowAction { get; set; }
        public Action? OpenWindowAction { get; set; }
        public Action? HideWindowAction { get; set; }
        public Action<string>? ShowErrorMessageAction { get; set; }

        public ICommand MinimizeWindowCommand => new Command(_ => MinimizeWindowAction?.Invoke());
        public ICommand CloseWindowCommand => new Command(_ => CloseWindowAction?.Invoke());

    }
}
