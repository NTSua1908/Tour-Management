using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_management.Model;
using System.Windows;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace Tour_management.ViewModel
{
    class HotelViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        private ObservableCollection<KhachSan> _lstHotel;
        public ObservableCollection<KhachSan> lstHotel { get { return _lstHotel; } set { _lstHotel = value; OnPropertyChanged(); } }
        private string _TenKS;
        public string TenKS { get { return _TenKS; } set { _TenKS = value; OnPropertyChanged(); } }
        private string _SDT;
        public string SDT { get { return _SDT; } set { _SDT = value; OnPropertyChanged(); } }
        private string _DiaChi;
        public string DiaChi { get { return _DiaChi; } set { _DiaChi = value; OnPropertyChanged(); } }
        private string _ChiPhi;
        public string ChiPhi { get { return _ChiPhi; } set { _ChiPhi = value; OnPropertyChanged(); } }
        private ObservableCollection<KhuVuc> _lstArea;
        public ObservableCollection<KhuVuc> lstArea { get { return _lstArea; } set { _lstArea = value; OnPropertyChanged(); } }
        private KhuVuc _SelectedArea;
        public KhuVuc SelectedArea { get { return _SelectedArea; } set { _SelectedArea = value; OnPropertyChanged(); } }
        public KhachSan _SelectedItem;
        public KhachSan SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    TenKS = SelectedItem.TenKS;
                    SDT = SelectedItem.SDT.ToString();
                    DiaChi = SelectedItem.DiaChi;
                    ChiPhi = SelectedItem.ChiPhi.ToString();
                    SelectedArea = SelectedItem.KhuVuc;
                }
            }
        }
        public HotelViewModel()
        {
            lstHotel = new ObservableCollection<KhachSan>(DataProvider.Ins.Entities.KhachSans);
            lstArea = new ObservableCollection<KhuVuc>(DataProvider.Ins.Entities.KhuVucs);
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstHotel = new ObservableCollection<KhachSan>(DataProvider.Ins.Entities.KhachSans);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) => 
            {
                KhachSan ks = new KhachSan()
                {
                    TenKS = TenKS,
                    DiaChi = DiaChi,
                    SDT = SDT,
                    ChiPhi = Convert.ToDecimal(ChiPhi),
                    KhuVuc = SelectedArea
                };

                DataProvider.Ins.Entities.KhachSans.Add(ks);
                DataProvider.Ins.Entities.SaveChanges();

                lstHotel.Add(ks);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedItem != null;
            }, (p) =>
            {
                int index = lstHotel.IndexOf(SelectedItem);

                KhachSan ks = DataProvider.Ins.Entities.KhachSans.Where(w => w.MaKS == SelectedItem.MaKS).FirstOrDefault();
                ks.TenKS = TenKS;
                ks.DiaChi = DiaChi;
                ks.SDT = SDT;
                ks.ChiPhi = Convert.ToDecimal(ChiPhi);
                ks.KhuVuc = SelectedArea;
                DataProvider.Ins.Entities.SaveChanges();

                lstHotel[index] = new KhachSan()
                {
                    MaKS = ks.MaKS,
                    TenKS = TenKS,
                    DiaChi = DiaChi,
                    SDT = SDT,
                    ChiPhi = Convert.ToDecimal(ChiPhi),
                    KhuVuc = SelectedArea
                };
                SelectedItem = lstHotel[index];

            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) => {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstHotel);
                view.Filter = HotelFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedItem != null;
            }, (p) =>
            {
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;
                List<DSKhachSan> lstkhachsan = new List<DSKhachSan>(DataProvider.Ins.Entities
                    .DSKhachSans.Where(x => x.MaKS == SelectedItem.MaKS).ToList());

                foreach (DSKhachSan item in lstkhachsan)
                {
                    DataProvider.Ins.Entities.DSKhachSans.Remove(item);
                }

                //Tui chỉnh lại dòng này, lỗi xảy ra khi mình sửa một khách sạn, sau đó xóa nó sẽ xảy ra lỗi
                //Tui khắc phục nó bằng dòng bên dưới :(( 
                //Tui cũng chưa rõ nguyên nhân thật sự gây ra lỗi
                KhachSan ks = DataProvider.Ins.Entities.KhachSans.Where(x => x.MaKS == SelectedItem.MaKS).FirstOrDefault();
                DataProvider.Ins.Entities.KhachSans.Remove(ks);
                DataProvider.Ins.Entities.SaveChanges();

                lstHotel.Remove(SelectedItem);

            });
        }
        private bool isCommandEnable()
        {
            if (string.IsNullOrEmpty(TenKS) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(DiaChi)
                || string.IsNullOrEmpty(ChiPhi) || SelectedArea == null || SelectedArea.TenKhuVuc.Equals("Null"))
            {
                return false;
            }

            return true;
        }
        private bool HotelFilter(object item)
        {
            KhachSan ks = item as KhachSan;
            if (filterTenKS(ks) && filterSDT(ks) && filterChiPhi(ks) && filterDiaChi(ks) && filterKhuVuc(ks))
            { 
                return true; 
            }
            return false;
        }
        #region Filter
        private bool filterTenKS(KhachSan ks)
        {
            if (string.IsNullOrEmpty(TenKS) || ks.TenKS.ToLower().Contains(TenKS.ToLower()))
            {
                return true;
            }
            return false;
        }
        private bool filterKhuVuc(KhachSan ks)
        {
            if (SelectedArea == null || SelectedArea.TenKhuVuc.Equals("Null") || ks.KhuVuc == SelectedArea)
            {
                return true;
            }

            return false;
        }
        private bool filterSDT(KhachSan ks)
        {
            if (string.IsNullOrEmpty(SDT) || ks.SDT.ToLower().Contains(SDT.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterChiPhi(KhachSan ks)
        {
            if (string.IsNullOrEmpty(ChiPhi))
            {
                return true;
            }
            else
            {
                Decimal Tien = Convert.ToDecimal(ChiPhi);
                if (ks.ChiPhi.Equals(Tien))
                {
                    return true;
                }
            }
            return false;
        }
        private bool filterDiaChi(KhachSan ks)
        {
            if (string.IsNullOrEmpty(DiaChi) || ks.DiaChi.ToLower().Contains(DiaChi.ToLower()))
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
        public void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; //Neu la phim cach thi xem nhu da xu ly (bo qua phim do)
            }
        }
        public void NumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        #endregion
    }
}
