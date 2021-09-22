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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class UserManagementViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<User> _lstUser;
        public ObservableCollection<User> lstUser { get { return _lstUser; } set { _lstUser = value; OnPropertyChanged(); } }

        private ObservableCollection<LoaiUser> _lstUserType;
        public ObservableCollection<LoaiUser> lstUserType { get { return _lstUserType; } set { _lstUserType = value; OnPropertyChanged(); } }

        private ImageSource _Avatar;
        public ImageSource Avatar { get { return _Avatar; } set { _Avatar = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Age;
        public string Age { get { return _Age; } set { _Age = value; OnPropertyChanged(); } }

        private string _CMND;
        public string CMND { get { return _CMND; } set { _CMND = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }

        private LoaiUser _SelectedUserType;
        public LoaiUser SelectedUserType { get { return _SelectedUserType; } set { _SelectedUserType = value; OnPropertyChanged(); } }

        private User _SelectedUser;
        public User SelectedUser {
            get {
                return _SelectedUser;
            } set {
                _SelectedUser = value;
                OnPropertyChanged();
                if (SelectedUser != null)
                {
                    Avatar = new BitmapImage(new Uri("pack://application:,,,/Tour%20management;component/Resources/avatar" + SelectedUser.Avatar + ".png", UriKind.Absolute));        
                    DisplayName = SelectedUser.HoTen;
                    Age = SelectedUser.Tuoi.ToString();
                    CMND = SelectedUser.CMND;
                    Phone = SelectedUser.SDT;
                    SelectedUserType = SelectedUser.LoaiUser;
                }
            }
        }

        public UserManagementViewModel()
        {
            lstUser = new ObservableCollection<User>(DataProvider.Ins.Entities.Users);
            lstUserType = new ObservableCollection<LoaiUser>(DataProvider.Ins.Entities.LoaiUsers);
            lstUserType.Add(new LoaiUser() { TenLoai = "Null" });
            Avatar = new BitmapImage(new Uri("pack://application:,,,/Tour%20management;component/Resources/user.png", UriKind.Absolute));

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                RegisterAccount register = new RegisterAccount();
                register.ShowDialog();

                //Sau khi thêm xong cần reload lại để cập nhật danh sách
                lstUser = new ObservableCollection<User>(DataProvider.Ins.Entities.Users);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Age)
                || string.IsNullOrEmpty(CMND) || string.IsNullOrEmpty(Phone) || SelectedUser == null
                || SelectedUserType == null || SelectedUserType.TenLoai.Equals("Null"))
                {
                    return false; 
                }

                return true;
            }, (p) =>
            {
                int index = lstUser.IndexOf(SelectedUser);

                User user = DataProvider.Ins.Entities.Users.Where(x => x.ID == SelectedUser.ID).FirstOrDefault();
                user.HoTen = DisplayName;
                user.CMND = CMND;
                user.LoaiUser = SelectedUserType;
                user.SDT = Phone;
                user.Tuoi = Convert.ToInt32(Age);
                DataProvider.Ins.Entities.SaveChanges();

                lstUser[index] = new User()
                {
                    ID = user.ID,
                    HoTen = user.HoTen,
                    CMND = user.CMND,
                    LoaiUser = user.LoaiUser,
                    SDT = user.SDT,
                    Tuoi = Convert.ToInt32(Age),
                    Avatar = user.Avatar,
                };
                SelectedUser = lstUser[index];
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedUser != null;
            }, (p) =>
            {
                //Hoi lai cho chac
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                DataProvider.Ins.Entities.Users.Remove(SelectedUser);
                DataProvider.Ins.Entities.SaveChanges();

                lstUser.Remove(SelectedUser);
                //SelectedUser = null;
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) => {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstUser);
                view.Filter = UserFilter;
            });

            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstUser = new ObservableCollection<User>(DataProvider.Ins.Entities.Users);
            });
        }

        private bool UserFilter(object item)
        {
            User user = item as User;
            if (filterAge(user) && filterName(user)
                && filterCMND(user) && filterPhone(user) && filterType(user))
            {
                return true;
            }

            return false;
        }

        #region Filter

        private bool filterName(User user)
        {
            if (string.IsNullOrEmpty(DisplayName) || user.HoTen.ToLower().Contains(DisplayName.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterAge(User user)
        {
            if (string.IsNullOrEmpty(Age))
            {
                return true;
            }
            else
            {
                int Tuoi = Convert.ToInt32(Age);
                if (user.Tuoi.Equals(Tuoi))
                {
                    return true;
                }
            }
            return false;
        }

        private bool filterCMND(User user)
        {
            if (string.IsNullOrEmpty(CMND) || user.CMND.ToLower().Contains(CMND.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterPhone(User user)
        {
            if (string.IsNullOrEmpty(Phone) || user.SDT.ToLower().Contains(Phone.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterType(User user)
        {
            if (SelectedUserType == null || SelectedUserType.TenLoai.Equals("Null") || user.LoaiUser == SelectedUserType)
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
            //MessageBox.Show("Paste");
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
