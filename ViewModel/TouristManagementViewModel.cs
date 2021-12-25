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
        public ICommand AddCusCommand { get; set; }
        public ICommand DeleteCusCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private ObservableCollection<DoanDuLich> _lstTouristGr;
        public ObservableCollection<DoanDuLich> lstTouristGr { get { return _lstTouristGr; } set { _lstTouristGr = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachHang> _lstCustomer; //danh sách khách hàng đổ vào combo box
        public ObservableCollection<KhachHang> lstCustomer { get { return _lstCustomer; } set { _lstCustomer = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachDuLich> _lstTourist; //danh sách khách du lịch thuộc đoàn
        public ObservableCollection<KhachDuLich> lstTourist { get { return _lstTourist; } set { _lstTourist = value; OnPropertyChanged(); } }

        private ObservableCollection<DSPhuongTien> _lstVehicle;
        public ObservableCollection<DSPhuongTien> lstVehicle { get { return _lstVehicle; } set { _lstVehicle = value; OnPropertyChanged(); } }

        private ObservableCollection<DSKhachSan> _lstHotel;
        public ObservableCollection<DSKhachSan> lstHotel { get { return _lstHotel; } set { _lstHotel = value; OnPropertyChanged(); } }

        private ObservableCollection<DSNhanVien> _lstStaff;
        public ObservableCollection<DSNhanVien> lstStaff { get { return _lstStaff; } set { _lstStaff = value; OnPropertyChanged(); } }

        private ObservableCollection<DSDiaDiem> _lstDestination;
        public ObservableCollection<DSDiaDiem> lstDestination { get { return _lstDestination; } set { _lstDestination = value; OnPropertyChanged(); } }

        private KhachDuLich _SelectedCustomer;
        public KhachDuLich SelectedCustomer { get { return _SelectedCustomer; } set { _SelectedCustomer = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private string _MaxAmount;
        public string MaxAmount { get { return _MaxAmount; } set { _MaxAmount = value; OnPropertyChanged(); } }

        private string _Amount;
        public string Amount { get { return _Amount; } set { _Amount = value; OnPropertyChanged(); } }

        private DateTime? _Start;
        public DateTime? Start
        {
            get { return _Start; }
            set
            {
                _Start = value;
                OnPropertyChanged();
                if (Start != null)
                {
                    End = null;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTouristGr);
                    view.Filter = StartFilter;
                }
            }
        }

        private DateTime? _End;
        public DateTime? End
        {
            get { return _End; }
            set
            {
                _End = value;
                OnPropertyChanged();
                if (End != null)
                {
                    Start = null;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTouristGr);
                    view.Filter = EndFilter;
                }

            }
        }
        private DateTime? _Start2;
        public DateTime? Start2 { get { return _Start2; } set { _Start2 = value; OnPropertyChanged(); } }

        private DateTime? _End2;
        public DateTime? End2 { get { return _End2; } set { _End2 = value; OnPropertyChanged(); } }
        private KhachHang _Customer;
        public KhachHang Customer
        {
            get { return _Customer; }
            set
            {
                _Customer = value;
                OnPropertyChanged();
            }
        }

        private DoanDuLich _SelectedTouristGr;
        public DoanDuLich SelectedTouristGr
        {
            get { return _SelectedTouristGr; }
            set
            {
                _SelectedTouristGr = value;
                OnPropertyChanged();
                if (SelectedTouristGr != null)
                {
                    lstHotel = new ObservableCollection<DSKhachSan>(DataProvider.Ins.Entities.DSKhachSans.Where(p => (p.MaDoan == SelectedTouristGr.MaDoan)));
                    lstVehicle = new ObservableCollection<DSPhuongTien>(DataProvider.Ins.Entities.DSPhuongTiens.Where(p => (p.MaDoan == SelectedTouristGr.MaDoan)));
                    lstStaff = new ObservableCollection<DSNhanVien>(DataProvider.Ins.Entities.DSNhanViens.Where(p => (p.MaDoan == SelectedTouristGr.MaDoan)));
                    lstTourist = new ObservableCollection<KhachDuLich>(DataProvider.Ins.Entities.KhachDuLiches.Where(p => (p.MaDoan == SelectedTouristGr.MaDoan)));
                    lstDestination = new ObservableCollection<DSDiaDiem>(DataProvider.Ins.Entities.DSDiaDiems.Where(p => (p.MaTour == SelectedTouristGr.Tour.MaTour)));
                    Amount = (SelectedTouristGr.SoLuong).ToString() + "/";
                    MaxAmount = SelectedTouristGr.SoLuongToiDa.ToString();
<<<<<<< HEAD
                    Start2 = SelectedTouristGr.NgayKhoiHanh;
                    End2 = SelectedTouristGr.NgayKetThuc;
=======
                    //MessageBox.Show(SelectedTouristGr.SoLuongGiamGia + "");
>>>>>>> c54b68c8eb936153a2af3065208ff847feb0b518
                }

            }
        }
        public TouristManagementViewModel()
        {
            lstTouristGr = new ObservableCollection<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches);
            lstCustomer = new ObservableCollection<KhachHang>(DataProvider.Ins.Entities.KhachHangs);
            
            AddCusCommand = new RelayCommand<Window>((p) =>
            {
                return Customer != null; //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) =>
            {
                if (SelectedTouristGr.SoLuong == SelectedTouristGr.SoLuongToiDa)
                {
                    MessageBox.Show("Số lượng hành khách đã đạt tối đa");
                    return;
                }

                KhachDuLich kdl = new KhachDuLich()
                {
                    MaKH = Customer.MaKH,
                    MaDoan = SelectedTouristGr.MaDoan,
                };

                foreach (KhachDuLich item in lstTourist)
                {
                    if (item.MaKH == Customer.MaKH)
                    {
                        MessageBox.Show("Khách này đã nằm trong danh sách đoàn", "Thông báo", MessageBoxButton.OK);
                        return;
                    }
                }

                //cap nhat so lan di tour cua khach hang
                KhachHang kh = DataProvider.Ins.Entities.KhachHangs.Where(x => x.MaKH == Customer.MaKH).FirstOrDefault(); ;
                kh.SoLanDiTour += 1;

                if (kh.SoLanDiTour == 5)
                {
                    KhachHangThanThiet thanThiet = new KhachHangThanThiet
                    {
                        MaKH = Customer.MaKH
                    };
                    DataProvider.Ins.Entities.KhachHangThanThiets.Add(thanThiet);
                }

                //Them giảm giá nếu số lần đi tour chia hết cho 5
                if (kh.SoLanDiTour % 5 == 0)
                {
                    DoanDuLich dl = DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaDoan == SelectedTouristGr.MaDoan).FirstOrDefault();
                    dl.SoLuongGiamGia += 1;
                }
                DataProvider.Ins.Entities.SaveChanges();
                List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                    .DSKhachSans.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                decimal GiaKs = 0;
                foreach (DSKhachSan hotel in lstHotel)
                {
                    GiaKs += Convert.ToInt32(hotel.KhachSan.ChiPhi);
                   
                }

                DoanDuLich tour = DataProvider.Ins.Entities.DoanDuLiches.Where(w => w.MaDoan == SelectedTouristGr.MaDoan).FirstOrDefault();

                tour.TongGiaKS += Convert.ToDecimal(GiaKs) * Math.Abs(getday(Start2.Value.Date, End2.Value.Date));

                //SelectedTouristGr.TongGiaKS = GiaKs;

                DataProvider.Ins.Entities.SaveChanges();
                
                SelectedTouristGr.SoLuong +=1;

                Amount = (SelectedTouristGr.SoLuong).ToString() + "/";

                //MessageBox.Show(SelectedTouristGr.TongGiaPT.ToString());

                DataProvider.Ins.Entities.KhachDuLiches.Add(kdl);
                DataProvider.Ins.Entities.SaveChanges();

                lstTourist.Add(kdl);
                int index = lstTouristGr.IndexOf(SelectedTouristGr);

                lstTouristGr[index] = new DoanDuLich()
                {
                    ChiPhiKhac = SelectedTouristGr.ChiPhiKhac,
                    MaDoan = SelectedTouristGr.MaDoan,
                    ChiTiet = SelectedTouristGr.ChiTiet,
                    SoLuong = SelectedTouristGr.SoLuong,
                    MaTour = SelectedTouristGr.MaTour,
                    TenDoan = SelectedTouristGr.TenDoan,
                    DSKhachSans = SelectedTouristGr.DSKhachSans,
                    DSNhanViens = SelectedTouristGr.DSNhanViens,
                    DSPhuongTiens = SelectedTouristGr.DSPhuongTiens,
                    KhachDuLiches = SelectedTouristGr.KhachDuLiches,
                    NgayKetThuc = SelectedTouristGr.NgayKetThuc,
                    NgayKhoiHanh = SelectedTouristGr.NgayKhoiHanh,
                    SoLuongToiDa = SelectedTouristGr.SoLuongToiDa,
                    TongGiaKS =  tour.TongGiaKS,
                    TongGiaAU = SelectedTouristGr.TongGiaAU,
                    TongGiaPT = SelectedTouristGr.TongGiaPT,
                    Tour = SelectedTouristGr.Tour
                };
                SelectedTouristGr = lstTouristGr[index];
            });
            
            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedTouristGr != null;
            }, (p) =>
            {

                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                    .DSKhachSans.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSKhachSan hotel in lstHotel)
                {
                    DataProvider.Ins.Entities.DSKhachSans.Remove(hotel);
                }

                List<DSPhuongTien> lstPhuongTien = new List<DSPhuongTien>(DataProvider.Ins.Entities
                    .DSPhuongTiens.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSPhuongTien phuongTien in lstPhuongTien)
                {
                    DataProvider.Ins.Entities.DSPhuongTiens.Remove(phuongTien);
                }

                List<DSNhanVien> lstNhanVien = new List<DSNhanVien>(DataProvider.Ins.Entities
                    .DSNhanViens.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSNhanVien nhanvien in lstNhanVien)
                {
                    DataProvider.Ins.Entities.DSNhanViens.Remove(nhanvien);
                }


                List<KhachDuLich> lstKhachHang = new List<KhachDuLich>(DataProvider.Ins.Entities
                    .KhachDuLiches.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (KhachDuLich khachHang in lstKhachHang)
                {
                    DataProvider.Ins.Entities.KhachDuLiches.Remove(khachHang);
                }

                DataProvider.Ins.Entities.DoanDuLiches.Remove(SelectedTouristGr);
                DataProvider.Ins.Entities.SaveChanges();

                lstTouristGr.Remove(SelectedTouristGr);
                lstPhuongTien.Clear();
                lstHotel.Clear();
                lstStaff.Clear();
                lstTourist.Clear();
                lstDestination.Clear();
                SelectedTouristGr = null;
            });

            DeleteCusCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedCustomer != null;
            }, (p) =>
            {
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;
                

                //SelectedTouristGr.TongGiaKS = GiaKs;

                DataProvider.Ins.Entities.SaveChanges();
                List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                    .DSKhachSans.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                decimal GiaKs = 0;
                foreach (DSKhachSan hotel in lstHotel)
                {
                    GiaKs += Convert.ToInt32(hotel.KhachSan.ChiPhi);

                }

                DoanDuLich tour = DataProvider.Ins.Entities.DoanDuLiches.Where(w => w.MaDoan == SelectedTouristGr.MaDoan).FirstOrDefault();

                tour.TongGiaKS -= Convert.ToDecimal(GiaKs) * Math.Abs(getday(Start2.Value.Date, End2.Value.Date));
                
                SelectedTouristGr.SoLuong -= 1;

                Amount = (SelectedTouristGr.SoLuong).ToString() + "/";

                //cap nhat so lan di tour cua khach hang
                KhachHang kh = DataProvider.Ins.Entities.KhachHangs.Where(x => x.MaKH == SelectedCustomer.MaKH).FirstOrDefault(); ;
                if (kh.SoLanDiTour % 5 == 0)
                {
                    DoanDuLich dl = DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaDoan == SelectedTouristGr.MaDoan).FirstOrDefault();
                    dl.SoLuongGiamGia -= 1;
                }    
                
                kh.SoLanDiTour -= 1;
                if (kh.SoLanDiTour == 4)
                {
                    KhachHangThanThiet thanThiet = DataProvider.Ins.Entities.KhachHangThanThiets.Where(x => x.MaKH == SelectedCustomer.MaKH).FirstOrDefault();
                    DataProvider.Ins.Entities.KhachHangThanThiets.Remove(thanThiet);
                }

                DataProvider.Ins.Entities.KhachDuLiches.Remove(SelectedCustomer);
                DataProvider.Ins.Entities.SaveChanges();
                lstTourist.Remove(SelectedCustomer);
                int index = lstTouristGr.IndexOf(SelectedTouristGr);

                lstTouristGr[index] = new DoanDuLich()
                {
                    ChiPhiKhac = SelectedTouristGr.ChiPhiKhac,
                    MaDoan = SelectedTouristGr.MaDoan,
                    ChiTiet = SelectedTouristGr.ChiTiet,
                    SoLuong = SelectedTouristGr.SoLuong,
                    MaTour = SelectedTouristGr.MaTour,
                    TenDoan = SelectedTouristGr.TenDoan,
                    DSKhachSans = SelectedTouristGr.DSKhachSans,
                    DSNhanViens = SelectedTouristGr.DSNhanViens,
                    DSPhuongTiens = SelectedTouristGr.DSPhuongTiens,
                    KhachDuLiches = SelectedTouristGr.KhachDuLiches,
                    NgayKetThuc = SelectedTouristGr.NgayKetThuc,
                    NgayKhoiHanh = SelectedTouristGr.NgayKhoiHanh,
                    SoLuongToiDa = SelectedTouristGr.SoLuongToiDa,
                    TongGiaKS = tour.TongGiaKS,
                    TongGiaAU = SelectedTouristGr.TongGiaAU,
                    TongGiaPT = SelectedTouristGr.TongGiaPT,
                    Tour = SelectedTouristGr.Tour
                };
                SelectedTouristGr = lstTouristGr[index];
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                Start = null;
                End = null;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTouristGr);
                view.Filter = TouristGrFilter;
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedTouristGr != null; //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) =>
            {
                TouristGroup tourist = new TouristGroup();

                TouristViewModel viewModel = tourist.DataContext as TouristViewModel;
                viewModel.SetTourist(SelectedTouristGr);
                tourist.ShowDialog();

                //Them dong nay cho no reload lai
                lstTouristGr = new ObservableCollection<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches);
                lstHotel = new ObservableCollection<DSKhachSan>(DataProvider.Ins.Entities.DSKhachSans.Where(x => (x.MaDoan == SelectedTouristGr.MaDoan)));
                lstVehicle = new ObservableCollection<DSPhuongTien>(DataProvider.Ins.Entities.DSPhuongTiens.Where(x => (x.MaDoan == SelectedTouristGr.MaDoan)));
                lstStaff = new ObservableCollection<DSNhanVien>(DataProvider.Ins.Entities.DSNhanViens.Where(x => (x.MaDoan == SelectedTouristGr.MaDoan)));
                lstTourist = new ObservableCollection<KhachDuLich>(DataProvider.Ins.Entities.KhachDuLiches.Where(x => (x.MaDoan == SelectedTouristGr.MaDoan)));
                lstDestination = new ObservableCollection<DSDiaDiem>(DataProvider.Ins.Entities.DSDiaDiems.Where(x => (x.MaTour == SelectedTouristGr.Tour.MaTour)));
                Amount = (SelectedTouristGr.SoLuong).ToString() + "/";
                MaxAmount = SelectedTouristGr.SoLuongToiDa.ToString();
            });


        }
        private decimal getday(DateTime? StartDay, DateTime? EndDate)
        {
            TimeSpan interval = StartDay.Value.Date - EndDate.Value.Date;
            int day = (int)(interval.TotalMilliseconds / 86400000);
            return day;
        }
        private bool TouristGrFilter(object item)
        {
            DoanDuLich dl = item as DoanDuLich;
            if (filterName(dl))
                return true;

            return false;
        }

        private bool StartFilter(object item)
        {
            DoanDuLich dl = item as DoanDuLich;
            if (filterStart(dl))
                return true;

            return false;
        }

        private bool EndFilter(object item)
        {
            DoanDuLich dl = item as DoanDuLich;
            if (filterEnd(dl))
                return true;

            return false;
        }

        #region Filter

        private bool filterName(DoanDuLich dl)
        {
            if (string.IsNullOrEmpty(Name) || dl.TenDoan.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterEnd(DoanDuLich dl)
        {
            if (End == null || dl.NgayKetThuc == End)
            {
                return true;
            }
            return false;
        }

        private bool filterStart(DoanDuLich dl)
        {
            if (Start == null || dl.NgayKhoiHanh == Start)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}