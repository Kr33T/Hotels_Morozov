using Hotels_Morozov.Classes;
using Hotels_Morozov.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            cfp.CountPage = hotels.Count;
            DataContext = cfp;
            try
            {
                pageCountTB.Text = "10";
            }
            catch
            {

            }
            refreshTotalRecords();
        }

        void refreshDataGrid()
        {
            hotels = DBHelper.hE.Hotel.ToList();
            listOfHotels.ItemsSource = hotels;
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
            refreshTotalRecords();
        }

        void refreshTotalRecords()
        {
            totalRecordsTB.Text = $"Записей выведено: {listOfHotels.Items.Count}/{hotels.Count}";
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
                int index = (listOfHotels.SelectedItem as Hotel).Id;
                Hotel hotel = DBHelper.hE.Hotel.FirstOrDefault(x => x.Id == index);
                List<HotelOfTour> hot = DBHelper.hE.HotelOfTour.Where(x => x.HotelId == hotel.Id).ToList();
                List<Tour> tours = new List<Tour>();
                foreach (var item in hot)
                {
                    tours.Add(DBHelper.hE.Tour.FirstOrDefault(x => x.Id == item.TourId && x.IsActual));
                }
                if (tours.Count == 0)
                {
                    var res = MessageBox.Show($"Вы уверены, что хотите удалить {hotel.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        foreach (var item in DBHelper.hE.HotelImage)
                        {
                            if (item.HotelId == hotel.Id)
                            {
                                DBHelper.hE.HotelImage.Remove(item);
                            }
                        }

                        foreach (var item in DBHelper.hE.HotelComment)
                        {
                            if (item.HotelId == hotel.Id)
                            {
                                DBHelper.hE.HotelComment.Remove(item);
                            }
                        }

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
                    MessageBox.Show("Отель подходит под актуальный тур, поэтому его нельзя удалить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Question);
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
            refreshTotalRecords();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(pageCountTB.Text, @"^[0-9]*$"))
            {
                if(pageCountTB.Text.Length != 0)
                    pageCountTB.Text = pageCountTB.Text.Substring(0, pageCountTB.Text.Length - 1);
            }
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
            refreshTotalRecords();
        }

        private void listOfHotels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void countToursTB_Loaded(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as TextBlock).Uid);

            int count = DBHelper.hE.HotelOfTour.Where(x => x.HotelId == index).Count();

            (sender as TextBlock).Text = count.ToString();
        }
    }
}
