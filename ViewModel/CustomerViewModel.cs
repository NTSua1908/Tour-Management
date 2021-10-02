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
    class CustomerViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private bool _isForeign;
        public bool isForeign { get { return _isForeign; } set { _isForeign = value; OnPropertyChanged(); } }

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
        public LoaiKhach SelectedCustomerType { 
            get { return _SelectedCustomerType; } 
            set {
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
            lstCustomerType.Add(new LoaiKhach() { 
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
                lstCustomer = new ObservableCollection<KhachHang>(DataProvider.Ins.Entities.KhachHangs);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable(); //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) => //Đây là đoạn xử lý khi button được nhấn Các command sau tương tự
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
            },  (p) =>{
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstCustomer);
                view.Filter = CustomerFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedItem != null;
            }, (p) =>
            {
                //Hoi lai cho chac
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                //Tìm tất cả các bảng có liên quan đến khách hàng và xóa nó trước khi xóa khách hàng đó (cẩn thận!!!)

                //Bảng khách du lịch là bảng có liên quan!!!
                List<KhachDuLich> lstKhach = new List<KhachDuLich>(DataProvider.Ins.Entities
                    .KhachDuLiches.Where(x => x.MaKH == SelectedItem.MaKH).ToList());

                foreach (KhachDuLich item in lstKhach)
                {
                    DataProvider.Ins.Entities.KhachDuLiches.Remove(item);
                }

                DataProvider.Ins.Entities.KhachHangs.Remove(SelectedItem);
                DataProvider.Ins.Entities.SaveChanges();

                lstCustomer.Remove(SelectedItem); 

            });
        }

        /// <summary>
        /// Lọc danh sách khách hàng, lấy mỗi khách hàng trong list ra xét, return true nếu thỏa và ngược lại
        /// </summary>
        /// <param name="item">Mỗi item trong trường hợp này là một khách hàng</param>
        /// <returns></returns>
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

        private bool isCommandEnable() //Đúng chỉ khi những thông tin cần thiết đã được nhập
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Age) || string.IsNullOrEmpty(CMND)
                || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address) 
                || SelectedGender == null || SelectedCustomerType == null
                || SelectedGender.Equals("Null") || SelectedCustomerType.TenLoaiKhach.Equals("Null")
                || (SelectedCustomerType.TenLoaiKhach.Equals("Foreign") && (Visa == null || Passport == null))) //Nếu là nước ngoài thì visa và passort phải được nhập
                {
                return false;
            }

            return true;
        }
    }
}
