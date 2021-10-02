using LiveCharts;
using LiveCharts.Wpf;
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
    class StatisticSalesViewModel : BaseViewModel
    {
        public ICommand SearchCommand { get; set; }

        private ObservableCollection<Tour> _lstTour;
        public ObservableCollection<Tour> lstTour
        {
            get { return _lstTour; }
            set { _lstTour = value; OnPropertyChanged(); }
        }

        private Tour _SelectedTour;
        public Tour SelectedTour { get { return _SelectedTour; } set { _SelectedTour = value; OnPropertyChanged(); } }

        private DateTime _FromDay;
        public DateTime FromDay { get { return _FromDay; } set { _FromDay = value; OnPropertyChanged(); } }

        private DateTime _ToDay;
        public DateTime ToDay { get { return _ToDay; } set { _ToDay = value; OnPropertyChanged(); } }

        public SeriesCollection SeriesSelection { get; private set; }


        //Doanh so Tour
        private SeriesCollection _SeriesSelectionAllTour;
        public SeriesCollection SeriesSelectionAllTour { 
            get { return _SeriesSelectionAllTour; } 
            set { _SeriesSelectionAllTour = value; OnPropertyChanged(); }
        }


        private AxesCollection _AxisYCollectionAllTour;
        public AxesCollection AxisYCollectionAllTour
        {
            get { return _AxisYCollectionAllTour; }
            set { _AxisYCollectionAllTour = value; OnPropertyChanged(); }
        }

        private AxesCollection _AxisXCollectionAllTour;
        public AxesCollection AxisXCollectionAllTour
        {
            get { return _AxisXCollectionAllTour; }
            set { _AxisXCollectionAllTour = value; OnPropertyChanged(); }
        }

        Func<double, string> formatter = value => value.ToString("#,###,###.##"); //Định dạng số tiền hiển thị ra

        public StatisticSalesViewModel()
        {
            FromDay = DateTime.Now.AddMonths(-12);
            ToDay = DateTime.Now;

            AllTourStatistics();

            lstTour = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return (ToDay >= FromDay) && (SelectedTour != null);
            }, (p) =>
            {
                AllTourStatistics();
            });
        }


        private void AllTourStatistics()
        {
            SeriesSelectionAllTour = new SeriesCollection();
            int month = getMonth(ToDay, FromDay) + 1; //Lay so thang

            List<Tour> lstTour = new List<Tour>(DataProvider.Ins.Entities.Tours);
            foreach (Tour item in lstTour)
            {
                LineSeries line = new LineSeries(); //Với mỗi tour là một đường trong biểu đồ
                line.Title = item.TenTour;
                line.ScalesYAt = 0;

                ChartValues<decimal> DoanhThu = new ChartValues<decimal>(); //Dùng để lưu doanh thu từng tháng

                for (int i = 0; i <= month; i++) //số tháng
                {
                    DoanhThu.Add(0); //Khởi tạo giá trị mặc định là 0
                }

                //Lấy danh sách các đoàn của tour đang xét
                List<DoanDuLich> lstGroup = new List<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaTour == item.MaTour));
                foreach (DoanDuLich doan in lstGroup)
                {
                    if (doan.NgayKetThuc.Value >= FromDay && doan.NgayKetThuc.Value <= ToDay)
                    {
                        decimal valueIn = (int)doan.SoLuong * (decimal)item.GiaTour * (decimal)item.LoaiTour.HeSo; //Tien thu
                        decimal valueOut = (decimal)doan.TongGiaAU + (decimal)doan.TongGiaKS + (decimal)doan.TongGiaPT + (decimal)doan.ChiPhiKhac; //Tien chi
                        decimal revenue = valueIn - valueOut; //Doanh thu

                        //if (getMonth(FromDay, doan.NgayKetThuc.Value) > 0)
                        //    MessageBox.Show(FromDay.ToString() + " " + doan.NgayKetThuc.ToString());

                        DoanhThu[getMonth(FromDay, doan.NgayKetThuc.Value) + 1] += revenue;
                    }
                }

                line.Values = DoanhThu;
                SeriesSelectionAllTour.Add(line);
            }

            AxisYCollectionAllTour = new AxesCollection
            {
                new Axis { Title = "Danh thu Tour (VNĐ)", LabelFormatter= formatter }
            };
            AxisXCollectionAllTour = new AxesCollection()
            {
                new Axis{Title = "Doanh thu", MinValue=0, MaxValue= month}
            };
        }

        /// <summary>
        /// Get month from StartDay to EndDate
        /// </summary>
        /// <param name="StartDay"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        private int getMonth(DateTime StartDay, DateTime EndDate)
        {
            return Math.Abs(12 * (StartDay.Year - EndDate.Year) + StartDay.Month - EndDate.Month);
        }
    }
}
