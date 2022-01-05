using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tour_management.ViewModel;
using Tour_management.Model;

namespace Tour_management
{
    /// <summary>
    /// Interaction logic for TouristGroup.xaml
    /// </summary>
    public partial class TouristGroup : Window
    {
        TouristViewModel touristView = new TouristViewModel();
        public TouristGroup()
        {
            InitializeComponent();
            this.DataContext = touristView;                        
        }

        public void CreateSelectedVehicles()
        {

            foreach (var item in touristView.SelectedVehicles)
            {
                listVehicle.SelectedItems.Add(item);
            }

            //foreach (var item in touristView.SelectedVehicles)
            //{
            //    for (int i = 0; i < listVehicle.Items.Count; i++)
            //    {
            //        if (listVehicle.Items[i] == item)
            //        {
            //            MessageBox.Show("here");
            //            listVehicle.SelectedItems.Add(listVehicle.Items[i]);
            //        }
            //    }
            //}

            //listVehicle.SelectedItems.Add(listVehicle.Items[1]);
        }

        public void CreateSelectedHotels()
        {

            //listHotel.SelectedItems.Add(listHotel.Items[1]);
            Type type = listHotel.Items.GetType();

            foreach (var item in touristView.SelectedHotels)
            {

            }
        }
    }

    public class FutureDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out time)) return new ValidationResult(false, "Không hợp lệ");

            return time.Date <= DateTime.Now.Date
                ? new ValidationResult(false, "Future date required")
                : ValidationResult.ValidResult;
        }
    }
}
