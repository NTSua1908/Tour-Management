﻿using System;
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
        public ICommand SearchTouristCommand { get; set; }

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

        private string _TouristName;
        public string TouristName { get { return _TouristName; } set { _TouristName = value; OnPropertyChanged(); } }

        private string _MaxAmount;
        public string MaxAmount { get { return _MaxAmount; } set { _MaxAmount = value; OnPropertyChanged(); } }

        private string _Amount;
        public string Amount { get { return _Amount; } set { _Amount = value; OnPropertyChanged(); } }

        private string _ChiPhiAU;
        public string ChiPhiAU { get { return _ChiPhiAU; } set { _ChiPhiAU = value; OnPropertyChanged(); } }

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
                }

            }
        }
        public TouristManagementViewModel()
        {
            lstTouristGr = new ObservableCollection<DoanDuLich>(DataProvider.Ins.Entities.DoanDuLiches);
            lstCustomer = new ObservableCollection<KhachHang>(DataProvider.Ins.Entities.KhachHangs);

            AddCusCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedTouristGr == null)
                {
                    return false;
                }
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

                List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                    .DSKhachSans.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSKhachSan hotel in lstHotel)
                {
                    SelectedTouristGr.TongGiaKS += Convert.ToInt32(hotel.KhachSan.ChiPhi);
                }

                List<DSPhuongTien> lstPhuongTien = new List<DSPhuongTien>(DataProvider.Ins.Entities
                    .DSPhuongTiens.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSPhuongTien phuongTien in lstPhuongTien)
                {
                    SelectedTouristGr.TongGiaPT += Convert.ToInt32(phuongTien.PhuongTien.ChiPhi);
                }

                SelectedTouristGr.SoLuong +=1;
                Amount = (SelectedTouristGr.SoLuong).ToString() + "/";
                SelectedTouristGr.TongGiaAU *= SelectedTouristGr.SoLuong;

                DataProvider.Ins.Entities.KhachDuLiches.Add(kdl);
                DataProvider.Ins.Entities.SaveChanges();

                MessageBox.Show(SelectedTouristGr.TongGiaAU.ToString());

                lstTourist.Add(kdl);
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

                List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                    .DSKhachSans.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSKhachSan hotel in lstHotel)
                {
                    SelectedTouristGr.TongGiaKS -= Convert.ToInt32(hotel.KhachSan.ChiPhi);
                }

                List<DSPhuongTien> lstPhuongTien = new List<DSPhuongTien>(DataProvider.Ins.Entities
                    .DSPhuongTiens.Where(x => x.MaDoan == SelectedTouristGr.MaDoan));
                foreach (DSPhuongTien phuongTien in lstPhuongTien)
                {
                    SelectedTouristGr.TongGiaPT -= Convert.ToInt32(phuongTien.PhuongTien.ChiPhi);
                }

                SelectedTouristGr.SoLuong -= 1;
                Amount = (SelectedTouristGr.SoLuong).ToString() + "/";
                SelectedTouristGr.TongGiaAU *= SelectedTouristGr.SoLuong;

                DataProvider.Ins.Entities.KhachDuLiches.Remove(SelectedCustomer);
                DataProvider.Ins.Entities.SaveChanges();

                MessageBox.Show(SelectedTouristGr.TongGiaAU.ToString());
                lstTourist.Remove(SelectedCustomer);
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

            SearchTouristCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTourist);
                view.Filter = TouristFilter;
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedTouristGr != null; //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) =>
            {
                TouristGroup tourist = new TouristGroup();

                TouristViewModel viewModel = tourist.DataContext as TouristViewModel;
                viewModel.SetTourist(SelectedTouristGr);
                tourist.CreateSelectedVehicles();
                tourist.CreateSelectedHotels();
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

        private bool TouristGrFilter(object item)
        {
            DoanDuLich dl = item as DoanDuLich;
            if (filterGroupTourName(dl))
                return true;
            return false;
        }

        private bool TouristFilter(object item)
        {
            KhachDuLich kdl = item as KhachDuLich;
            if (filterTouristName(kdl))
                return true;
            return false;
        }

        private bool StartFilter(object item)
        {
            DoanDuLich dl = item as DoanDuLich;
            if (filterStartDate(dl))
                return true;

            return false;
        }

        private bool EndFilter(object item)
        {
            DoanDuLich dl = item as DoanDuLich;
            if (filterEndDate(dl))
                return true;

            return false;
        }

        #region Filter

        private bool filterGroupTourName(DoanDuLich dl)
        {
            if (string.IsNullOrEmpty(Name) || dl.TenDoan.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterTouristName(KhachDuLich kdl)
        {
            if (string.IsNullOrEmpty(TouristName) || kdl.KhachHang.Hoten.ToLower().Contains(TouristName.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterEndDate(DoanDuLich dl)
        {
            if (End == null || dl.NgayKetThuc == End)
            {
                return true;
            }
            return false;
        }

        private bool filterStartDate(DoanDuLich dl)
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