using System.IO;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;

namespace TobaccoCompanyWPF.ViewModels
{
    public class ProductCreateViewModel : BaseViewModel
    {
        private string name = "";
        private string description = "";
        private decimal price;

        private IFileService fileService;
        private IDialogService dialogService;

        public ProductCreateViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            var navigationService = new NavigationService<ProductsViewModel>(
                navigationStore,
                () => new ProductsViewModel(navigationStore));

            this.fileService = new DefaultFileService();
            this.dialogService = new DefaultDialogService();

            this.CreateProductCommand = new AsyncCommand(() => CreateProductAsync(navigationService));
            this.CancelCommand = new Command(_ => navigationService.Navigate());
            this.ChoiceImageFile = new Command(_ =>
            {
                try
                {
                    if (dialogService.ChoiceFileDialog() == true)
                    {
                        this.PathToImage = dialogService.FilePath;
                    }
                }
                catch (Exception ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
            });
        }

        private async Task CreateProductAsync(NavigationService<ProductsViewModel> navigationService)
        {
            if (this.ValidateSelf())
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    var fileName = !string.IsNullOrEmpty(this.PathToImage) ? fileService.SaveImageToPublic(this.PathToImage) : null;
                    var product = new Product()
                    {
                        Name = this.Name,
                        Description = this.Description,
                        Price = this.Price,
                        FileName = fileName,
                    };

                    using TobaccoCompanyContext context = new TobaccoCompanyContext();
                    context.Products.Add(product);
                    context.SaveChanges();
                });
                navigationService.Navigate();
            }
        }

        [Required(ErrorMessage = "Название - обязательное поле")]
        public string Name
        {
            get => name;
            set
            {
                name=value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Описание - обязательное поле")]
        public string Description
        {
            get => description;
            set
            {
                OnPropertyChanged();
                ValidateProperty(value);
                description=value;
            }
        }

        [Required(ErrorMessage = "Цена - обязательное поле")]
        public decimal Price
        {
            get => price;
            set
            {
                OnPropertyChanged();
                ValidateProperty(value);
                price=value;
            }
        }

        public string PathToImage { get; set; }

        public ICommand CreateProductCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChoiceImageFile { get; }

        public NavigationStore navigationStore { get; set; }
    }
}
