using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TLGames.WPFUI.Views
{
    /// <summary>
    /// Interaction logic for CardProduct.xaml
    /// </summary>
    public partial class ProductCardUserControl : UserControl
    {
        public ProductCardUserControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ProductCardUserControl), new PropertyMetadata(null));

        // Dependency Property for Product Name
        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register("ProductName", typeof(string), typeof(ProductCardUserControl), new PropertyMetadata("Product Name"));

        // Dependency Property for Product Price
        public decimal ProductPrice
        {
            get { return (decimal)GetValue(ProductPriceProperty); }
            set { SetValue(ProductPriceProperty, value); }
        }

        public static readonly DependencyProperty ProductPriceProperty =
            DependencyProperty.Register("ProductPrice", typeof(decimal), typeof(ProductCardUserControl), new PropertyMetadata(0.000m));
    }
}

