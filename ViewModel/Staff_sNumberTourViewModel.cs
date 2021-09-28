using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class Staff_sNumberTourViewModel : BaseViewModel
    {
        public ICommand SearchCommand { get; set; }

        private ObservableCollection<StaffAnalysis> _lstTourNumber;
        public ObservableCollection<StaffAnalysis> lstTourNumber
        {
            get { return _lstTourNumber; }
            set { _lstTourNumber = value; OnPropertyChanged(); }
        }

        private DateTime _FromDay;
        public DateTime FromDay { get { return _FromDay; } set { _FromDay = value; OnPropertyChanged(); } }

        private DateTime _ToDay;
        public DateTime ToDay { get { return _ToDay; } set { _ToDay = value; OnPropertyChanged(); } }

        private List<NhanVien> lstNhanVien;

        public Staff_sNumberTourViewModel()
        {
            FromDay = DateTime.Now;
            ToDay = DateTime.Now;

            lstTourNumber = new ObservableCollection<StaffAnalysis>();
            lstNhanVien = new List<NhanVien>(DataProvider.Ins.Entities.NhanViens);
            foreach (NhanVien item in lstNhanVien)
            {
                lstTourNumber.Add(new StaffAnalysis
                {
                    NhanVien = item,
                    Quantity = 0
                });
            }

            getReport();


            SearchCommand = new RelayCommand<object>((p) =>
            {
                return DateTime.Compare(ToDay, FromDay) >= 0;
            }, (p) =>
            {
                //MessageBox.Show(ToDay.ToString() + "\n" + FromDay.ToString() + "\n" + (DateTime.Compare(ToDay, FromDay) >= 0).ToString());
                getReport();
            });
        }

        private void getReport()
        {
            int index = -1;
            foreach (NhanVien item in lstNhanVien)
            {
                index++;
                int count = DataProvider.Ins.Entities.DSNhanViens.Where(x => x.MaNV == item.MaNV
                        && x.DoanDuLich.NgayKhoiHanh >= FromDay && x.DoanDuLich.NgayKetThuc <= ToDay).Count(); //
                lstTourNumber[index].Quantity = count;
            }
        }
    }
}
