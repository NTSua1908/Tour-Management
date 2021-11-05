using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    public class RegisterAccountViewModel : BaseViewModel
    {
        public ICommand NextAvatarCommand { get; set; }
        public ICommand PreviousAvatarCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        private ObservableCollection<LoaiUser> _lstUserType;
        public ObservableCollection<LoaiUser> lstUserType { get { return _lstUserType; } set { _lstUserType = value; OnPropertyChanged(); } }

        private string _UserName;
        public string UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }

        public string Password;
        //public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }

        public string RePassword;
        //public string RePassword { get { return _RePassword; } set { _RePassword = value; OnPropertyChanged(); } }

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

        private string _AvatarDisplayIndex;
        public string AvatarDisplayIndex { get { return _AvatarDisplayIndex; } set { _AvatarDisplayIndex = value; OnPropertyChanged(); } }

        private ImageSource _Avatar;
        public ImageSource Avatar { get { return _Avatar; } set { _Avatar = value; OnPropertyChanged(); } }

        public int? _AvatarIndex;
        public int? AvatarIndex
        {
            get
            {
                return _AvatarIndex;
            }
            set
            {
                _AvatarIndex = value;
                OnPropertyChanged();
                if (AvatarIndex != null)
                {
                    AvatarDisplayIndex = (AvatarIndex + 1).ToString();
                }
            }
        }

        public RegisterAccountViewModel()
        {
            lstUserType = new ObservableCollection<LoaiUser>(DataProvider.Ins.Entities.LoaiUsers);

            AvatarIndex = 0;
            LoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { setAvatar((int)AvatarIndex); }); //LoaiUser loai = new LoaiUser(); loai.

            NextAvatarCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (AvatarIndex + 1 >= 9)
                {
                    AvatarIndex = 0;
                }
                else AvatarIndex++;
                setAvatar((int)AvatarIndex);
            });

            PreviousAvatarCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (AvatarIndex - 1 < 0)
                {
                    AvatarIndex = 8;
                }
                else AvatarIndex--;
                setAvatar((int)AvatarIndex);
            });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                Password = p.Password;
            });

            RePasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                RePassword = p.Password;
            });

            RegisterCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RePassword)
                || Password.Length == 0 || RePassword.Length == 0 || string.IsNullOrEmpty(UserName)
                || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(CMND)
                || string.IsNullOrEmpty(Age) || SelectedUserType == null )
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                if  (addUser())
                    MessageBox.Show("Đăng kí thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                else MessageBox.Show("Đăng kí thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            });

            ExitCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });
        }

        public bool addUser()
        {
            try
            {
                if (!Password.Equals(RePassword)) //Kiểm tra 2 lần nhập mậ khẩu mới có trùng khớp nhau không
                {
                    MessageBox.Show("Mật khẩu không trùng khớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                User newUser = new User();

                newUser.Tuoi = Convert.ToInt32(Age);

                if (newUser.Tuoi < 18 || newUser.Tuoi > 60)
                    return false;
                newUser.HoTen = DisplayName;
                newUser.CMND = CMND;
                newUser.Avatar = AvatarIndex;
                
                newUser.SDT = Phone;
                newUser.Taikhoan = UserName;
                newUser.Password = MD5Hash(Base64Encode(Password));
                newUser.LoaiUser = SelectedUserType;
                DataProvider.Ins.Entities.Users.Add(newUser);
                DataProvider.Ins.Entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void setAvatar(int index)
        {
            Avatar = new BitmapImage(new Uri("pack://application:,,,/Tour%20Management;component/Resources/avatar" + index + ".png", UriKind.Absolute));
            //MessageBox.Show("avatar" + AvatarIndex + ".png");
        }

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

        #region Encode
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        #endregion
    }
}
