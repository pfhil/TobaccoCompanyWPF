using System.IO;
using System.Reflection;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;

namespace TobaccoCompanyWPF.ViewModels
{
    public class ProductEditViewModel : BaseViewModel
    {
        private Product product;
        private IFileService fileService;
        private IDialogService dialogService;
        private bool ImageWasEdited = false;

        public ProductEditViewModel(NavigationStore navigationStore, Product product)
        {
            this.product = product;
            this.navigationStore = navigationStore;
            var navigationService = new NavigationService<ProductsViewModel>(
                navigationStore,
                () => new ProductsViewModel(navigationStore));

            this.fileService = new DefaultFileService();
            this.dialogService = new DefaultDialogService();

            this.SaveChangesCommand = new Command(_ => SaveChangesAsync(navigationService));
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

        private async void SaveChangesAsync(NavigationService<ProductsViewModel> navigationService)
        {
            if (this.ValidateSelf())
            {
                await this.DoActionWithLoadingAsync(() =>
                {
                    using TobaccoCompanyContext context = new TobaccoCompanyContext();
                    var product = context.Products.Find(this.product.Id);
                    product.Name = this.product.Name;
                    product.Description = this.product.Description;
                    product.Price = this.product.Price;
                    if (ImageWasEdited)
                    {
                        var fileName = !string.IsNullOrEmpty(this.PathToImage) ? fileService.SaveImageToPublic(this.PathToImage) : null;
                        product.FileName = fileName;
                    }
                    context.Products.Update(product);
                    context.SaveChanges();
                });
                navigationService.Navigate();
            }
        }

        [Required(ErrorMessage = "Название - обязательное поле")]
        public string Name
        {
            get => product.Name;
            set
            {
                product.Name = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [Required(ErrorMessage = "Описание - обязательное поле")]
        public string Description
        {
            get => product.Description;
            set
            {
                OnPropertyChanged();
                ValidateProperty(value);
                product.Description = value;
            }
        }

        [Required(ErrorMessage = "Цена - обязательное поле")]
        public decimal Price
        {
            get => product.Price;
            set
            {
                OnPropertyChanged();
                ValidateProperty(value);
                product.Price = value;
            }
        }

        private static string PathToPublic = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @"Public\ProductsImage");

        public string PathToImage
        {
            get
            {
                if (!ImageWasEdited)
                {
                    return product.FileName != null ? Path.Combine(PathToPublic, product.FileName) : "";
                }
                else
                {
                    return product.FileName;
                }
            }
            set
            {
                product.FileName = value;
                OnPropertyChanged();
                this.ImageWasEdited = true;
            }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChoiceImageFile { get; }

        public NavigationStore navigationStore { get; set; }
    }
}
