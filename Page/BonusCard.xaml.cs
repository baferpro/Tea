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

namespace Tea.Page
{
    /// <summary>
    /// Логика взаимодействия для BonusCard.xaml
    /// </summary>
    public partial class BonusCard : Window
    {
        public BonusCard()
        {
            InitializeComponent();

            CB_Status.ItemsSource = db.Status.Select(i => i.Name).ToList();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if(TB_FName.Text.Length > 0 && TB_Name.Text.Length > 0 && DP_Birthday.SelectedDate != null && CB_Status.SelectedIndex >= 0)
            {
                db.BonusCard.Add(new ModelSQL.BonusCard
                {
                    FName = TB_FName.Text,
                    SName = TB_Name.Text,
                    BirthdayDay = DP_Birthday.SelectedDate.Value,
                    IdStatus = db.Status.Where(i => i.Name == CB_Status.SelectedItem.ToString()).Select(i => i.IdStatus).FirstOrDefault()
                });

                db.SaveChanges();
                MessageBox.Show($"{CB_Status.SelectedItem} карта успешно сделана.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show($"Не удалось создать карту, заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
