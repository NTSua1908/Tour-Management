﻿using System;
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

namespace Tour_management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

<<<<<<< HEAD
            //Test man hinh
            //CustomerManagement customer = new CustomerManagement();
            //customer.ShowDialog();
            //DestinationManagement destination = new DestinationManagement();
            //destination.ShowDialog();
            TouristGroupManagment t = new TouristGroupManagment();
            t.ShowDialog();
=======
<<<<<<< HEAD
            Staff_sNumberTour t = new Staff_sNumberTour();
            t.ShowDialog();
            //HotelManagement t = new HotelManagement();
=======
            //StatisticSales t = new StatisticSales();
>>>>>>> f5111c47ef177b7a38d5aff3e3f6b4c00a8b9180
            //t.ShowDialog();
>>>>>>> ecf517dc9b54bc60e9f249b22dd0d8994a8a3051
        }
    }
}
