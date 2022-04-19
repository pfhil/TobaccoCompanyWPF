using System.Windows;
using System.Windows.Controls;
using TobaccoCompanyWPF.ViewModels;

namespace TobaccoCompanyWPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow(LoginPresenter loginPresenter)
        {
            InitializeComponent();

            var registrationPresenter = new RegistrationPresenter(loginPresenter)
            {
                MinimizeWindowAction = () => SystemCommands.MinimizeWindow(this),
                CloseWindowAction = () => SystemCommands.CloseWindow(this),
                ShowErrorMessageAction = text =>
                {
                    this.errorMessageBorder.Visibility = Visibility.Visible;
                    this.errorMessageTextBlock.Text = text;
                }
            };

            this.DataContext = registrationPresenter;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((RegistrationPresenter)this.DataContext).User.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
