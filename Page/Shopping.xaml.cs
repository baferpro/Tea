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
        List<Basket> gBasketList = new List<Basket>();
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
                bool bProductHave = false;
                for(int g = 0; g < gBasketList.Count; g++)
                {
                    if(gBasketList[g].product.IdProduct == gProductBascet[i].IdProduct)
                    {
                        gBasketList[g].TotalPrice += gBasketList[g].product.Price;
                        gBasketList[g].Count++;
                        bProductHave = true;
                    }
                }
                if (!bProductHave)
                {
                    gBasketList.Add(new Basket
                    {
                        product = gProductBascet[i],
                        TotalPrice = gProductBascet[i].Price,
                        Count = 1
                    });
                }
            }

            Lv_Bascet.ItemsSource = gBasketList.ToList();

            UpdateAll();

            Btn_Checkout.IsEnabled = false;
        }

        private void UpdateAll()
        {
            countProduct = 0;
            for (int i = 0; i < gBasketList.Count; i++)
            {
                countProduct += gBasketList[i].Count;
            }
            Tb_CountBascet.Text = countProduct.ToString();

            CheckSumm();
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
                int cardID = Convert.ToInt32(Tb_NumberCard.Text);
                var card = db.BonusCard.Where(i => i.IdCard == cardID).FirstOrDefault();
                card.CountBall += finalSum * Convert.ToDecimal(0.1);
                sale = db.Sale.Add(new Sale
                {
                    IdEmployee = gUser.IdEmployee,
                    Date = DateTime.Now,
                    IdBonusCard = cardID
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
                var product = db.Product.Where(g => g.IdProduct == idProduct).FirstOrDefault();
                product.Quantity -= 1;
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
                TBDiscount.Text = $"({DiscountBD}%)";
                decimal Discount = 1 - (DiscountBD / 100);
                decimal newprice = finalSum * Discount;
                PriceDiscount.Text = (finalSum - newprice).ToString();
                finalSum = newprice;
                CheckSumm(false);
                CardAdded = true;
            }
            else
            {
                CheckSumm();
                CardAdded = false;
            }
        }

        public void CheckSumm(bool Check = true)
        {
            if (Check)
            {
                finalSum = 0;
                for (int i = 0; i < gProductBascet.Count; i++)
                {
                    finalSum += gProductBascet[i].Price;
                }

                PriceDiscount.Text = "0";
                TBDiscount.Text = "(0%)";
                Tb_FinalSum.Text = finalSum.ToString();
            }
            else
            {
                Tb_FinalSum.Text = finalSum.ToString();
            }
        }

        private void BTNMinusCount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var BasketItem = button.DataContext as Basket;
            if(BasketItem.Count>1)
            {
                BasketItem.Count--;
                BasketItem.TotalPrice -= BasketItem.product.Price;
            }
            else
            {
                gBasketList.Remove(BasketItem);
            }
            gProductBascet.Remove(BasketItem.product);
            Lv_Bascet.ItemsSource = gBasketList.ToList();
            UpdateAll();
        }

        private void BTNPlusCount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var BasketItem = button.DataContext as Basket;
            BasketItem.Count++;
            BasketItem.TotalPrice += BasketItem.product.Price;
            gProductBascet.Add(BasketItem.product);
            Lv_Bascet.ItemsSource = gBasketList.ToList();
            UpdateAll();
        }
    }
    public class Basket
    {
        public Product product { get; set; }
        public int TotalPrice { get; set; }
        public int Count { get; set; }
    }
}
