using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class CustomerViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private ObservableCollection<KhachHang> _lstCustomer;
        public ObservableCollection<KhachHang> lstCustomer { get { return _lstCustomer; } set { _lstCustomer = value; OnPropertyChanged(); } }

        private ObservableCollection<LoaiKhach> _lstCustomerType;
        public ObservableCollection<LoaiKhach> lstCustomerType { get { return _lstCustomerType; } set { _lstCustomerType = value; OnPropertyChanged(); } }
        
        private ObservableCollection<string> _lstGender;
        public ObservableCollection<string> lstGender { get { return _lstGender; } set { _lstGender = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private string _Age;
        public string Age { get { return _Age; } set { _Age = value; OnPropertyChanged(); } }

        private string _SelectedGender;
        public string SelectedGender { get { return _SelectedGender; } set { _SelectedGender = value; OnPropertyChanged(); } }

        private LoaiKhach _SelectedCustomerType;
        public LoaiKhach SelectedCustomerType { get { return _SelectedCustomerType; } set { _SelectedCustomerType = value; OnPropertyChanged(); } }

        private string _CMND;
        public string CMND { get { return _CMND; } set { _CMND = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get { return _Address; } set { _Address = value; OnPropertyChanged(); } }

        private DateTime? _Visa;
        public DateTime? Visa { get { return _Visa; } set { _Visa = value; OnPropertyChanged(); } }

        private DateTime? _Passport;
        public DateTime? Passport { get { return _Passport; } set { _Passport = value; OnPropertyChanged(); } }

        public KhachHang _SelectedItem;
        public KhachHang SelectedItem { get { return _SelectedItem; } 
            set { 
                _SelectedItem = value; 
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Name = SelectedItem.Hoten;
                    Age =  SelectedItem.Tuoi.ToString();
                    CMND = SelectedItem.CMND_Passport.ToString();
                    Phone = SelectedItem.SDT.ToString();
                    Address = SelectedItem.DiaChi;
                    Visa = SelectedItem.HanVisa;
                    Passport = SelectedItem.HanPassort;
                    SelectedGender = SelectedItem.GioiTinh;
                    SelectedCustomerType = SelectedItem.LoaiKhach;
                }
        } }

        public CustomerViewModel()
        {
            lstCustomer = new ObservableCollection<KhachHang>(DataProvider.Ins.Entities.KhachHangs);
            lstCustomerType = new ObservableCollection<LoaiKhach>(DataProvider.Ins.Entities.LoaiKhaches);
            lstGender = new ObservableCollection<string>();
            lstGender.Add("Nam");
            lstGender.Add("Nữ");

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) =>
            {
                KhachHang kh = new KhachHang()
                {
                    Hoten = Name,
                    DiaChi = Address,
                    GioiTinh = SelectedGender,
                    LoaiKhach = SelectedCustomerType,
                    CMND_Passport = CMND,
                    Tuoi = Convert.ToInt32(Age),
                    SDT = Phone,
                    HanPassort = Passport,
                    HanVisa = Visa
                };

                DataProvider.Ins.Entities.KhachHangs.Add(kh);
                DataProvider.Ins.Entities.SaveChanges();

                lstCustomer.Add(kh);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedItem != null;
            }, (p) =>
            {
                int index = lstCustomer.IndexOf(SelectedItem);

                KhachHang kh = DataProvider.Ins.Entities.KhachHangs.Where(w => w.MaKH == SelectedItem.MaKH).FirstOrDefault();
                kh.Hoten = Name;
                kh.DiaChi = Address;
                kh.GioiTinh = SelectedGender;
                kh.LoaiKhach = SelectedCustomerType;
                kh.CMND_Passport = CMND;
                kh.Tuoi = Convert.ToInt32(Age);
                kh.SDT = Phone;
                kh.HanPassort = Passport;
                kh.HanVisa = Visa;

                DataProvider.Ins.Entities.SaveChanges();
                
                lstCustomer[index] = new KhachHang()
                {
                    MaKH = kh.MaKH,
                    Hoten = Name,
                    DiaChi = Address,
                    GioiTinh = SelectedGender,
                    LoaiKhach = SelectedCustomerType,
                    CMND_Passport = CMND,
                    Tuoi = Convert.ToInt32(Age),
                    SDT = Phone,
                    HanPassort = Passport,
                    HanVisa = Visa
                };
                SelectedItem = lstCustomer[index];

                if (SelectedItem == null)
                {
                    MessageBox.Show("NULL");
                }
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>{
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstCustomer);
                view.Filter = CustomerFilter;


            });
        }

        private bool CustomerFilter(object item)
        {
            if (filterAge(item) && filterName(item))
                    return true;

            return false;
        }

        #region Filter

        private bool filterName(object item)
        {
            KhachHang kh = item as KhachHang;

            if (string.IsNullOrEmpty(Name) || kh.Hoten.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterAge(object item)
        {
            KhachHang kh = item as KhachHang;

            if (string.IsNullOrEmpty(Age) || kh.Tuoi.Equals(Age))
            {
                return true;
            }
            return false;
        }

        #endregion

        private bool isCommandEnable()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Age)
                || string.IsNullOrEmpty(CMND) || string.IsNullOrEmpty(Phone)
                || string.IsNullOrEmpty(Address) || SelectedGender == null || SelectedCustomerType == null){
                return false;
            }

            return true;
        }
    }
}
