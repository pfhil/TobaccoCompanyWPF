using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.ViewModels;

namespace TobaccoCompanyWPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var loginPresenter = new LoginViewModel()
            {
                OpenWindowAction = () => this.Show(),
                HideWindowAction = () => this.Hide(),
                MinimizeWindowAction = () => SystemCommands.MinimizeWindow(this),
                CloseWindowAction = _ => SystemCommands.CloseWindow(this),
                ShowErrorMessageAction = text =>
                {
                    this.errorMessageBorder.Visibility = Visibility.Visible;
                    this.errorMessageTextBlock.Text = text;
                }
            };
            this.DataContext = loginPresenter;

        }

        public new void Show()
        {
            this.passwordBox.Clear();
            this.loginTextBox.Clear();
            base.Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void textFiels_Changed()
        {
            hideErrorMessage();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textFiels_Changed();
        }

        private void loginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textFiels_Changed();
        }

        private void hideErrorMessage()
        {
            if (errorMessageBorder.Visibility != Visibility.Collapsed)
            {
                errorMessageBorder.Visibility = Visibility.Collapsed;
                errorMessageTextBlock.Text = string.Empty;
            }
        }
    }
}
