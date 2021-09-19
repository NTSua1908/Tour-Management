using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class PersonalInformationViewModel : BaseViewModel
    {
        private string _UserName;
        public string UserName { get { return _UserName; } set { _UserName = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }

        private string _RePassword;
        public string RePassword { get { return _RePassword; } set { _RePassword = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Age;
        public string Age { get { return _Age; } set { _Age = value; OnPropertyChanged(); } }

        private string _CMND;
        public string CMND { get { return _CMND; } set { _CMND = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }

        private User user;

        public PersonalInformationViewModel()
        {

        }

        public void setUser(User user)
        {
            this.user = user;

            UserName = this.user.Taikhoan;
            DisplayName = this.user.HoTen;
            Age = this.user.Tuoi.ToString();
            CMND = this.user.CMND;
            Phone = this.user.SDT;
        }
    }
}
