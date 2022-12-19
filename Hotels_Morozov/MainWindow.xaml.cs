using Hotels_Morozov.Classes;
using Hotels_Morozov.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace Hotels_Morozov
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DBHelper.hE = new hotelsEntities();
            frameClass.mainFrame = mainFrame;
            frameClass.mainFrame.Navigate(new toursPage());
        }

        private void openHotelPageBTN_Click(object sender, RoutedEventArgs e)
        {
            frameClass.mainFrame.Navigate(new hotelPage());
        }

        private void openToursPageBTN_Click(object sender, RoutedEventArgs e)
        {
            frameClass.mainFrame.Navigate(new toursPage());
        }
    }
}
