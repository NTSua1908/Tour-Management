using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool isLogin { get; set; }
        public User user { get; set; }

        public ICommand ExitCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        private string _Password;
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }

        private string _UserName;
        public string UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }

        public LoginViewModel()
        {
            isLogin = false;

            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            });

            LoginCommand = new RelayCommand<Window>((p) => 
            {
                if (Password != null && UserName != null && Password.Length > 0 && UserName.Length > 0)
                {
                    return true;
                } 
                return false; 
            }, (p) => {

                string password = MD5Hash(Base64Encode(Password));
                user = DataProvider.Ins.Entities.Users.Where(w => w.Taikhoan == UserName && w.Password == password).FirstOrDefault();

                if (user != null)
                {
                    isLogin = true;
                    p.Close();
                }
                else
                {
                    isLogin = false;
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

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
    }
}
