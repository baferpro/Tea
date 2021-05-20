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

namespace Tea.Page
{
    /// <summary>
    /// Логика взаимодействия для FirstPage.xaml
    /// </summary>
    public partial class FirstPage : Window
    {
        MainWindow gWindow;
        Employee gUser;
        public FirstPage(MainWindow lWindow, Employee lUser)
        {
            gUser = lUser;
            gWindow = lWindow;
            InitializeComponent();

            TB_FIN.Text = gUser.Name + " " + gUser.MName[0] + "." + gUser.SName[0] + ".";

            Btn_Checkout.IsEnabled = false;
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }

        private void Btn_List_Product_Click(object sender, RoutedEventArgs e)
        {
            ListProduct listProduct = new ListProduct(this, gUser);
            this.Visibility = Visibility.Hidden;
            listProduct.ShowDialog();
        }

        public void BackToStart()
        {
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }
    }
}
