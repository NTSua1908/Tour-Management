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
    class LoyalCustomersViewModel : BaseViewModel
    {
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private bool _isForeign;
        public bool isForeign { get { return _isForeign; } set { _isForeign = value; OnPropertyChanged(); } }

        private ObservableCollection<KhachHangThanThiet> _lstLoyalCustomer;
        public ObservableCollection<KhachHangThanThiet> lstLoyalCustomer { get { return _lstLoyalCustomer; } set { _lstLoyalCustomer = value; OnPropertyChanged(); } }

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
        public LoaiKhach SelectedCustomerType
        {
            get { return _SelectedCustomerType; }
            set
            {
                _SelectedCustomerType = value;
                OnPropertyChanged();

                //Nếu chọn khách nước ngoài thì enable 2 datepicker Visa và Passport
                if (SelectedCustomerType != null && SelectedCustomerType.TenLoaiKhach.Equals("Foreign"))
                {
                    isForeign = true;
                }
                else isForeign = false;
            }
        }

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

        public KhachHangThanThiet _SelectedItem;
        public KhachHangThanThiet SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Name = SelectedItem.KhachHang.Hoten;
                    Age = SelectedItem.KhachHang.Tuoi.ToString();
                    CMND = SelectedItem.KhachHang.CMND_Passport.ToString();
                    Phone = SelectedItem.KhachHang.SDT.ToString();
                    Address = SelectedItem.KhachHang.DiaChi;
                    Visa = SelectedItem.KhachHang.HanVisa;
                    Passport = SelectedItem.KhachHang.HanPassort;
                    SelectedGender = SelectedItem.KhachHang.GioiTinh;
                    SelectedCustomerType = SelectedItem.KhachHang.LoaiKhach;
                }
            }
        }

        public LoyalCustomersViewModel()
        {
            lstLoyalCustomer = new ObservableCollection<KhachHangThanThiet>(DataProvider.Ins.Entities.KhachHangThanThiets);
            lstCustomerType = new ObservableCollection<LoaiKhach>(DataProvider.Ins.Entities.LoaiKhaches);
            lstCustomerType.Add(new LoaiKhach()
            {
                TenLoaiKhach = "Null" //Người dùng chọn mục này khi muốn tìm kiếm người dùng thuộc tất cả loại khách
            });
            lstGender = new ObservableCollection<string>();
            lstGender.Add("Nam");
            lstGender.Add("Nữ");
            lstGender.Add("Null"); //Người dùng chọn mục này khi muốn tìm kiếm cả nam và nữ

            //Load lại toàn bộ bảng
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstLoyalCustomer = new ObservableCollection<KhachHangThanThiet>(DataProvider.Ins.Entities.KhachHangThanThiets);
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) => {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstLoyalCustomer);
                view.Filter = CustomerFilter;
            });
        }

        private bool CustomerFilter(object item)
        {
            KhachHang kh = item as KhachHang;
            if (filterAge(kh) && filterName(kh) && filterGender(kh)
                && filterCMND(kh) && filterPhone(kh) && filterAddress(kh)
                && filterVisa(kh) && filterPassport(kh) && filterType(kh))
                return true;

            return false;
        }

        #region Filter

        private bool filterName(KhachHang kh)
        {
            if (string.IsNullOrEmpty(Name) || kh.Hoten.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterAge(KhachHang kh)
        {
            if (string.IsNullOrEmpty(Age))
            {
                return true;
            }
            else
            {
                int Tuoi = Convert.ToInt32(Age);
                if (kh.Tuoi.Equals(Tuoi))
                {
                    return true;
                }
            }
            return false;
        }

        private bool filterGender(KhachHang kh)
        {
            if (SelectedGender == null || SelectedGender.Equals("Null") || kh.GioiTinh == SelectedGender)
            {
                return true;
            }

            return false;
        }

        private bool filterCMND(KhachHang kh)
        {
            if (string.IsNullOrEmpty(CMND) || kh.CMND_Passport.ToLower().Contains(CMND.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterPhone(KhachHang kh)
        {
            if (string.IsNullOrEmpty(Phone) || kh.SDT.ToLower().Contains(Phone.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterAddress(KhachHang kh)
        {
            if (string.IsNullOrEmpty(Address) || kh.DiaChi.ToLower().Contains(Address.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterVisa(KhachHang kh)
        {
            if (Visa == null || kh.HanVisa == Visa)
            {
                return true;
            }

            return false;
        }

        private bool filterPassport(KhachHang kh)
        {
            if (Passport == null || kh.HanPassort == Passport)
            {
                return true;
            }

            return false;
        }

        private bool filterType(KhachHang kh)
        {
            if (SelectedCustomerType == null || SelectedCustomerType.TenLoaiKhach.Equals("Null") || kh.LoaiKhach == SelectedCustomerType)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Number Input 
        // Ràng dữ liệu các textbox như tuổi, sđt... chỉ được nhập số
        // Hiện tại vẫn chưa hoàn thiện chức năng chặn người dùng paste dữ liệu text (không phải số) vào textbox

        private Regex _regex = new Regex("[^0-9.-]+"); //Dinh dang chi cho phep go chu so
        private bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public void NumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        public void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; //Neu la phim cach thi xem nhu da xu ly (bo qua phim do)
            }
        }

        //Nếu như paste dữ liệu vào textbox là nó không phải là số thì cũng chặn
        //Nhưng hàm này không binding dược, chưa hiểu tại sao, tạm thời để đó
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            MessageBox.Show("Paste");
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
        #endregion
    }
}
