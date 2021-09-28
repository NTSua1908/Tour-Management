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
    class TouristViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ManageCommand { get; set; }
        public ICommand AddStaffCommand { get; set; }

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

        private ObservableCollection<DiaDiem> _SelectedDestinations; 
        public ObservableCollection<DiaDiem> SelectedDestinations { get { return _SelectedDestinations; } set { _SelectedDestinations = value; OnPropertyChanged(); } }

        private ObservableCollection<DSNhanVien> _ListStaffs; //Thêm vào danh sách nhân viên
        public ObservableCollection<DSNhanVien> ListStaffs { get { return _ListStaffs; } set { _ListStaffs = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private Tour _SelectedTour;
        public Tour SelectedTour { get { return _SelectedTour; } set { _SelectedTour = value; OnPropertyChanged(); } }       

        private DateTime? _Start;
        public DateTime? Start { get { return _Start; } set { _Start = value; OnPropertyChanged(); } }

        private DateTime? _End;
        public DateTime? End { get { return _End; } set { _End = value; OnPropertyChanged(); } }

        private string _Amount;
        public string Amount { get { return _Amount; } set { _Amount = value; OnPropertyChanged(); } }

        private string _Detail;
        public string Detail { get { return _Detail; } set { _Detail = value; OnPropertyChanged(); } }

        private String _SelectedDuty;
        public String SelectedDuty { get { return _SelectedDuty; } set { _SelectedDuty = value; OnPropertyChanged(); } }
        public TouristViewModel()
        {          
            lstTour = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);

            lstVehicle = new ObservableCollection<PhuongTien>(DataProvider.Ins.Entities.PhuongTiens);

            lstHotel = new ObservableCollection<KhachSan>(DataProvider.Ins.Entities.KhachSans);

            lstStaff = new ObservableCollection<NhanVien>(DataProvider.Ins.Entities.NhanViens);

            lstDestination = new ObservableCollection<DiaDiem>(DataProvider.Ins.Entities.DiaDiems);

            SelectedVehicles = new ObservableCollection<PhuongTien>();

            SelectedHotels = new ObservableCollection<KhachSan>();

            SelectedStaffs = new ObservableCollection<NhanVien>();

            SelectedDestinations = new ObservableCollection<DiaDiem>();

            ListStaffs = new ObservableCollection<DSNhanVien>();

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
                DoanDuLich dl = new DoanDuLich()
                {
                    TenDoan = Name,
                    MaTour = SelectedTour.MaTour,
                    NgayKhoiHanh = Start,
                    NgayKetThuc = End,
                    SoLuong = Convert.ToInt32(Amount),
                    ChiTiet = Detail,
                };
                
                DataProvider.Ins.Entities.DoanDuLiches.Add(dl);
                DataProvider.Ins.Entities.SaveChanges();

                for (int i = 0; i < SelectedVehicles.Count; i++)
                {
                    DSPhuongTien dspt = new DSPhuongTien()
                    {
                        MaDoan = dl.MaDoan,
                        MaPT = SelectedVehicles[i].MaPT,
                    };

                    DataProvider.Ins.Entities.DSPhuongTiens.Add(dspt);                    
                }

                for (int i = 0; i < SelectedHotels.Count; i++)
                {
                    DSKhachSan dsks = new DSKhachSan()
                    {
                        MaDoan = dl.MaDoan,
                        MaKS = SelectedHotels[i].MaKS,
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

                DataProvider.Ins.Entities.SaveChanges();
            });

            AddStaffCommand = new RelayCommand<Window>((p) =>
            {
                return isAddStaffEnable();
            }, (p) =>
            {
                //Thêm vào danh sách nhân viên tham gia đoàn
                for (int i = 0; i < SelectedStaffs.Count; i++)
                {
                    DSNhanVien dsnv = new DSNhanVien()
                    {
                        MaNV = SelectedStaffs[i].MaNV,
                        NhiemVu = SelectedDuty,
                    };
                    ListStaffs.Add(dsnv);
                }
            });

            ManageCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                TouristGroupManagment tm = new TouristGroupManagment();
                tm.ShowDialog();
            });
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

        public System.Collections.IList SelectedDestination
        {
            get
            {
                return SelectedDestinations;
            }
            set
            {
                SelectedDestinations.Clear();
                foreach (DiaDiem dd in value)
                {
                    SelectedDestinations.Add(dd);
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
                || Start == null || End == null || SelectedVehicles == null 
                || SelectedHotels ==null || ListStaffs == null)
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
