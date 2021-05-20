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
using Tea.Page;
using Tea.ModelSQL;
using System.IO;

namespace Tea.Page
{
    /// <summary>
    /// Логика взаимодействия для DescriptionProduct.xaml
    /// </summary>
    public partial class DescriptionProduct : Window
    {
        ListProduct gWindow;
        Product gProduct;
        public DescriptionProduct(Product lProduct, ListProduct lWindow)
        {
            gWindow = lWindow;
            gProduct = lProduct;
            InitializeComponent();
            
            Tb_Category.Text = gProduct.View.Name;
            Tb_Name.Text = gProduct.Name;
            Tb_Description.Text = gProduct.Information;
            Tb_Quantity.Text = gProduct.Quantity.ToString();
            Tb_Articul.Text = gProduct.IdProduct.ToString();
            Tb_Price.Text = gProduct.Price.ToString();

            using (MemoryStream stream = new MemoryStream(gProduct.Image))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                pathProduct.Source = bitmapImage;
            }
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gWindow.BackToStart();
        }

        private void Btn_Bascet_Click(object sender, RoutedEventArgs e)
        {
            gWindow.productList.Add(gProduct);
            MessageBox.Show("Товар успешно добавлен в корзину", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }
    }
}
