using Hotels_Morozov.Classes;
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
    /// Логика взаимодействия для toursPage.xaml
    /// </summary>
    public partial class toursPage : Page
    {
        public toursPage()
        {
            InitializeComponent();
            toursLV.ItemsSource = DBHelper.hE.Tour.ToList();
            toursLV.SelectedValuePath = "Id";

            List<Type> allTypes = DBHelper.hE.Type.ToList();
            allTypes.Insert(0, new Type()
            {
                Name = "Все типы"
            });
            typeTourCB.ItemsSource = allTypes;
            typeTourCB.DisplayMemberPath = "Name";
            typeTourCB.SelectedIndex = 0;

            orderByCB.SelectedIndex = 0;
        }
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as TextBlock).Uid);
            Tour tour = DBHelper.hE.Tour.FirstOrDefault(x => x.Id == index);

            (sender as TextBlock).Text = $"{Math.Round(tour.Price)} РУБ";
        }

        private void actualityTB_Loaded(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as TextBlock).Uid);
            Tour tour = DBHelper.hE.Tour.FirstOrDefault(x => x.Id == index);

            if (tour.IsActual)
            {
                (sender as TextBlock).Text = "Актуален";
                (sender as TextBlock).Foreground = new SolidColorBrush(Color.FromRgb(29, 165, 29));
            }
            else
            {
                (sender as TextBlock).Text = "Не актуален";
                (sender as TextBlock).Foreground = new SolidColorBrush(Color.FromRgb(212, 43, 43));
            }
        }

        private void ticketCountTB_Loaded(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as TextBlock).Uid);
            Tour tour = DBHelper.hE.Tour.FirstOrDefault(x => x.Id == index);

            (sender as TextBlock).Text = $"Билетов: {tour.TicketCount}";
        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            filter();
        }

        private void typeTourCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void onlyActualCB_Checked(object sender, RoutedEventArgs e)
        {
            filter();
        }

        void filter()
        {
            List<Tour> tours = DBHelper.hE.Tour.ToList();

            if (!String.IsNullOrEmpty(searchTB.Text))
            {
                tours = tours.Where(x => x.Name.ToString().ToLower().Contains(searchTB.Text.ToLower())).ToList();
            }
            if ((bool)onlyActualCB.IsChecked)
            {
                tours = tours.Where(x => x.IsActual).ToList();
            }
            if (typeTourCB.SelectedIndex != 0)
            {
                tours = tours.Where(x => x.Type.Contains(typeTourCB.SelectedValue)).ToList();
            }
            switch (orderByCB.SelectedIndex)
            {
                case 1:
                    tours = tours.OrderByDescending(x => x.Price).ToList();
                    break;
                case 2:
                    tours = tours.OrderBy(x => x.Price).ToList();
                    break;
            }

            double totalPrice = 0;

            foreach (var item in tours)
            {
                totalPrice += Convert.ToDouble(item.TicketCount * item.Price);
            }

            totalPriceTB.Text = $"Общая стоимость всех отображающихся туров - {totalPrice}";

            toursLV.ItemsSource = tours;
        }

        private void onlyActualCB_Unchecked(object sender, RoutedEventArgs e)
        {
            filter();
        }

        private void imageTourI_Loaded(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32((sender as Image).Uid);

            Tour tour = DBHelper.hE.Tour.FirstOrDefault(x => x.Id == index);

            if (tour.ImagePreview != null)
            {
                string path = Environment.CurrentDirectory;
                path = path.Replace("bin\\Debug", tour.ImagePreview);
                (sender as Image).Source = BitmapFrame.Create(new Uri(path));
            }
            else
            {
                string path = Environment.CurrentDirectory;
                path = path.Replace("bin\\Debug", "Images\\picture.png");
                (sender as Image).Source = BitmapFrame.Create(new Uri(path));
            }
        }

        private void orderByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }
    }
}
