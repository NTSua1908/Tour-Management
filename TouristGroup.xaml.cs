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
            foreach (var item in touristView.ListVehicles)
            {
                listVehicle.SelectedItems.Add(item.PhuongTien);
            }
        }

        public void CreateSelectedHotels()
        {
            foreach (var item in touristView.ListHotels)
            {
                listHotels.SelectedItems.Add(item.KhachSan);
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
