using System.Windows;
using TobaccoCompanyWPF.ViewModels;

namespace TobaccoCompanyWPF.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = new MainPresenter()
            //{
            //    OpenWindowAction = () => this.Show(),
            //    HideWindowAction = () => this.Hide(),
            //    MinimizeWindowAction = () => SystemCommands.MinimizeWindow(this),
            //    CloseWindowAction = () => SystemCommands.CloseWindow(this),
            //};
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
