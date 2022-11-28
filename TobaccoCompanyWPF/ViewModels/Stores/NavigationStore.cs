using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoCompanyWPF.ViewModels.MVVM;

namespace TobaccoCompanyWPF.ViewModels.Stores
{
    public class NavigationStore
    {
        public event Action? CurrentViewModelChanged;

        private BaseViewModel? currentViewModel;

        public BaseViewModel? CurrentViewModel
        {
            get => this.currentViewModel;
            set
            {
                this.currentViewModel = value;
                this.CurrentViewModelChanged?.Invoke();
            }
        }
    }
}
