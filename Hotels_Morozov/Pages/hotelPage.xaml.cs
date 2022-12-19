using Hotels_Morozov.Classes;
using Hotels_Morozov.Windows;
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

namespace Hotels_Morozov.Pages
{
    /// <summary>
    /// Логика взаимодействия для hotelPage.xaml
    /// </summary>
    public partial class hotelPage : Page
    {
        classForPaginator cfp = new classForPaginator();
        List<Hotel> hotels = DBHelper.hE.Hotel.ToList();
        public hotelPage()
        {
            InitializeComponent();
            hotels = DBHelper.hE.Hotel.ToList();
            listOfHotels.ItemsSource = hotels;
            totalRecordsTB.Text = $"Записей выведено: {hotels.Count}";
            cfp.CountPage = hotels.Count;
            DataContext = cfp;
            try
            {
                pageCountTB.Text = "10";
            }
            catch
            {

            }
        }

        void refreshDataGrid()
        {
            hotels = DBHelper.hE.Hotel.ToList();
            listOfHotels.ItemsSource = hotels;
            totalRecordsTB.Text = $"Записей выведено: {hotels.Count}";
            cfp.CountPage = hotels.Count;
            DataContext = cfp;
            try
            {
                cfp.CountPage = Convert.ToInt32(pageCountTB.Text);
            }
            catch
            {
                cfp.CountPage = hotels.Count;
            }
            cfp.Countlist = hotels.Count;
            listOfHotels.ItemsSource = hotels.Skip(0).Take(cfp.CountPage).ToList();
            cfp.CurrentPage = 1;
        }

        private void updateHotel_Click(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as Button).Uid);
            Hotel h = DBHelper.hE.Hotel.FirstOrDefault(x => x.Id == index);
            editHotel hotel = new editHotel(h);
            hotel.Show();

            hotel.Closing += (obj, args) =>
            {
                refreshDataGrid();
            };
        }

        private void addHotelBTN_Click(object sender, RoutedEventArgs e)
        {
            editHotel hotel = new editHotel();
            hotel.Show();

            hotel.Closing += (obj, args) =>
            {
                refreshDataGrid();
            };
        }

        private void deleteHotelBTN_Click(object sender, RoutedEventArgs e)
        {
            if (listOfHotels.SelectedItem != null)
            {
                var res = MessageBox.Show("Вы уверены, что хотите удалить", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    DBHelper.hE.Hotel.Remove(listOfHotels.SelectedItem as Hotel);
                    DBHelper.hE.SaveChanges();
                    refreshDataGrid();
                }
                else
                {
                    MessageBox.Show("Удаление отменено!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Отель не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Question);
            }
        }
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            switch (tb.Uid)
            {
                case "prev":
                    cfp.CurrentPage--;
                    break;
                case "next":
                    cfp.CurrentPage++;
                    break;
                case "firstOne":
                    cfp.CurrentPage = 1;
                    break;
                case "lastOne":
                    cfp.CurrentPage = cfp.CountPages;
                    break;
                default:
                    cfp.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            listOfHotels.ItemsSource = hotels.Skip(cfp.CurrentPage * cfp.CountPage - cfp.CountPage).Take(cfp.CountPage).ToList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                cfp.CountPage = Convert.ToInt32(pageCountTB.Text);
            }
            catch
            {
                cfp.CountPage = hotels.Count;
            }
            cfp.Countlist = hotels.Count;
            listOfHotels.ItemsSource = hotels.Skip(0).Take(cfp.CountPage).ToList();
            cfp.CurrentPage = 1;
        }

        private void listOfHotels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void countToursTB_Loaded(object sender, RoutedEventArgs e)
        {
            int idnex = Convert.ToInt32((sender as TextBlock).Uid);

            List<Tour> tours = DBHelper.hE.Tour.ToList();

            foreach (var item in DBHelper.hE.Tour.Include(x => x.Hotel))
            {

            }
        }
    }
}
