using System.Collections.ObjectModel;
using TobaccoCompanyWPF.Models;
using TobaccoCompanyWPF.Models.CurrentPrincipal;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.MVVM;
using TobaccoCompanyWPF.ViewModels.Services;
using TobaccoCompanyWPF.ViewModels.Stores;
using TobaccoCompanyWPF.ViewModels.ValidationAttributes;

namespace TobaccoCompanyWPF.ViewModels
{
    public class PlaceOrderViewModel : BaseViewModel
    {
        private string addressDelivery;
        private ReceivingMethod receivingMethod;
        private DateTime dateDelivery = DateTime.Now.AddDays(2);

        public PlaceOrderViewModel(CurrentPrincipal currentPrincipal, NavigationStore navigationStore)
        {
            this.CurrentPrincipal=currentPrincipal;

            var navigationService = new NavigationService<UserOrdersViewModel>(
                navigationStore,
                () => new UserOrdersViewModel(currentPrincipal));

            this.PlaceOrderCommand = new AsyncCommand(async () =>
            {
                if (this.ValidateSelf())
                {
                    await this.DoActionWithLoadingAsync(() =>
                    {
                        using TobaccoCompanyContext context = new TobaccoCompanyContext();
                        List<CashReceipt> cashReceipts = new List<CashReceipt>();

                        foreach (var product in UserCart)
                        {
                            cashReceipts.Add(new CashReceipt()
                            {
                                Product = context.Products.First(prod => prod.Id == product.Id),
                                Amount = product.AmountTaken,
                            });
                        }
                        var client = context.Users.First(user => user.Id == this.CurrentPrincipal.User.Id);
                        Order order = new Order()
                        {
                            Client = client,
                            Address = string.IsNullOrEmpty(this.AddressDelivery) ? null : this.AddressDelivery,
                            Date = DateTime.Now,
                            DateReceived = this.ReceivingMethod == ReceivingMethod.Delivery ? this.DateDelivery : null,
                            PaymentType = this.PaymentType,
                            ReceivingMethod = this.ReceivingMethod,
                            CashReceipts = cashReceipts,
                            OrderState = OrderState.Open,
                        };

                        context.Orders.Add(order);
                        context.SaveChanges();
                    });
                    this.CurrentPrincipal.UserCart.ProductStack.Clear();
                    this.UserCart?.Clear();
                    navigationService.Navigate();
                }
            });

            this.DeleteProductCommand = new Command(objProduct =>
            {
                var productStackTemp = objProduct as ProductStackViewModel;

                this.UserCart.Remove(productStackTemp);
                CurrentPrincipal.UserCart.ProductStack.Remove(CurrentPrincipal.UserCart.ProductStack.First(productStack => productStack.product.Id == productStackTemp.Id)!);
            });

            this.AddProductToCartCommand = new Command(productobj => this.addProduct(productobj));
            this.RemoveProductFromCartCommand = new Command(productobj => this.RemoveProduct(productobj));
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

                    if (amountTaken > 0)
                    {
                        obsProducts.Add(new ProductStackViewModel(tempProduct, amount - wasTakenProduct, amountTaken));
                    }
                }
                this.UserCart = obsProducts;
            });
        }

        private void RemoveProduct(object? productObject)
        {
            var product = (productObject as ProductStackViewModel)!.Product;
            var productStackViewModel = this.UserCart.First(productBox => productBox.Id == product!.Id);

            ProductStack productStack = CurrentPrincipal.UserCart.ProductStack.FirstOrDefault(productStack => productStack.product.Id == product.Id)!;
            productStack.Amount--;
            productStackViewModel.AmountTaken--;
            if (productStack.Amount == 0)
            {
                CurrentPrincipal.UserCart.ProductStack.Remove(productStack);
                this.UserCart.Remove(productStackViewModel);
            }
        }

        private void addProduct(object? productObject)
        {
            var product = (productObject as ProductStackViewModel)!.Product;
            var productStackViewModel = this.UserCart.First(productBox => productBox.Id == product!.Id);

            ProductStack? productStack = CurrentPrincipal.UserCart.ProductStack.FirstOrDefault(productStack => productStack.product.Id == product.Id);
            productStack.Amount++;

            productStackViewModel.AmountTaken++;
        }

        public ObservableCollection<ProductStackViewModel> UserCart { get; set; }

        public PaymentType PaymentType { get; set; }

        public ReceivingMethod ReceivingMethod
        {
            get => receivingMethod;
            set
            {
                receivingMethod=value;
                OnPropertyChanged();

                if (value is ReceivingMethod.Pickup)
                {
                    this.AddressDelivery = "";
                    ValidateProperty("", nameof(AddressDelivery));
                    this.DateDelivery = DateTime.Now.AddDays(2);
                    ValidateProperty(DateTime.Now.AddDays(2), nameof(DateDelivery));
                }
            }
        }

        [RequiredIf("ReceivingMethod", ReceivingMethod.Delivery, ErrorMessage = "Адрес доставки - обязательное поле")]
        public string AddressDelivery
        {
            get => addressDelivery;
            set
            {
                addressDelivery = value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }

        [RequiredIf("ReceivingMethod", ReceivingMethod.Delivery)]
        [DateRange(0, 2, ErrorMessage = "Минимальный срок доставки - 2 дня")]
        public DateTime DateDelivery
        {
            get => dateDelivery;
            set
            {
                dateDelivery=value;
                OnPropertyChanged();
                ValidateProperty(value);
            }
        }
        public ICommand PlaceOrderCommand { get; }
        public ICommand LoadDataCommand => new AsyncCommand(() => LoadDataAsync());
        public ICommand DeleteProductCommand { get; }
        public ICommand RemoveProductFromCartCommand { get; }
        public ICommand AddProductToCartCommand { get; }

        public CurrentPrincipal CurrentPrincipal { get; }
    }
}
