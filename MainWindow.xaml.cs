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

            //Test man hinh
<<<<<<< HEAD
            //CustomerManagement customer = new CustomerManagement();
            //customer.ShowDialog();
            //DestinationManagement destination = new DestinationManagement();
            //destination.ShowDialog();
            TouristGroup t = new TouristGroup();
            t.ShowDialog();
=======

            //RegisterAccount register = new RegisterAccount();
            //register.ShowDialog();
>>>>>>> c29e09f686da78a843ce392420134d71112f4dfa
        }
    }
}
