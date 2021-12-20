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

        private SeriesCollection _SeriesSelectionAllTourGroup;
        public SeriesCollection SeriesSelectionAllTourGroup
        {
            get { return _SeriesSelectionAllTourGroup; }
            set { _SeriesSelectionAllTourGroup = value; OnPropertyChanged(); }
        }


        private AxesCollection _AxisYCollectionAllTourGroup;
        public AxesCollection AxisYCollectionAllTourGroup
        {
            get { return _AxisYCollectionAllTourGroup; }
            set { _AxisYCollectionAllTourGroup = value; OnPropertyChanged(); }
        }

        private AxesCollection _AxisXCollectionAllTourGroup;
        public AxesCollection AxisXCollectionAllTourGroup
        {
            get { return _AxisXCollectionAllTourGroup; }
            set { _AxisXCollectionAllTourGroup = value; OnPropertyChanged(); }
        }
        private SeriesCollection _SeriesSelectionNumberofTourGroup;
        public SeriesCollection SeriesSelectionNumberofTourGroup
        {
            get { return _SeriesSelectionNumberofTourGroup; }
            set { _SeriesSelectionNumberofTourGroup = value; OnPropertyChanged(); }
        }

        private AxesCollection _AxisYCollectionNumberofTourGroup;
        public AxesCollection AxisYCollectionNumberofTourGroup
        {
            get { return _AxisYCollectionNumberofTourGroup; }
            set { _AxisYCollectionNumberofTourGroup = value; OnPropertyChanged(); }
        }

        private AxesCollection _AxisXCollectionNumberofTourGroup;
        public AxesCollection AxisXCollectionNumberofTourGroup
        {
            get { return _AxisXCollectionNumberofTourGroup; }
            set { _AxisXCollectionNumberofTourGroup = value; OnPropertyChanged(); }
        }
        Func<double, string> formatter = value => value.ToString("#,###,###.##"); //Định dạng số tiền hiển thị ra

        public StatisticSalesViewModel()
        {
            FromDay = DateTime.Now.AddMonths(-12);
            ToDay = DateTime.Now;
            int month = getMonth(ToDay, FromDay) + 1;
            AllTourStatistics();
            AllTourgroupStatistics();
            NumberofTourGroup();

            AxisYCollectionNumberofTourGroup = new AxesCollection
            {
                new Axis { Title = "Số đoàn", LabelFormatter= formatter }
            };
            AxisXCollectionNumberofTourGroup = new AxesCollection()
            {
                new Axis{Title = "Doanh thu", MinValue=0, MaxValue= month }
            };
            lstTour = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return (ToDay >= FromDay) && (SelectedTour != null);
            }, (p) =>
            {
                AllTourStatistics();
                AllTourgroupStatistics();
                NumberofTourGroup();
            });
        }

        private void NumberofTourGroup()
        {
            if(SelectedTour == null)
                return;   

            SeriesSelectionNumberofTourGroup = new SeriesCollection();
            int month = getMonth(ToDay, FromDay) + 1;

            

            LineSeries line = new LineSeries(); //Với mỗi tour là một đường trong biểu đồ
            line.ScalesYAt = 0;
            ChartValues<decimal> Number = new ChartValues<decimal>();
            for (int i = 0; i <= month; i++) //số tháng
            {
                Number.Add(0); //Khởi tạo giá trị mặc định là 0
            }
            List<DoanDuLich> lstGroup = new List<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaTour == SelectedTour.MaTour));
            foreach (DoanDuLich doan in lstGroup)
            {
                if (doan.NgayKetThuc.Value >= FromDay && doan.NgayKetThuc.Value <= ToDay)
                {
                    Number[getMonth(FromDay, doan.NgayKetThuc.Value) + 1] += 1;
                }

            }
            line.Values = Number;
            //line.Title = Number.ToString();
            line.Title = SelectedTour.TenTour;
            SeriesSelectionNumberofTourGroup.Add(line);

            AxisYCollectionNumberofTourGroup = new AxesCollection
            {
                new Axis { Title = "Số đoàn", LabelFormatter= formatter }
            };
            AxisXCollectionNumberofTourGroup = new AxesCollection()
            {
                new Axis{Title = "Doanh thu", MinValue=0, MaxValue= month }
            };

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
                        if (doan.TongGiaAU == null || doan.TongGiaKS == null || doan.TongGiaPT == null || doan.ChiPhiKhac == null)
                            continue;
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

        private void AllTourgroupStatistics()
        {
            SeriesSelectionAllTourGroup = new SeriesCollection();
            int month = getMonth(ToDay, FromDay) + 1 ;

            List<DoanDuLich> lstDoanDuLiches = new List<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches);
            foreach (DoanDuLich tourgroup in lstDoanDuLiches)
            {
                LineSeries line = new LineSeries();
                line.Title = tourgroup.TenDoan;
                line.ScalesYAt = 0;

                ChartValues<decimal> Sales = new ChartValues<decimal>(); 

                for (int i = 0; i <= month; i++)
                {
                    Sales.Add(0);
                }

                if (tourgroup.NgayKetThuc.Value >= FromDay && tourgroup.NgayKetThuc.Value <= ToDay)
                {
                    if (tourgroup.TongGiaAU != null && tourgroup.TongGiaKS != null && tourgroup.TongGiaPT != null && tourgroup.ChiPhiKhac != null)
                    {
                        decimal valueIn = (int)tourgroup.SoLuong * (decimal)tourgroup.Tour.GiaTour * (decimal)tourgroup.Tour.LoaiTour.HeSo; //Tien thu
                        decimal valueOut = (decimal)tourgroup.TongGiaAU + (decimal)tourgroup.TongGiaKS + (decimal)tourgroup.TongGiaPT + (decimal)tourgroup.ChiPhiKhac; //Tien chi
                        decimal revenue = valueIn - valueOut;

                        Sales[getMonth(FromDay, tourgroup.NgayKetThuc.Value) + 1] += revenue;
                    }
                }

                line.Values = Sales;
                SeriesSelectionAllTourGroup.Add(line);

            }

            AxisYCollectionAllTourGroup = new AxesCollection
            {
                new Axis { Title = "Danh thu DoanDuLich (VNĐ)", LabelFormatter= formatter }
            };
            AxisXCollectionAllTourGroup = new AxesCollection()
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
