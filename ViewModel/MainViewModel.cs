using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tour_management.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }

        public MainViewModel()
        {
            //Goi ham nay de thuc hien dang nhap
            //LoadedCommand = new RelayCommand<Window>((p) => { return true; },  (p) => { Login(p); } );
        }

        void Login(Window w)
        {
            w.Hide();
            LoginWindow login = new LoginWindow();
            LoginViewModel loginViewModel = (LoginViewModel)login.DataContext;
            login.ShowDialog();

            if (loginViewModel.isLogin)
            {
                w.Show();
            }
            else w.Close();

        }
    }
}
