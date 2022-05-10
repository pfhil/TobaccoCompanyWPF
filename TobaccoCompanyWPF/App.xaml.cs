using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels;
using TobaccoCompanyWPF.ViewModels.Stores;
using TobaccoCompanyWPF.Views.Windows;

namespace TobaccoCompanyWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    NavigationStore navigationStore = new NavigationStore();
        //    MainWindow window = new MainWindow()
        //    {
        //        DataContext = new MainViewModel(navigationStore)
        //    };
        //    window.Show();
        //    base.OnStartup(e);
        //}
    }
}
