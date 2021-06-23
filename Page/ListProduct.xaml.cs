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
using static Tea.ModelSQL.DateFrame;
using Tea.ModelSQL;

namespace Tea.Page
{
    /// <summary>
    /// Логика взаимодействия для ListProduct.xaml
    /// </summary>
    public partial class ListProduct : Window
    {
        public List<Product> productList = new List<Product>();
        List<Product> descriptionProductList = new List<Product>();

        FirstPage gWindow;
        Employee gUser;
        public ListProduct(FirstPage lWindow, Employee lUser)
        {
            gUser = lUser;
            gWindow = lWindow;
            InitializeComponent();

            if (gUser.Position.IdPosition != 7)
                Btn_AddProduct.Visibility = Visibility.Collapsed;

            List<View> views = db.View.ToList();
            Cb_Sort_View.ItemsSource = views;
            views.Insert(0, new View { Name = "Все виды" });
            Cb_Sort_View.DisplayMemberPath = "Name";
            Cb_Sort_View.SelectedIndex = 0;

            Btn_List_Product.IsEnabled = false;

            Filter();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            BackToStart();
        }

        public void Filter()
        {
            List<_listProduct> list = new List<_listProduct>();
            var allProduct = db.Product.ToList();
            foreach (var item in allProduct)
            {
                Visibility visibility;
                if (gUser.Position.IdPosition != 7)
                    visibility = Visibility.Collapsed;
                else
                    visibility = Visibility.Visible;
                list.Add(new _listProduct
                {
                    _product = item,
                    _btnVisible = visibility
                });
            }

            if (TB_Search_Articul.Text.Length > 0 && TB_Search_Articul.Text.ToLower().Equals("Поиск по артикулу".ToLower()) == false)
            {
                list = list.Where(i => i._product.IdProduct.ToString().ToLower().Contains(TB_Search_Articul.Text.ToLower())).ToList();
            }
            if (TB_Search_Name.Text.Length > 0 && TB_Search_Name.Text.ToLower().Equals("Поиск по названию".ToLower()) == false)
            {
                list = list.Where(i => i._product.Name.ToLower().Contains(TB_Search_Name.Text.ToLower())).ToList();
            }
            if (Cb_Sort_Price.SelectedIndex != 0)
            {
                if (Cb_Sort_Price.SelectedIndex == 1)
                {
                    list = list.OrderBy(i => i._product.Price).ToList();
                }
                else
                {
                    list = list.OrderByDescending(i => i._product.Price).ToList();
                }
            }
            if (Cb_Sort_View.SelectedIndex != 0)
            {
                if (Cb_Sort_View.SelectedIndex == 0)
                {
                    Lv_Product.ItemsSource = list;
                }
                else
                {
                    var View = Cb_Sort_View.SelectedItem as View;
                    list = list.Where(i => i._product.IdView == View.IdView).ToList();
                }
            }

            Lv_Product.ItemsSource = list;
        }

        private void Btn_Exit_Click_1(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var product = button.DataContext as Product;

            productList.Add(product);
            MessageBox.Show("Товар успешно добавлен в корзину", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Btn_Checkout_Click(object sender, RoutedEventArgs e)
        {
            Shopping shopping = new Shopping(productList, this, gUser);
            this.Visibility = Visibility.Hidden;
            shopping.ShowDialog();
        }

        private void TB_Search_Articul_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void TB_Search_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.IsLoaded)
            {
                Filter();
            }
        }

        private void Lv_Product_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
                return;
            if (lv.SelectedItem != null)
            {
                var product = lv.SelectedItem as Product;
                DescriptionProduct descriptionProduct = new DescriptionProduct(product, this);
                this.Visibility = Visibility.Hidden;
                descriptionProduct.ShowDialog();
            }

        }

        private void TB_Search_Articul_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(TB_Search_Articul.Text.Equals("Поиск по артикулу"))
            {
                TB_Search_Articul.Clear();
            }
        }

        private void TB_Search_Name_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(TB_Search_Name.Text.Equals("Поиск по названию"))
            {
                TB_Search_Name.Clear();
            }
        }

        private void Cb_Sort_Price_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void Cb_Sort_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        public void BackToStart()
        {
            this.Close();
            gWindow.BackToStart();
        }

        private void BTNClearFilter_Click(object sender, RoutedEventArgs e)
        {
            TB_Search_Name.Text = "Поиск по названию";
            TB_Search_Articul.Text = "Поиск по артикулу";
            Cb_Sort_Price.SelectedIndex = 0;
            Cb_Sort_View.SelectedIndex = 0;
            Filter();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var product = button.DataContext as Product;
            var _messageBoxAnswer = MessageBox.Show("Вы действительно хотите удалить товар?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(_messageBoxAnswer == MessageBoxResult.Yes)
            {
                db.Product.Remove(product);
                db.SaveChanges();
                MessageBox.Show("Товар успешно удалён.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Filter();
            }
        }

        private void Btn_AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProduct product = new AddProduct();
            product.ShowDialog(); 
            Filter();
        }

        class _listProduct
        {
            public Product _product { get; set; }
            public Visibility _btnVisible { get; set; }
        }
    }
}
