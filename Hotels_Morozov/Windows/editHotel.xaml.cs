using Hotels_Morozov.Classes;
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
using System.Windows.Shapes;

namespace Hotels_Morozov.Windows
{
    /// <summary>
    /// Логика взаимодействия для editHotel.xaml
    /// </summary>
    public partial class editHotel : Window
    {
        bool updateFlag = false;
        Hotel hotel;
        void uploadFields()
        {
            countryCB.ItemsSource = DBHelper.hE.Country.ToList();
            countryCB.SelectedValuePath = "Id";
            countryCB.DisplayMemberPath = "Name";
        }

        public editHotel()
        {
            InitializeComponent();
            uploadFields();
            editHotelBTN.Content = "Добавить";
        }

        public editHotel(Hotel hotel)
        {
            InitializeComponent();
            uploadFields();
            this.hotel = hotel;

            hotelNameTB.Text = hotel.Name;
            countOfStarsTB.Text = hotel.CountOfStars.ToString();
            hotelDescriptionTB.Text = hotel.Description;
            countryCB.SelectedItem = hotel.Country;

            updateFlag = true;

            editHotelBTN.Content = "Изменить";
        }

        private void editHotelBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(hotelNameTB.Text) && !String.IsNullOrEmpty(hotelDescriptionTB.Text) && !String.IsNullOrEmpty(countOfStarsTB.Text) && countryCB.SelectedIndex != -1)
            {
                if (updateFlag)
                {
                    hotel.Name = hotelNameTB.Text;
                    hotel.Description = hotelDescriptionTB.Text;
                    hotel.CountOfStars = Convert.ToInt32(countOfStarsTB.Text);
                    hotel.Country = countryCB.SelectedItem as Country;

                    DBHelper.hE.SaveChanges();
                    MessageBox.Show("Изменения внесены!", "Уведомление");
                    this.Close();
                }
                else
                {
                    Hotel hotel = new Hotel()
                    {
                        Name = hotelNameTB.Text,
                        Description = hotelDescriptionTB.Text,
                        CountOfStars = Convert.ToInt32(countOfStarsTB.Text),
                        Country = countryCB.SelectedItem as Country
                    };
                    DBHelper.hE.Hotel.Add(hotel);
                    DBHelper.hE.SaveChanges();

                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Заполнены не все поля!", "Ошибка");
            }
        }

        private void countOfStarsTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(countOfStarsTB.Text, @"[0-5]") || countOfStarsTB.Text.Length > 1)
            {
                if(countOfStarsTB.Text.Length != 0)
                    countOfStarsTB.Text = countOfStarsTB.Text.Substring(0, countOfStarsTB.Text.Length - 1);
            }
        }
    }
}
