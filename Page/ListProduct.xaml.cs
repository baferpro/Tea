﻿using System;
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

            Lv_Product.ItemsSource = db.Product.ToList();

            List<View> views = db.View.ToList();
            Cb_Sort_View.ItemsSource = views;
            views.Insert(0, new View { Name = "Все виды" });
            Cb_Sort_View.DisplayMemberPath = "Name";
            Cb_Sort_View.SelectedIndex = 0;

            Btn_List_Product.IsEnabled = false;
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            BackToStart();
        }

        public void Filter()
        {
            var allProduct = db.Product.ToList();

            if (TB_Search_Articul.Text.Length > 0 && TB_Search_Articul.Text.ToLower().Equals("Поиск по артикулу".ToLower()) == false)
            {
                allProduct = allProduct.Where(i => i.IdProduct.ToString().ToLower().Contains(TB_Search_Articul.Text.ToLower())).ToList();
            }
            if (TB_Search_Name.Text.Length > 0 && TB_Search_Name.Text.ToLower().Equals("Поиск по названию".ToLower()) == false)
            {
                allProduct = allProduct.Where(i => i.Name.ToLower().Contains(TB_Search_Name.Text.ToLower())).ToList();
            }
            if (Cb_Sort_Price.SelectedIndex != 0)
            {
                if (Cb_Sort_Price.SelectedIndex == 1)
                {
                    allProduct = allProduct.OrderBy(i => i.Price).ToList();
                }
                else
                {
                    allProduct = allProduct.OrderByDescending(i => i.Price).ToList();
                }
            }
            if (Cb_Sort_View.SelectedIndex != 0)
            {
                if (Cb_Sort_View.SelectedIndex == 0)
                {
                    Lv_Product.ItemsSource = allProduct;
                }
                else
                {
                    var View = Cb_Sort_View.SelectedItem as View;
                    allProduct = allProduct.Where(i => i.IdView == View.IdView).ToList();
                }
            }

            Lv_Product.ItemsSource = allProduct;
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
            var product = lv.SelectedItem as Product;


            DescriptionProduct descriptionProduct = new DescriptionProduct(product, this);
            this.Visibility = Visibility.Hidden;
            descriptionProduct.ShowDialog();

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
    }
}
