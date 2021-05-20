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
using Tea.Page;
using Tea.ModelSQL;

namespace Tea.Page
{
    /// <summary>
    /// Логика взаимодействия для Shopping.xaml
    /// </summary>
    public partial class Shopping : Window
    {
        int countProduct = 0;
        decimal finalSum = 0;
        List<Product> gProductBascet;
        ListProduct gWindow;
        Employee gUser;
        bool CardAdded = false;
        public Shopping(List<Product> lProductBascet, ListProduct lWindow, Employee lUser)
        {
            gProductBascet = lProductBascet;
            gWindow = lWindow;
            gUser = lUser;
            InitializeComponent();

            for(int i = 0; i < gProductBascet.Count; i++)
            {
                finalSum += gProductBascet[i].Price;
            }

            Lv_Bascet.ItemsSource = gProductBascet.ToList();

            countProduct = gProductBascet.Count();
            Tb_CountBascet.Text = countProduct.ToString();
            Tb_FinalSum.Text = finalSum.ToString();
            

            Btn_Checkout.IsEnabled = false;
        }

     

        private void Btn_List_Product_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gWindow.BackToStart();
        }

        private void Btn_NewCard_Click(object sender, RoutedEventArgs e)
        {
            BonusCard bonusCard = new BonusCard();
            bonusCard.ShowDialog();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }

        private void Btn_CheckoutShopping_Click(object sender, RoutedEventArgs e)
        {
            Sale sale;
            if (CardAdded)
            {
                sale = db.Sale.Add(new Sale
                {
                    IdEmployee = gUser.IdEmployee,
                    Date = DateTime.Now,
                    IdBonusCard = Convert.ToInt32(Tb_NumberCard.Text)
                });
            }
            else
            {
                sale = db.Sale.Add(new Sale
                {
                    IdEmployee = gUser.IdEmployee,
                    Date = DateTime.Now,
                    IdBonusCard = null
                });
            }
            for (int i = 0; i < gProductBascet.Count; i++)
            {
                int idProduct = gProductBascet[i].IdProduct;
                db.ProductInSale.Add(new ProductInSale
                {
                    IdSale = sale.IdSale,
                    IdProduct = idProduct,
                    Quantity = 1
                });
            }
            db.SaveChanges();

            MessageBox.Show("Покупка успешно оформлена", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            gWindow.Visibility = Visibility.Visible;
        }

        private void Tb_NumberCard_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bonuscard = db.BonusCard.Where(i => i.IdCard.ToString().Equals(Tb_NumberCard.Text));
            if (bonuscard.Count() == 1)
            {
                decimal DiscountBD = bonuscard.First().Status.Discount;
                decimal Discount = 1 - (DiscountBD / 100);
                decimal newprice = finalSum * Discount;
                Tb_FinalSum.Text = newprice.ToString();
                PriceDiscount.Text = (finalSum - newprice).ToString() + " Р";
                CardAdded = true;
            }
            else
            {
                CardAdded = false;
            }
        }
    }
}
