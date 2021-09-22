using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Tour_management.Properties;

namespace Tour_management.ViewModel
{
    class PersonalInformationViewModel : BaseViewModel
    {
        public ICommand NextAvatarCommand { get; set; }
        public ICommand PreviousAvatarCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand CurrentPasswordChangedCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }

        private string _UserName;
        public string UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }

        private string CurrentPassword;
        private string NewPassword;
        //public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }

        private string RePassword;
        //public string RePassword { get { return _RePassword; } set { _RePassword = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Age;
        public string Age { get { return _Age; } set { _Age = value; OnPropertyChanged(); } }

        private string _CMND;
        public string CMND { get { return _CMND; } set { _CMND = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }

        private string _AvatarDisplayIndex;
        public string AvatarDisplayIndex { get { return _AvatarDisplayIndex; } set { _AvatarDisplayIndex = value; OnPropertyChanged(); } }

        private ImageSource _Avatar;
        public ImageSource Avatar { get { return _Avatar; } set { _Avatar = value; OnPropertyChanged(); } }

        private int? _AvatarIndex;
        public int? AvatarIndex { 
            get { 
                return _AvatarIndex; 
            } 
            set { 
                _AvatarIndex = value; 
                OnPropertyChanged();
                if (AvatarIndex!=null)
                {
                    AvatarDisplayIndex = (AvatarIndex + 1).ToString();
                } 
            } 
        }

        private User user;

        public PersonalInformationViewModel()
        {
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

            CurrentPasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                CurrentPassword = p.Password;
            }); 
            
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                NewPassword = p.Password;
            });

            RePasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
                RePassword = p.Password;
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(DisplayName) 
                || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(CMND)
                || string.IsNullOrEmpty(Age))
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                User UpdateUser = DataProvider.Ins.Entities.Users.Where(x => x.ID == user.ID).FirstOrDefault();
                if (!string.IsNullOrEmpty(CurrentPassword) && !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(RePassword)
                && CurrentPassword.Length != 0 && NewPassword.Length != 0 && RePassword.Length != 0)
                {
                    if (!MD5Hash(Base64Encode(CurrentPassword)).Equals(user.Password)) //Kiểm tra mật khẩu hiện tại đã đúng hay chưa
                    {
                        MessageBox.Show("Mật khẩu không đúng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!NewPassword.Equals(RePassword)) //Kiểm tra 2 lần nhập mậ khẩu mới có trùng khớp nhau không
                    {
                        MessageBox.Show("Mật khẩu không trùng khớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    UpdateUser.Password = MD5Hash(Base64Encode(NewPassword));
                }

                UpdateUser.HoTen = DisplayName;
                UpdateUser.CMND = CMND;
                UpdateUser.Avatar = AvatarIndex;
                UpdateUser.SDT = Phone;
                UpdateUser.Tuoi = Convert.ToInt32(Age);
                UpdateUser.Taikhoan = UserName;
                DataProvider.Ins.Entities.SaveChanges();
                user = UpdateUser;

                MessageBox.Show("Đã lưu thay đổi", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            ExitCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });
        }

        void setAvatar(int index)
        {
            Avatar = new BitmapImage(new Uri("pack://application:,,,/Tour%20management;component/Resources/avatar" + index + ".png", UriKind.Absolute));
            //MessageBox.Show("avatar" + AvatarIndex + ".png");
        }

        public void setUser(User user)
        {
            this.user = user;

            UserName = this.user.Taikhoan;
            DisplayName = this.user.HoTen;
            Age = this.user.Tuoi.ToString();
            CMND = this.user.CMND;
            Phone = this.user.SDT;
            AvatarIndex = this.user.Avatar;

            setAvatar((int)AvatarIndex);
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
            MessageBox.Show("Paste");
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
