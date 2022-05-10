using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.CurrentPrincipal;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;

namespace TobaccoCompanyWPF.ViewModels
{
    public class CatalogViewModel : BaseViewModel
    {
        public CatalogViewModel(CurrentPrincipal currentPrincipal)
        {
            AddProductToCartCommand = new Command(addProduct);
            DeleteProductFromCartCommand = new Command(deleteProduct);
            this.CurrentPrincipal=currentPrincipal;
        }

        private void deleteProduct(object? productObject)
        {
            var product = (productObject as ProductStackViewModel)!.Product;
            var productStackViewModel = this.Products.First(productBox => productBox.Id == product!.Id);

            ProductStack productStack = CurrentPrincipal.UserCart.ProductStack.FirstOrDefault(productStack => productStack.product.Id == product.Id)!;
            productStack.Amount--;
            if (productStack.Amount == 0)
            {
                CurrentPrincipal.UserCart.ProductStack.Remove(productStack);
            }

            productStackViewModel.AmountTaken--;
        }

        private void addProduct(object? productObject)
        {
            var product = (productObject as ProductStackViewModel)!.Product;
            var productStackViewModel = this.Products.First(productBox => productBox.Id == product!.Id);

            ProductStack? productStack = CurrentPrincipal.UserCart.ProductStack.FirstOrDefault(productStack => productStack.product.Id == product.Id);
            if (productStack == null)
            {
                CurrentPrincipal.UserCart.ProductStack.Add(new ProductStack(product, 1));
            }
            else
            {
                productStack.Amount++;
            }

            productStackViewModel.AmountTaken++;
        }

        private async Task LoadDataAsync()
        {
            await this.DoActionWithLoadingAsync(() =>
            {
                using TobaccoCompanyContext context = new TobaccoCompanyContext();
                var tempProducts = context.Products.ToList();
                var obsProducts = new ObservableCollection<ProductStackViewModel>();

                foreach (var tempProduct in tempProducts)
                {
                    var amountTaken = CurrentPrincipal.UserCart.ProductStack.FirstOrDefault(productStack => productStack.product.Id == tempProduct.Id)?.Amount ?? 0;

                    var amount = context.Invoices.Where(inv => inv.ProductId == tempProduct.Id).Select(inv => inv.Amount).Sum();

                    var wasTakenProduct = context.CashReceipts.Where(cash => cash.ProductId == tempProduct.Id).Select(cash => cash.Amount).Sum();

                    obsProducts.Add(new ProductStackViewModel(tempProduct, amount - wasTakenProduct, amountTaken));
                }
                this.Products = obsProducts;
            });
        }

        public ObservableCollection<ProductStackViewModel> Products { get; set; }

        public ICommand AddProductToCartCommand { get; }

        public ICommand DeleteProductFromCartCommand { get; }

        public IAsyncCommand MyCommand => new AsyncCommand(() => LoadDataAsync());

        public CurrentPrincipal CurrentPrincipal { get; }
    }

    public class ProductStackViewModel : BaseViewModel
    {
        public Product Product;
        private string name;
        private int amountTaken;

        public ProductStackViewModel(Product product, int amount, int amountTaken)
        {
            this.Product = product;
            this.Amount = amount;
            this.AmountTaken = amountTaken;

            if (amountTaken > 0 && amountTaken == Amount)
            {
                this.BuyingState = BuyingStateEnum.WasTakenAndNoMoreAvailable;
            }
            else if (amountTaken == 0 && Amount > 0)
            {
                this.BuyingState = BuyingStateEnum.Available;
            }
            else if (amountTaken == 0 && Amount == 0)
            {
                this.BuyingState = BuyingStateEnum.NotAvailable;
            }
            else if (amountTaken > 0 && amountTaken < Amount)
            {
                this.BuyingState = BuyingStateEnum.WasTaken;
            }
        }

        private string GetBuyingStateString() => BuyingState switch
        {
            BuyingStateEnum.NotAvailable => "Not Available",
            BuyingStateEnum.Available => "Available",
            BuyingStateEnum.WasTakenAndNoMoreAvailable => "Was Taken and no more Available",
            BuyingStateEnum.WasTaken => "Was Taken",
            _ => throw new NotImplementedException(),
        };

        public string BuyingStateString => GetBuyingStateString();

        public BuyingStateEnum BuyingState { get; set; }

        public int Id { get => Product.Id; }

        public string Name { get => Product.Name; set => Product.Name=value; }

        public string Description { get => Product.Description; set => Product.Description=value; }

        public decimal Price { get => Product.Price; set => Product.Price=value; }

        private static string PathToPublic = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @"Public\ProductsImage");

        public string PathToImage =>
            Product.FileName != null ?
            Path.Combine(PathToPublic, Product.FileName) :
            Path.Combine(PathToPublic, "ImageNotFound.jpg");

        public int Amount { get; set; }

        public int AmountTaken 
        {
            get => amountTaken; 
            set
            {
                amountTaken = value;

                if (amountTaken > 0 && amountTaken == Amount)
                {
                    this.BuyingState = BuyingStateEnum.WasTakenAndNoMoreAvailable;
                }
                else if (amountTaken == 0 && Amount > 0)
                {
                    this.BuyingState = BuyingStateEnum.Available;
                }
                else if (amountTaken == 0 && Amount == 0)
                {
                    this.BuyingState = BuyingStateEnum.NotAvailable;
                }
                else if (amountTaken > 0 && amountTaken < Amount)
                {
                    this.BuyingState = BuyingStateEnum.WasTaken;
                }
            } 
        }

        public enum BuyingStateEnum
        {
            NotAvailable,
            Available,
            WasTaken,
            WasTakenAndNoMoreAvailable
        }

    }
}
