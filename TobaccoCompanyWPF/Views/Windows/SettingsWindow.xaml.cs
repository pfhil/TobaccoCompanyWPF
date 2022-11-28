using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TobaccoCompanyWPF.ViewModels;

namespace TobaccoCompanyWPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            var settingsViewModel = new SettingsViewModel(loginViewModel)
            {
                MinimizeWindowAction = () => SystemCommands.MinimizeWindow(this),
                CloseWindowAction = _ => SystemCommands.CloseWindow(this),
                ShowErrorMessageAction = text =>
                {
                    this.errorMessageBorder.Visibility = Visibility.Visible;
                    this.errorMessageTextBlock.Text = text;
                }
            };

            this.DataContext = settingsViewModel;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
