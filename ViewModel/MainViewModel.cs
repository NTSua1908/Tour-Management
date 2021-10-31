using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }

        public ICommand AreaCommand { get; set; }
        public ICommand DestinationCommand { get; set; }
        public ICommand VehicleCommand { get; set; }
        public ICommand HotelCommand { get; set; }
        public ICommand InformationCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand GroupCommand { get; set; }
        public ICommand TourCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand ManageStaffCommand { get; set; }
        public ICommand ManageUserCommand { get; set; }
        public ICommand TourTypeCommand { get; set; }
        public ICommand AddUserCommand { get; set; }
        public ICommand AddGroupCommand { get; set; }
        public ICommand StaffAnalysisCommand { get; set; }

        private User _user;
        public User user { get { return _user; } set { _user = value; OnPropertyChanged(); } }
        
        private ImageSource _Avatar;
        public ImageSource Avatar { get { return _Avatar; } set { _Avatar = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }

        private SeriesCollection _SeriesSelection;
        public SeriesCollection SeriesSelection { get { return _SeriesSelection; } set { _SeriesSelection = value; OnPropertyChanged(); } }

        public AxesCollection AxisYCollection { get; set; }
        public AxesCollection AxisXCollection { get; set; }

        public MainViewModel()
        {
            AddReport();
            
            //Goi ham nay de thuc hien dang nhap
            LoadedCommand = new RelayCommand<Window>((p) => { return true; },  (p) => { Login(p); } );

            LogoutCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Bạn có muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    Login(p);
                }
            });

            AreaCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                AreaManagement area = new AreaManagement();
                area.ShowDialog();
            });
            
            DestinationCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                DestinationManagement destination = new DestinationManagement();
                destination.ShowDialog();
            });

            VehicleCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                VehicleManagement vehicle = new VehicleManagement();
                vehicle.ShowDialog();
            });

            HotelCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                HotelManagement hotel = new HotelManagement();
                hotel.ShowDialog();
            });

            InformationCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                PersonalInformation information = new PersonalInformation();
                PersonalInformationViewModel viewModel = information.DataContext as PersonalInformationViewModel;
                viewModel.setUser(user);

                information.ShowDialog();
                Avatar = new BitmapImage(new Uri("pack://application:,,,/Tour%20management;component/Resources/avatar" + user.Avatar + ".png", UriKind.Absolute));
                DisplayName = user.HoTen;
            });

            GroupCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                TouristGroupManagment group = new TouristGroupManagment();
                group.ShowDialog();
            });
            
            TourCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                TourWindow tour = new TourWindow();
                tour.ShowDialog();
            });

            CustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                CustomerManagement customer = new CustomerManagement();
                customer.ShowDialog();
            });

            ReportCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                StatisticSales statistic = new StatisticSales();
                statistic.ShowDialog();
            });


            ManageStaffCommand = new RelayCommand<Window>((p) => { return isAdmin(); }, (p) => {
                StaffManagement staff = new StaffManagement();
                staff.ShowDialog();
            });

            ManageUserCommand = new RelayCommand<Window>((p) => { return isAdmin(); }, (p) => {
                UserManagement user = new UserManagement();
                user.ShowDialog();
            });
            
            TourTypeCommand = new RelayCommand<Window>((p) => { return isAdmin(); }, (p) => {
                TourType type = new TourType();
                type.ShowDialog();
            });
            
            AddUserCommand = new RelayCommand<Window>((p) => { return isAdmin(); }, (p) => {
                RegisterAccount register = new RegisterAccount();
                register.ShowDialog();
            });

            AddGroupCommand = new RelayCommand<Window>((p) => { return isAdmin(); }, (p) => {
                TouristGroup group = new TouristGroup();
                group.ShowDialog();
            });

            StaffAnalysisCommand = new RelayCommand<Window>((p) => { return isAdmin(); }, (p) => {
                Staff_sNumberTour numberTour = new Staff_sNumberTour();
                numberTour.ShowDialog();
            });
        }

        /// <summary>
        /// Hiển thị báo cáo doanh thu của các tour trong năm hiện tại
        /// </summary>
        private void AddReport()
        {
            SeriesSelection = new SeriesCollection();

            List<Tour> lstTour = new List<Tour>(DataProvider.Ins.Entities.Tours);
            foreach (Tour item in lstTour) 
            {
                LineSeries line = new LineSeries(); //Với mỗi tour là một đường trong biểu đồ
                line.Title = item.TenTour;
                line.ScalesYAt = 0;

                ChartValues<decimal> DoanhThu = new ChartValues<decimal>(); //Dùng để lưu doanh thu từng tháng
                for (int i = 0; i < 13; i++) //12 tháng
                {
                    DoanhThu.Add(0); //Khởi tạo giá trị mặc định là 0
                }

                //Lấy danh sách các đoàn của tour đang xét
                List<DoanDuLich> lstGroup = new List<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaTour == item.MaTour));
                foreach (DoanDuLich doan in lstGroup)
                {
                    if (doan.NgayKetThuc.Value.Year == DateTime.Now.Year && doan.NgayKetThuc.Value.Month <= DateTime.Now.Month)
                    {
                        if (doan.TongGiaAU == null || doan.TongGiaKS == null || doan.TongGiaPT == null || doan.ChiPhiKhac == null)
                            continue;
                        decimal valueIn = (int)doan.SoLuong * (decimal)item.GiaTour * (decimal)item.LoaiTour.HeSo;
                        //decimal valueOut = (decimal)doan.TongGiaAU + (decimal)doan.TongGiaKS + (decimal)doan.TongGiaPT + (decimal)doan.ChiPhiKhac;
                        //decimal revenue = valueIn - valueOut;

                        //MessageBox.Show(valueIn + " " + valueOut + " " + revenue);

                       // DoanhThu[doan.NgayKetThuc.Value.Month] += revenue;
                    }
                }

                line.Values = DoanhThu;
                SeriesSelection.Add(line);
            }

            Func<double, string> formatter = value => value.ToString("#,###,###.##"); //Định dạng số tiền hiển thị ra

            AxisYCollection = new AxesCollection
            {
                new Axis { Title = "Danh thu Tour (VNĐ)", Foreground = Brushes.Gray, LabelFormatter= formatter }
            };
            AxisXCollection = new AxesCollection()
            {
                new Axis{Title = "Doanh thu", MinValue=1, MaxValue=12 }
            };

        }

        void Login(Window w)
        {
            w.Hide();
            LoginWindow login = new LoginWindow();
            LoginViewModel loginViewModel = (LoginViewModel)login.DataContext;

            loginViewModel.isLogin = false;
            user = null;
            Avatar = null;
            DisplayName = null;

            login.ShowDialog();

            //MessageBox.Show(user.HoTen);

            if (loginViewModel.isLogin)
            {
                w.Show();
                user = loginViewModel.user;
                Avatar = new BitmapImage(new Uri("pack://application:,,,/Tour%20management;component/Resources/avatar" + user.Avatar + ".png", UriKind.Absolute));
                DisplayName = user.HoTen;
            }
            else w.Close();
        }

        private bool isAdmin()
        {
            if (user != null && user.LoaiUser.TenLoai == "admin")
            {
                return true;
            }
            return false;
        }
    }
}
