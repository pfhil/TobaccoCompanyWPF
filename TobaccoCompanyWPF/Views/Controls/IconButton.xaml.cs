using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;


namespace TobaccoCompanyWPF.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для IconButton.xaml
    /// </summary>
    public partial class IconButton : Button
    {

        public PackIcon Icon
        {
            get { return (PackIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty = 
            DependencyProperty.Register(
                "Icon",
                typeof(PackIcon),
                typeof(IconButton),
                new UIPropertyMetadata(null, new PropertyChangedCallback(IconChanged)),
                new ValidateValueCallback(ValidateIcon));


        public static bool ValidateIcon(object value)
        {
            if (value != null)
                return value is PackIcon;
            return true;
        }

        private static void IconChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            IconButton s = (IconButton)depObj;
            PackIcon packIcon = s.ico;
            packIcon.Kind = ((PackIcon)args.NewValue).Kind;
        }

        public IconButton() : base()
        {
            InitializeComponent();
        }
    }
}
