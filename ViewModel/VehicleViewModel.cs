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
    class VehicleViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        private ObservableCollection<PhuongTien> _lstVehicle;
        public ObservableCollection<PhuongTien> lstVehicle
        {
            get { return _lstVehicle; }
            set { _lstVehicle = value ; OnPropertyChanged(); }
        }
        private string _TenPT;
        public string TenPT
        {
            get { return _TenPT; }
            set { _TenPT = value; OnPropertyChanged(); }
        }
        private string _ChiPhi;
        public string ChiPhi
        {
            get { return _ChiPhi; }
            set { _ChiPhi = value; OnPropertyChanged(); }
        }
        public PhuongTien _SelectedItem;
        public PhuongTien SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if(SelectedItem!=null)
                {
                    TenPT = SelectedItem.TenPT;
                    ChiPhi = SelectedItem.ChiPhi.ToString();
                }    
            }
        }
        public VehicleViewModel()
        {
            lstVehicle= new ObservableCollection<PhuongTien>(DataProvider.Ins.Entities.PhuongTiens);
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstVehicle = new ObservableCollection<PhuongTien>(DataProvider.Ins.Entities.PhuongTiens);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) =>
            {
                PhuongTien pt = new PhuongTien()
                {
                    TenPT=TenPT,
                    ChiPhi = Convert.ToDecimal(ChiPhi)
                };

                DataProvider.Ins.Entities.PhuongTiens.Add(pt);
                DataProvider.Ins.Entities.SaveChanges();

                lstVehicle.Add(pt);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedItem != null;
            }, (p) =>
            {
                int index = lstVehicle.IndexOf(SelectedItem);

                PhuongTien pt = DataProvider.Ins.Entities.PhuongTiens.Where(w => w.MaPT == SelectedItem.MaPT).FirstOrDefault();
                pt.TenPT = TenPT;
                pt.ChiPhi = Convert.ToDecimal(ChiPhi);
                DataProvider.Ins.Entities.SaveChanges();

                lstVehicle[index] = new PhuongTien()
                {
                    MaPT = pt.MaPT,
                    TenPT = TenPT,
                    ChiPhi = Convert.ToDecimal(ChiPhi)
                };
                SelectedItem = lstVehicle[index];

                if (SelectedItem == null)
                {
                    MessageBox.Show("NULL");
                }
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) => {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstVehicle);
                view.Filter = VehicleFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedItem != null;
            }, (p) =>
            {
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;
                List<DSPhuongTien> lstphuongtien= new List<DSPhuongTien>(DataProvider.Ins.Entities
                    .DSPhuongTiens.Where(x => x.MaPT == SelectedItem.MaPT).ToList());

                foreach (DSPhuongTien item in lstphuongtien)
                {
                    DataProvider.Ins.Entities.DSPhuongTiens.Remove(item);
                }

                DataProvider.Ins.Entities.PhuongTiens.Remove(SelectedItem);
                DataProvider.Ins.Entities.SaveChanges();

                lstVehicle.Remove(SelectedItem);

            });
        }
        private bool isCommandEnable()
        {
            if (string.IsNullOrEmpty(TenPT) || string.IsNullOrEmpty(ChiPhi) )
            {
                return false;
            }

            return true;
        }
        private bool VehicleFilter(object item)
        {
            PhuongTien pt = item as PhuongTien;
            if (filterTenPT(pt) && filterChiPhi(pt))
            {
                return true;
            }
            return false;
        }
        #region Filter
        private bool filterTenPT(PhuongTien pt)
        {
            if (string.IsNullOrEmpty(TenPT) || pt.TenPT.ToLower().Contains(TenPT.ToLower()))
            {
                return true;
            }
            return false;
        }
        private bool filterChiPhi(PhuongTien pt)
        {
            if (string.IsNullOrEmpty(ChiPhi))
            {
                return true;
            }
            else
            {
                Decimal Tien = Convert.ToDecimal(ChiPhi);
                if (pt.ChiPhi.Equals(Tien))
                {
                    return true;
                }
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
        public void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        public void NumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        #endregion
    }
}
