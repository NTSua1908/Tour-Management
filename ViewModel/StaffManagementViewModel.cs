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
    class StaffManagementViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        private ObservableCollection<NhanVien> _lstStaff;
        public ObservableCollection<NhanVien> lstStaff { get { return _lstStaff; } set { _lstStaff = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _lstGender;
        public ObservableCollection<string> lstGender { get { return _lstGender; } set { _lstGender = value; OnPropertyChanged(); } }
        private string _StaffName;
        public string StaffName { get { return _StaffName; } set { _StaffName = value; OnPropertyChanged(); } }
        private string _SelectedGender;
        public string SelectedGender { get { return _SelectedGender; } set { _SelectedGender = value; OnPropertyChanged(); } }
        private string _CMND;
        public string CMND { get { return _CMND; } set { _CMND = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }
        public NhanVien _SelectedItem;
        public NhanVien SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    StaffName = SelectedItem.TenNV;
                    CMND = SelectedItem.CMND.ToString();
                    Phone = SelectedItem.SDT.ToString();
                    SelectedGender = SelectedItem.GioiTinh;
                }
            }
        }
        public StaffManagementViewModel()
        {
            lstStaff = new ObservableCollection<NhanVien>(DataProvider.Ins.Entities.NhanViens);
            lstGender = new ObservableCollection<string>();
            lstGender.Add("Nam");
            lstGender.Add("Nữ");
            lstGender.Add("Null");
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstStaff = new ObservableCollection<NhanVien>(DataProvider.Ins.Entities.NhanViens);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) => 
            {
                NhanVien nv = new NhanVien()
                {
                    TenNV = StaffName,
                    GioiTinh = SelectedGender,
                    CMND = CMND,
                    SDT = Phone,
                };

                DataProvider.Ins.Entities.NhanViens.Add(nv);
                DataProvider.Ins.Entities.SaveChanges();

                lstStaff.Add(nv);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedItem != null;
            }, (p) =>
            {
                int index = lstStaff.IndexOf(SelectedItem);

                NhanVien nv = DataProvider.Ins.Entities.NhanViens.Where(w => w.MaNV == SelectedItem.MaNV).FirstOrDefault();
                nv.TenNV = StaffName;
                nv.GioiTinh = SelectedGender;
                nv.CMND = CMND;
                nv.SDT = Phone;

                DataProvider.Ins.Entities.SaveChanges();

                lstStaff[index] = new NhanVien()
                {
                    MaNV = nv.MaNV,
                    TenNV = StaffName,
                    GioiTinh = SelectedGender,
                    CMND = CMND,
                    SDT = Phone,
                };
                SelectedItem = lstStaff[index];

                if (SelectedItem == null)
                {
                    MessageBox.Show("NULL");
                }
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) => {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstStaff);
                view.Filter = StaffFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedItem != null;
            }, (p) =>
            {
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;
                List<DSNhanVien> lstnhanvien = new List<DSNhanVien>(DataProvider.Ins.Entities
                    .DSNhanViens.Where(x => x.MaNV == SelectedItem.MaNV).ToList());

                foreach (DSNhanVien item in lstnhanvien)
                {
                    DataProvider.Ins.Entities.DSNhanViens.Remove(item);
                }

                DataProvider.Ins.Entities.NhanViens.Remove(SelectedItem);
                DataProvider.Ins.Entities.SaveChanges();

                lstStaff.Remove(SelectedItem);

            });
        }
        private bool isCommandEnable()
        {
            if (string.IsNullOrEmpty(StaffName) || string.IsNullOrEmpty(CMND)
                || string.IsNullOrEmpty(Phone)  || SelectedGender == null || SelectedGender.Equals("Null") ) 
            {
                return false;
            }

            return true;
        }
        private bool StaffFilter(object item)
        {
            NhanVien nv = item as NhanVien;
            if (filterName(nv) && filterGender(nv)
                && filterCMND(nv) && filterPhone(nv))
                return true;

            return false;
        }
        #region Filter

        private bool filterName(NhanVien nv)
        {
            if (string.IsNullOrEmpty(StaffName) || nv.TenNV.ToLower().Contains(StaffName.ToLower()))
            {
                return true;
            }
            return false;
        }
        private bool filterGender(NhanVien kh)
        {
            if (SelectedGender == null || SelectedGender.Equals("Null") || kh.GioiTinh == SelectedGender)
            {
                return true;
            }

            return false;
        }

        private bool filterCMND(NhanVien kh)
        {
            if (string.IsNullOrEmpty(CMND) || kh.CMND.ToLower().Contains(CMND.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterPhone(NhanVien kh)
        {
            if (string.IsNullOrEmpty(Phone) || kh.SDT.ToLower().Contains(Phone.ToLower()))
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Number Input 
        private Regex _regex = new Regex("[^0-9.-]+"); 
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
                e.Handled = true; 
            }
        }
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
