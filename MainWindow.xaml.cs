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
<<<<<<< HEAD

            //Test man hinh
            //DestinationManagement destination = new DestinationManagement();
            //destination.ShowDialog();
            TouristGroupManagment tg = new TouristGroupManagment();
            tg.Show();
=======
>>>>>>> 1586f6552c07058cf67936dfc00638b7f97195a5
        }
    }
}
