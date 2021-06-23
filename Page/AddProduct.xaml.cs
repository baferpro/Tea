using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Tea.Page
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private string _pathPhoto;
        public AddProduct()
        {
            InitializeComponent();

            _cbCountry.ItemsSource = db.Country.Select(i => i.Name).ToList();
            _cbForm.ItemsSource = db.Form.Select(i => i.Name).ToList();
            _cbUnit.ItemsSource = db.Unit.Select(i => i.Name).ToList();
            _cbView.ItemsSource = db.View.Select(i => i.Name).ToList();
            _cdCategory.ItemsSource = db.Category.Select(i => i.Name).ToList();
        }

        private void Btn_AddProduct_Click(object sender, RoutedEventArgs e)
        {
            db.Product.Add(new ModelSQL.Product
            {
                Name = _tbName.Text,
                Quantity = Convert.ToInt32(_tbQuantity.Text),
                Price = Convert.ToInt32(_tbPrice.Text),
                Information = _tbInformation.Text,
                IdForm = db.Form.Where( i => i.Name == _cbForm.SelectedItem.ToString()).Select(i => i.IdForm).FirstOrDefault(),
                IdCategory = db.Category.Where(i => i.Name == _cdCategory.SelectedItem.ToString()).Select(i => i.IdCategory).FirstOrDefault(),
                IdUnit = db.Unit.Where(i => i.Name == _cbUnit.SelectedItem.ToString()).Select(i => i.IdUnit).FirstOrDefault(),
                IdView = db.View.Where(i => i.Name == _cbView.SelectedItem.ToString()).Select(i => i.IdView).FirstOrDefault(),
                IdCountry = db.Country.Where(i => i.Name == _cbCountry.SelectedItem.ToString()).Select(i => i.IdCountry).FirstOrDefault(),
                Images = File.ReadAllBytes(_pathPhoto)
            });

            db.SaveChanges();
            MessageBox.Show($"Товар {_tbName.Text} успешно добавлен.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void Btn_CancelProduct_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _imProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                _pathPhoto = openFileDialog.FileName;
            }
        }
    }
}
