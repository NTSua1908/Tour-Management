using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class TouristManagementViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private ObservableCollection<DoanDuLich> _lstTouristGr;
        public ObservableCollection<DoanDuLich> lstTouristGr { get { return _lstTouristGr; } set { _lstTouristGr = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachHang> _lstCustomer;
        public ObservableCollection<KhachHang> lstCustomer { get { return _lstCustomer; } set { _lstCustomer = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachDuLich> _lstTourist;
        public ObservableCollection<KhachDuLich> lstTourist { get { return _lstTourist; } set { _lstTourist = value; OnPropertyChanged(); } }

        private ObservableCollection<PhuongTien> _lstVehicle;
        public ObservableCollection<PhuongTien> lstVehicle { get { return _lstVehicle; } set { _lstVehicle = value; OnPropertyChanged(); } }

        private ObservableCollection<DSKhachSan> _lstHotel;
        public ObservableCollection<DSKhachSan> lstHotel { get { return _lstHotel; } set { _lstHotel = value; OnPropertyChanged(); } }

        private ObservableCollection<DSNhanVien> _lstStaff;
        public ObservableCollection<DSNhanVien> lstStaff { get { return _lstStaff; } set { _lstStaff = value; OnPropertyChanged(); } }

        private DoanDuLich _SelectedTouristGr;
        public DoanDuLich SelectedTouristGr { get { return _SelectedTouristGr; } 
            set { 
                _SelectedTouristGr = value; 
                OnPropertyChanged();
                if (SelectedTouristGr != null)
                {
                    lstHotel = new ObservableCollection<DSKhachSan>(DataProvider.Ins.Entities.DSKhachSans.Where(p => (p.MaDoan == SelectedTouristGr.MaDoan)));
                }

            } }
        public TouristManagementViewModel()
        {
            lstTouristGr = new ObservableCollection<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches);
            lstVehicle = new ObservableCollection<PhuongTien>(DataProvider.Ins.Entities.PhuongTiens);

            lstCustomer = new ObservableCollection<KhachHang>(DataProvider.Ins.Entities.KhachHangs);

            //MessageBox.Show(lstTouristGr.Count.ToString(), "thong bao", MessageBoxButton.OK);

        }

    }
}
