using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class TouristViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ManageCommand { get; set; }
        public ICommand AddStaffCommand { get; set; }
        public ICommand DeleteStaffCommand { get; set; }

        private ObservableCollection<DoanDuLich> _lstTouristdb; 
        public ObservableCollection<DoanDuLich> lstTouristdb { get { return _lstTouristdb; } set { _lstTouristdb = value; OnPropertyChanged(); } }

        private ObservableCollection<NhanVien> _lstStaffdb; //danh sách từ database đổ vào listbox
        public ObservableCollection<NhanVien> lstStaffdb { get { return _lstStaffdb; } set { _lstStaffdb = value; OnPropertyChanged(); } }

        private ObservableCollection<PhuongTien> _lstVehicledb;
        public ObservableCollection<PhuongTien> lstVehicledb { get { return _lstVehicledb; } set { _lstVehicledb = value; OnPropertyChanged(); } }

        private ObservableCollection<DiaDiem> _lstDestinationdb;
        public ObservableCollection<DiaDiem> lstDestinationdb { get { return _lstDestinationdb; } set { _lstDestinationdb = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachSan> _lstHoteldb;
        public ObservableCollection<KhachSan> lstHoteldb { get { return _lstHoteldb; } set { _lstHoteldb = value; OnPropertyChanged(); } }

        private ObservableCollection<Tour> _lstTourdb;
        public ObservableCollection<Tour> lstTourdb { get { return _lstTourdb; } set { _lstTourdb = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _lstDutydb;
        public ObservableCollection<string> lstDutydb { get { return _lstDutydb; } set { _lstDutydb = value; OnPropertyChanged(); } }

        private ObservableCollection<PhuongTien> _SelectedVehicles; 
        public ObservableCollection<PhuongTien> SelectedVehicles { get { return _SelectedVehicles; } set { _SelectedVehicles = value; OnPropertyChanged(); } }

        private ObservableCollection<Hotels> _SelectedHotels;
        public ObservableCollection<Hotels> SelectedHotels { get { return _SelectedHotels; } set { _SelectedHotels = value; OnPropertyChanged(); } }

        private ObservableCollection<NhanVien> _SelectedStaffs; //Danh sách nhân viên được chọn trong listbox
        public ObservableCollection<NhanVien> SelectedStaffs { get { return _SelectedStaffs; } set { _SelectedStaffs = value; OnPropertyChanged(); } }

        private ObservableCollection<DSNhanVien> _ListStaffs; //Danh sách nhân viên theo đoàn
        public ObservableCollection<DSNhanVien> ListStaffs { get { return _ListStaffs; } set { _ListStaffs = value; OnPropertyChanged(); } }

        private ObservableCollection<DSPhuongTien> _ListVehicles; //Danh sách pương tiện trong đoàn
        public ObservableCollection<DSPhuongTien> ListVehicles { get { return _ListVehicles; } set { _ListVehicles = value; OnPropertyChanged(); } }

        private ObservableCollection<DSKhachSan> _ListHotels; //Danh sách khách sạn trong đoàn
        public ObservableCollection<DSKhachSan> ListHotels { get { return _ListHotels; } set { _ListHotels = value; OnPropertyChanged(); } }

        private ObservableCollection<Hotels> _lstHotel; //Danh sách khách sạn upload Listbox ds Khách sạn
        public ObservableCollection<Hotels> lstHotel{ get { return _lstHotel; } set { _lstHotel = value; OnPropertyChanged(); } }

        private string _TouristName;
        public string TouristName { get { return _TouristName; } set { _TouristName = value; OnPropertyChanged(); } }

        private Tour _SelectedTour; //Tour được chọn
        public Tour SelectedTour { get { return _SelectedTour; } set { _SelectedTour = value; OnPropertyChanged(); } }

        private DSNhanVien _Selectedstaff;  //Nhân viên được chọn
        public DSNhanVien Selectedstaff { get { return _Selectedstaff; } set { _Selectedstaff = value; OnPropertyChanged(); } }

        private DateTime? _DateStart;
        public DateTime? DateStart { get { return _DateStart; } set { _DateStart = value; OnPropertyChanged(); } }

        private DateTime? _DateEnd;
        public DateTime? DateEnd { get { return _DateEnd; } set { _DateEnd = value; OnPropertyChanged(); } }

        private DateTime _minAvailDate = DateTime.UtcNow;
        public DateTime MinAvailDate
        {
            get { return _minAvailDate; }
            set { _minAvailDate = value; OnPropertyChanged("MinAvailDate"); }
        }

        private string _Amount;
        public string Amount { get { return _Amount; } set { _Amount = value; OnPropertyChanged(); } }

        private string _CostAU;
        public string CostAU { get { return _CostAU; } set { _CostAU = value; OnPropertyChanged(); } }

        private string _OtherCost;
        public string OtherCost { get { return _OtherCost; } set { _OtherCost = value; OnPropertyChanged(); } }

        private string _Detail;
        public string Detail { get { return _Detail; } set { _Detail = value; OnPropertyChanged(); } }

        private string _ButtonAdd;
        public string ButtonAdd { get { return _ButtonAdd; } set { _ButtonAdd = value; OnPropertyChanged(); } }

        private String _SelectedDuty; //Nhiệm vụ được chọn
        public String SelectedDuty { get { return _SelectedDuty; } set { _SelectedDuty = value; OnPropertyChanged(); } }

        private decimal? _PTPrice;
        public decimal? PTPrice { get { return _PTPrice; } set { _PTPrice = value; OnPropertyChanged(); } }

        private DoanDuLich dl;
        private decimal getday(DateTime? StartDay, DateTime? EndDate)
        {
            TimeSpan interval = StartDay.Value.Date - EndDate.Value.Date;
            int day = (int)(interval.TotalMilliseconds / 86400000);
            return day;
        }
        public TouristViewModel()
        {

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            //Danh sách được đổ từ database vào
            lstTourdb = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);
            lstVehicledb = new ObservableCollection<PhuongTien>(DataProvider.Ins.Entities.PhuongTiens);
            lstHoteldb = new ObservableCollection<KhachSan>(DataProvider.Ins.Entities.KhachSans);
            lstStaffdb = new ObservableCollection<NhanVien>(DataProvider.Ins.Entities.NhanViens);

            //Khởi tạo danh sách được chọn
            SelectedVehicles = new ObservableCollection<PhuongTien>();
            SelectedHotels = new ObservableCollection<Hotels>();
            SelectedStaffs = new ObservableCollection<NhanVien>();

            //Khởi tạo danh sách chứa trong đoàn
            ListStaffs = new ObservableCollection<DSNhanVien>();
            ListVehicles = new ObservableCollection<DSPhuongTien>();
            ListHotels = new ObservableCollection<DSKhachSan>();

            //Khởi tạo danh sách khách sạn đổ vào listbox khách sạn
            lstHotel = new ObservableCollection<Hotels>();

            foreach (KhachSan item in lstHoteldb)
            {
                lstHotel.Add(new Hotels
                {
                    Hotel = item,
                    noDay = 0,
                    noNight = 0,
                });
            }

            ButtonAdd = "Thêm";
            MinAvailDate = DateTime.Now;

            lstDutydb = new ObservableCollection<string>();
            lstDutydb.Add("Trưởng đoàn");
            lstDutydb.Add("Phó đoàn");
            lstDutydb.Add("Tài xế");
            lstDutydb.Add("Phục vụ");
            lstDutydb.Add("Hướng dẫn viên");

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) =>
            {
                if (ButtonAdd == "Thêm")
                {
                    DoanDuLich dl = new DoanDuLich()
                    {
                        TenDoan = TouristName,
                        MaTour = SelectedTour.MaTour,
                        NgayKhoiHanh = DateStart,
                        NgayKetThuc = DateEnd,
                        SoLuongToiDa = Convert.ToInt32(Amount),
                        SoLuong = 0,
                        ChiTiet = Detail,
                        TongGiaPT = PTPrice,
                        TongGiaKS = 0,
                        TongGiaAU = 0,
                        ChiPhiKhac = 0,
                        SoLuongGiamGia = 0
                    };

                    if (!String.IsNullOrEmpty(OtherCost))
                    {
                        dl.ChiPhiKhac = Convert.ToInt32(OtherCost);
                    }

                    DataProvider.Ins.Entities.DoanDuLiches.Add(dl);
                    DataProvider.Ins.Entities.SaveChanges();
                    PTPrice = 0;
                    for (int i = 0; i < SelectedVehicles.Count; i++)
                    {
                        DSPhuongTien dspt = new DSPhuongTien()
                        {
                            MaDoan = dl.MaDoan,
                            MaPT = SelectedVehicles[i].MaPT,
                        };

                        //PTPrice += (SelectedVehicles[i].ChiPhi * Math.Abs(getday(Start.Value.Date, End.Value.Date)));

                        DataProvider.Ins.Entities.DSPhuongTiens.Add(dspt);
                    }

                    for (int i = 0; i < SelectedHotels.Count; i++)
                    {
                        DSKhachSan dsks = new DSKhachSan()
                        {
                            MaDoan = dl.MaDoan,
                            KhachSan = SelectedHotels[i].Hotel,
                            SoNgay = SelectedHotels[i].noDay,
                            SoDem = SelectedHotels[i].noNight,
                        };

                        DataProvider.Ins.Entities.DSKhachSans.Add(dsks);
                    }

                    for (int i = 0; i < ListStaffs.Count; i++)
                    {
                        //Thêm danh sách nhân viên tham gia đoàn vào Database
                        DSNhanVien dsnv = new DSNhanVien()
                        {
                            MaDoan = dl.MaDoan,
                            MaNV = ListStaffs[i].MaNV,
                            NhiemVu = ListStaffs[i].NhiemVu,
                        };

                        DataProvider.Ins.Entities.DSNhanViens.Add(dsnv);
                    }
                    dl.TongGiaPT = PTPrice;
                    DataProvider.Ins.Entities.SaveChanges();
                }
                else if (ButtonAdd == "Sửa")
                {

                    DoanDuLich ddlich = DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaDoan == dl.MaDoan).FirstOrDefault();

                    ddlich.TenDoan = TouristName;
                    ddlich.MaTour = SelectedTour.MaTour;
                    ddlich.NgayKhoiHanh = DateStart;
                    ddlich.NgayKetThuc = DateEnd;
                    ddlich.SoLuongToiDa = Convert.ToInt32(Amount);
                    ddlich.ChiTiet = Detail;

                    int ChiPhiAnUong = Convert.ToInt32(CostAU);

                    if (ddlich.SoLuong > ddlich.SoLuongToiDa)
                    {
                        MessageBox.Show("Không thể giảm số lượng tối đa");
                        return;
                    }

                    //if (!String.IsNullOrEmpty(OtherCost))
                    //{
                    //    ddlich.ChiPhiKhac = Convert.ToInt32(OtherCost);
                    //

                    if (ddlich.SoLuong > 0)
                    {
                        ddlich.TongGiaAU = ChiPhiAnUong * ddlich.SoLuong;
                    }

                    List<DSPhuongTien> lstPhuongTien = new List<DSPhuongTien>(DataProvider.Ins.Entities
                        .DSPhuongTiens.Where(x => x.MaDoan == dl.MaDoan));
                    foreach (DSPhuongTien item in lstPhuongTien)
                    {
                        var dd = SelectedVehicles.Where(x => x.MaPT == item.MaPT).FirstOrDefault(); //item co con nam trong danh sach dia diem duoc chon hay khong
                        if (dd == null) //item khong con nam trong danh sach dia diem cua tour nua
                        {
                            //ddlich.TongGiaPT -= (item.PhuongTien.ChiPhi * Math.Abs(getday(Start.Value.Date, End.Value.Date)));
                            DataProvider.Ins.Entities.DSPhuongTiens.Remove(item);
                        }
                    }
                    foreach (PhuongTien item in SelectedVehicles)
                    {
                        var dd = lstPhuongTien.Where(x => x.MaPT == item.MaPT).FirstOrDefault();
                        if (dd == null) //Neu chua co thi them vao
                        {
                            DataProvider.Ins.Entities.DSPhuongTiens.Add(new DSPhuongTien
                            {
                                MaPT = item.MaPT,
                                MaDoan = dl.MaDoan,
                            });

                            //ddlich.TongGiaPT += (item.ChiPhi * Math.Abs(getday(Start.Value.Date, End.Value.Date)));
                        }
                    }

                    List<DSKhachSan> lstKhachSan = new List<DSKhachSan>(DataProvider.Ins.Entities
                        .DSKhachSans.Where(x => x.MaDoan == dl.MaDoan));
                    foreach (DSKhachSan hotel in lstKhachSan)
                    {
                        var ks = SelectedHotels.Where(x => x.Hotel.MaKS == hotel.MaKS).FirstOrDefault();
                        if (ks == null)
                        {
                            //ddlich.TongGiaKS -= (hotel.KhachSan.ChiPhi * ddlich.SoLuong) * Math.Abs(getday(Start.Value.Date, End.Value.Date));
                            DataProvider.Ins.Entities.DSKhachSans.Remove(hotel);
                        }
                    }
                    foreach (Hotels item in SelectedHotels)
                    {
                        var ks = lstKhachSan.Where(x => x.MaKS == item.Hotel.MaKS).FirstOrDefault(); //Xem dia diem duoc chon da nam trong danh sach dia diem ban dau hay khong
                        if (ks == null) //Neu chua co thi them vao
                        {
                            DataProvider.Ins.Entities.DSKhachSans.Add(new DSKhachSan
                            {
                                MaKS = item.Hotel.MaKS,
                                MaDoan = dl.MaDoan,
                                SoNgay = item.noDay,
                                SoDem = item.noNight,
                            });
                            //ddlich.TongGiaKS += Convert.ToInt32(item.ChiPhi * ddlich.SoLuong) * Math.Abs(getday(Start.Value.Date, End.Value.Date));
                        }
                        else if (ks != null && ks.SoNgay != item.noDay)
                        {
                            
                        }
                    }

                    List<DSNhanVien> lstNhanVien = new List<DSNhanVien>(DataProvider.Ins.Entities
                       .DSNhanViens.Where(x => x.MaDoan == dl.MaDoan));
                    foreach (DSNhanVien item in lstNhanVien)
                    {
                        var dd = ListStaffs.Where(x => x.MaNV == item.MaNV && x.NhiemVu == item.NhiemVu).FirstOrDefault(); //item co con nam trong danh sach dia diem duoc chon hay khong
                        if (dd == null) //item khong con nam trong danh sach dia diem cua tour nua
                            DataProvider.Ins.Entities.DSNhanViens.Remove(item);
                    }
                    foreach (DSNhanVien item in ListStaffs)
                    {
                        var dd = lstNhanVien.Where(x => x.MaNV == item.MaNV && x.NhiemVu == item.NhiemVu).FirstOrDefault(); //Xem dia diem duoc chon da nam trong danh sach dia diem ban dau hay khong
                        if (dd == null) //Neu chua co thi them vao
                            DataProvider.Ins.Entities.DSNhanViens.Add(new DSNhanVien
                            {
                                MaNV = item.MaNV,
                                NhiemVu = item.NhiemVu,
                                MaDoan = dl.MaDoan,
                            });
                    }

                    DataProvider.Ins.Entities.SaveChanges();
                }
            });

            AddStaffCommand = new RelayCommand<Window>((p) =>
            {
                return isAddStaffEnable();
            }, (p) =>
            {
                //Thêm vào danh sách nhân viên tham gia đoàn
                if (ListStaffs != null)
                {
                    for (int i = 0; i < ListStaffs.Count; i++)
                    {
                        if (ListStaffs[i].NhiemVu == SelectedDuty)
                        {
                            var nv = SelectedStaffs.Where(x => (x.MaNV == ListStaffs[i].MaNV)).FirstOrDefault();
                            if (nv == null) ListStaffs.Remove(ListStaffs[i]);
                        }
                    }
                }

                foreach (NhanVien item in SelectedStaffs)
                {
                    DSNhanVien nv = new DSNhanVien();
                    nv = ListStaffs.Where(x => (x.MaNV == item.MaNV && x.NhiemVu == SelectedDuty)).FirstOrDefault();
                    if (nv == null)
                    {
                        DSNhanVien dsnv = new DSNhanVien()
                        {
                            MaNV = item.MaNV,
                            NhiemVu = SelectedDuty,
                            NhanVien = item,
                        };

                        ListStaffs.Add(dsnv);
                    }
                }
            });

            DeleteStaffCommand = new RelayCommand<Window>((p) =>
            {
                return Selectedstaff != null;
            }, (p) =>
            {
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                ListStaffs.Remove(Selectedstaff);
            });


            ManageCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                TouristGroupManagment tm = new TouristGroupManagment();
                tm.ShowDialog();
            });
        }
        public void SetTourist(DoanDuLich dl)
        {
            this.dl = dl;

            TouristName = this.dl.TenDoan;
            DateStart = this.dl.NgayKhoiHanh;
            DateEnd = this.dl.NgayKetThuc;
            Amount = this.dl.SoLuongToiDa.ToString();
            Detail = this.dl.ChiTiet;
            SelectedTour = this.dl.Tour;
            ButtonAdd = "Sửa";
            OtherCost = this.dl.ChiPhiKhac.ToString();

            ListStaffs = new ObservableCollection<DSNhanVien>(DataProvider.Ins.Entities.DSNhanViens.Where(p => (p.MaDoan == dl.MaDoan)));
            ListVehicles = new ObservableCollection<DSPhuongTien>(DataProvider.Ins.Entities.DSPhuongTiens.Where(p => (p.MaDoan == dl.MaDoan)));
            ListHotels = new ObservableCollection<DSKhachSan>(DataProvider.Ins.Entities.DSKhachSans.Where(p => (p.MaDoan == dl.MaDoan)));

            foreach (var item in ListHotels)
            {
                SelectedHotels.Add(new Hotels
                {
                    Hotel = item.KhachSan,
                    noDay = Convert.ToInt32(item.SoNgay),
                    noNight = Convert.ToInt32(item.SoDem),
                });
            }

            foreach (Hotels item in SelectedHotels)
            {
                Hotels check = lstHotel.Where(x => x.Hotel.MaKS == item.Hotel.MaKS).FirstOrDefault();
                if (check != null)
                {
                    check.noDay = item.noDay;
                    check.noNight = item.noNight;
                }
            }

            foreach (var item in ListVehicles)
            {                
                SelectedVehicles.Add(item.PhuongTien);
            }
        }

        #region Trigger

        public System.Collections.IList SelectedItems
        {
            get
            {
                return SelectedVehicles;
            }
            set
            {
                SelectedVehicles.Clear();

                foreach (PhuongTien pt in value)
                {
                    SelectedVehicles.Add(pt);
                }
            }
        }

        public System.Collections.IList SelectedHotel
        {
            get
            {
                return SelectedHotels;
            }
            set
            {
                SelectedHotels.Clear();

                foreach (Hotels ks in value)
                {
                    SelectedHotels.Add(ks);
                }
            }
        }

        public System.Collections.IList SelectedStaff
        {
            get
            {
                return SelectedStaffs;
            }
            set
            {
                SelectedStaffs.Clear();
                foreach (NhanVien nv in value)
                {
                    SelectedStaffs.Add(nv);
                }
            }
        }
        #endregion

        private bool isCommandEnable()
        {
            if (string.IsNullOrEmpty(TouristName) || string.IsNullOrEmpty(Amount)
                 || SelectedVehicles == null
                || SelectedHotels == null || ListStaffs == null)
            {
                return false;
            }

            return true;
        }

        private bool isAddStaffEnable()
        {
            if (SelectedStaffs.Count == 0 || SelectedDuty == null)
            {
                return false;
            }

            return true;
        }

        #region Input

        private Regex _regex = new Regex("[^0-9.-]+"); //Dinh dang chi cho phep go chu so

        private Regex _Aregex = new Regex("[^a-zA-Z]"); //Dinh dang chi cho phep nhap chu
        private bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private bool AlphabetCheck(string text)
        {
            return !_Aregex.IsMatch(text);

        }

        public void NumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        public void AlphabetInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AlphabetCheck(e.Text);
        }

        public void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; //Neu la phim cach thi xem nhu da xu ly (bo qua phim do)
            }
        }
        #endregion


    }



}



