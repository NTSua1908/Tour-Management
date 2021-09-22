using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        //...

        public UserManagementViewModel()
        {
            lstUser = new ObservableCollection<User>(DataProvider.Ins.Entities.Users);
            //User user = new User(); user.
        }
    }
}
