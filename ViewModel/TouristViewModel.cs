﻿using System;
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

        private ObservableCollection<DoanDuLich> _lstTourist;
        public ObservableCollection<DoanDuLich> lstTourist { get { return _lstTourist; } set { _lstTourist = value; OnPropertyChanged(); } }

        private ObservableCollection<NhanVien> _lstStaff; //danh sách từ database đổ vào listbox
        public ObservableCollection<NhanVien> lstStaff { get { return _lstStaff; } set { _lstStaff = value; OnPropertyChanged(); } }

        private ObservableCollection<PhuongTien> _lstVehicle;
        public ObservableCollection<PhuongTien> lstVehicle { get { return _lstVehicle; } set { _lstVehicle = value; OnPropertyChanged(); } }

        private ObservableCollection<DiaDiem> _lstDestination;
        public ObservableCollection<DiaDiem> lstDestination { get { return _lstDestination; } set { _lstDestination = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachSan> _lstHotel;
        public ObservableCollection<KhachSan> lstHotel { get { return _lstHotel; } set { _lstHotel = value; OnPropertyChanged(); } }

        private ObservableCollection<Tour> _lstTour;
        public ObservableCollection<Tour> lstTour { get { return _lstTour; } set { _lstTour = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _lstDuty;
        public ObservableCollection<string> lstDuty { get { return _lstDuty; } set { _lstDuty = value; OnPropertyChanged(); } }

        private ObservableCollection<PhuongTien> _SelectedVehicles;
        public ObservableCollection<PhuongTien> SelectedVehicles { get { return _SelectedVehicles; } set { _SelectedVehicles = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachSan> _SelectedHotels;
        public ObservableCollection<KhachSan> SelectedHotels { get { return _SelectedHotels; } set { _SelectedHotels = value; OnPropertyChanged(); } }

        private ObservableCollection<NhanVien> _SelectedStaffs; //Danh sách nhân viên được chọn trong listbox
        public ObservableCollection<NhanVien> SelectedStaffs { get { return _SelectedStaffs; } set { _SelectedStaffs = value; OnPropertyChanged(); } }

        private ObservableCollection<DSNhanVien> _ListStaffs; //Thêm vào danh sách nhân viên
        public ObservableCollection<DSNhanVien> ListStaffs { get { return _ListStaffs; } set { _ListStaffs = value; OnPropertyChanged(); } }

        private ObservableCollection<DSPhuongTien> _ListVehicles;
        public ObservableCollection<DSPhuongTien> ListVehicles { get { return _ListVehicles; } set { _ListVehicles = value; OnPropertyChanged(); } }

        private ObservableCollection<DSKhachSan> _ListHotels;
        public ObservableCollection<DSKhachSan> ListHotels { get { return _ListHotels; } set { _ListHotels = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private Tour _SelectedTour;
        public Tour SelectedTour { get { return _SelectedTour; } set { _SelectedTour = value; OnPropertyChanged(); } }

        private DSNhanVien _Selectedstaff;  //Thành phần trong list view nhân viên
        public DSNhanVien Selectedstaff { get { return _Selectedstaff; } set { _Selectedstaff = value; OnPropertyChanged(); } }

        private DateTime? _Start;
        public DateTime? Start { get { return _Start; } set { _Start = value; OnPropertyChanged(); } }

        private DateTime? _End;
        public DateTime? End { get { return _End; } set { _End = value; OnPropertyChanged(); } }

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

        private String _SelectedDuty;
        public String SelectedDuty { get { return _SelectedDuty; } set { _SelectedDuty = value; OnPropertyChanged(); } }

        private DoanDuLich dl;

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged();
            }
        }
        public TouristViewModel()
        {

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            lstTour = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);
            lstVehicle = new ObservableCollection<PhuongTien>(DataProvider.Ins.Entities.PhuongTiens);
            lstHotel = new ObservableCollection<KhachSan>(DataProvider.Ins.Entities.KhachSans);
            lstStaff = new ObservableCollection<NhanVien>(DataProvider.Ins.Entities.NhanViens);
            SelectedVehicles = new ObservableCollection<PhuongTien>();
            SelectedHotels = new ObservableCollection<KhachSan>();
            SelectedStaffs = new ObservableCollection<NhanVien>();
            ListStaffs = new ObservableCollection<DSNhanVien>();
            ListVehicles = new ObservableCollection<DSPhuongTien>();
            ButtonAdd = "Thêm";
            MinAvailDate = DateTime.Now;
                        

            lstDuty = new ObservableCollection<string>();
            lstDuty.Add("Trưởng đoàn");
            lstDuty.Add("Phó đoàn");
            lstDuty.Add("Tài xế");
            lstDuty.Add("Phục vụ");
            lstDuty.Add("Hướng dẫn viên");           

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) =>
            {
                if (ButtonAdd == "Thêm")
                {
                    MessageBox.Show(Start.Value.TimeOfDay.ToString());
                    DoanDuLich dl = new DoanDuLich()
                    {
                        TenDoan = Name,
                        MaTour = SelectedTour.MaTour,
                        NgayKhoiHanh = Start,
                        NgayKetThuc = End,
                        SoLuongToiDa = Convert.ToInt32(Amount),
                        SoLuong = 0,
                        ChiTiet = Detail,
                        TongGiaPT = 0,
                        TongGiaKS = 0,
                        TongGiaAU = Convert.ToInt32(CostAU),
                        ChiPhiKhac = 0,
                    };

                    if (!String.IsNullOrEmpty(OtherCost))
                    {
                        dl.ChiPhiKhac = Convert.ToInt32(OtherCost);
                    }

                    DataProvider.Ins.Entities.DoanDuLiches.Add(dl);
                    DataProvider.Ins.Entities.SaveChanges();

                    for (int i = 0; i < SelectedVehicles.Count; i++)
                    {
                        DSPhuongTien dspt = new DSPhuongTien()
                        {
                            MaDoan = dl.MaDoan,
                            MaPT = SelectedVehicles[i].MaPT,
                        };

                        //dl.TongGiaPT += SelectedVehicles[i].ChiPhi;
                        
                        DataProvider.Ins.Entities.DSPhuongTiens.Add(dspt);
                    }

                    for (int i = 0; i < SelectedHotels.Count; i++)
                    {
                        DSKhachSan dsks = new DSKhachSan()
                        {
                            MaDoan = dl.MaDoan,
                            MaKS = SelectedHotels[i].MaKS,
                        };

                        //dl.TongGiaKS += SelectedHotels[i].ChiPhi;

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

                    DataProvider.Ins.Entities.SaveChanges();
                    MessageBox.Show(dl.TongGiaAU.ToString());
                }
                else if (ButtonAdd == "Sửa")
                {

                    DoanDuLich ddlich = DataProvider.Ins.Entities.DoanDuLiches.Where(x => x.MaDoan == dl.MaDoan).FirstOrDefault();

                    ddlich.TenDoan = Name;
                    ddlich.MaTour = SelectedTour.MaTour;
                    ddlich.NgayKhoiHanh = Start;
                    ddlich.NgayKetThuc = End;
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
                            ddlich.TongGiaPT -= (item.PhuongTien.ChiPhi * ddlich.SoLuong); 
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

                            ddlich.TongGiaPT += Convert.ToInt32(item.ChiPhi * ddlich.SoLuong);
                        }
                    }

                    List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                        .DSKhachSans.Where(x => x.MaDoan == dl.MaDoan));
                    foreach (DSKhachSan hotel in lstHotel)
                    {
                        var ks = SelectedHotels.Where(x => x.MaKS == hotel.MaKS).FirstOrDefault();
                        if (ks == null)
                        {
                            ddlich.TongGiaKS -= (hotel.KhachSan.ChiPhi * ddlich.SoLuong);
                            DataProvider.Ins.Entities.DSKhachSans.Remove(hotel);
                        }
                    }
                    foreach (KhachSan item in SelectedHotels)
                    {
                        var ks = lstHotel.Where(x => x.MaKS == item.MaKS).FirstOrDefault(); //Xem dia diem duoc chon da nam trong danh sach dia diem ban dau hay khong
                        if (ks == null) //Neu chua co thi them vao
                        {
                            DataProvider.Ins.Entities.DSKhachSans.Add(new DSKhachSan
                            {
                                MaKS = item.MaKS,
                                MaDoan = dl.MaDoan,
                            });

                            ddlich.TongGiaKS += Convert.ToInt32(item.ChiPhi * ddlich.SoLuong);
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
                    MessageBox.Show(ddlich.TongGiaAU.ToString());
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
                return Selectedstaff !=null;
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

            Name = this.dl.TenDoan;
            Start = this.dl.NgayKhoiHanh;
            End = this.dl.NgayKetThuc;
            Amount = this.dl.SoLuongToiDa.ToString();
            Detail = this.dl.ChiTiet;
            SelectedTour = this.dl.Tour;
            ButtonAdd = "Sửa";            
            OtherCost = this.dl.ChiPhiKhac.ToString();

            ListStaffs = new ObservableCollection<DSNhanVien>(DataProvider.Ins.Entities.DSNhanViens.Where(p => (p.MaDoan == dl.MaDoan)));
            ListVehicles = new ObservableCollection<DSPhuongTien>(DataProvider.Ins.Entities.DSPhuongTiens.Where(p => (p.MaDoan == dl.MaDoan)));
            ListHotels = new ObservableCollection<DSKhachSan>(DataProvider.Ins.Entities.DSKhachSans.Where(p => (p.MaDoan == dl.MaDoan)));

            foreach (var item in ListVehicles)
            {
                SelectedVehicles.Add(item.PhuongTien);
            }

            foreach (var item in ListHotels)
            {
                SelectedHotels.Add(item.KhachSan);
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
                foreach (KhachSan ks in value)
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
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Amount)
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