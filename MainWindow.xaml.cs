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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tea.Page;
using Tea.ModelSQL;
using static Tea.ModelSQL.DateFrame;

namespace Tea
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_LogIn_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> list = db.Employee.Where(i => i.Login != null && i.Password != null).ToList();
            list = list.Where(i => i.Login.Equals(Tb_Login.Text)).ToList();
            if(list.Count() == 1)
            {
                list = list.Where(i => i.Password.Equals(Tb_Password.Password)).ToList();
                if(list.Count() == 1)
                {
                    var user = list.FirstOrDefault();
                    FirstPage firstPage = new FirstPage(this, user);
                    this.Visibility = Visibility.Hidden;
                    Tb_Login.Clear();
                    Tb_Password.Clear();
                    firstPage.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Пароль неверный, попробуйте ещё раз.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Логин неверный, попробуйте ещё раз.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
