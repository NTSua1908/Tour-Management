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
    class AreaViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        private ObservableCollection<KhuVuc> _lstArea;
        public ObservableCollection<KhuVuc> lstArea { get { return _lstArea; } set { _lstArea = value; OnPropertyChanged(); } }
        private string _TenKhuVuc;
        public string TenKhuVuc { get { return _TenKhuVuc; } set { _TenKhuVuc = value; OnPropertyChanged(); } }
        public KhuVuc _SelectedItem;
        public KhuVuc SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    TenKhuVuc = SelectedItem.TenKhuVuc;
                }
            }
        }
        public AreaViewModel()
        {
            lstArea = new ObservableCollection<KhuVuc>(DataProvider.Ins.Entities.KhuVucs);
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstArea = new ObservableCollection<KhuVuc>(DataProvider.Ins.Entities.KhuVucs);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable();
            }, (p) =>
            {
                KhuVuc kv = new KhuVuc()
                {
                    TenKhuVuc = TenKhuVuc,
                };

                DataProvider.Ins.Entities.KhuVucs.Add(kv);
                DataProvider.Ins.Entities.SaveChanges();

                lstArea.Add(kv);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedItem != null;
            }, (p) =>
            {
                int index = lstArea.IndexOf(SelectedItem);

                KhuVuc kv = DataProvider.Ins.Entities.KhuVucs.Where(w => w.MaKhuVuc == SelectedItem.MaKhuVuc).FirstOrDefault();
                kv.TenKhuVuc = TenKhuVuc;
                DataProvider.Ins.Entities.SaveChanges();

                lstArea[index] = new KhuVuc()
                {
                    MaKhuVuc = kv.MaKhuVuc,
                    TenKhuVuc = TenKhuVuc,   
                };
                SelectedItem = lstArea[index];

            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) => {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstArea);
                view.Filter = AreaFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedItem != null;
            }, (p) =>
            {
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;
                List<DiaDiem> diadiem = new List<DiaDiem>(DataProvider.Ins.Entities
                    .DiaDiems.Where(x => x.MaKhuVuc == SelectedItem.MaKhuVuc).ToList());
                foreach (DiaDiem item in diadiem)
                {
                    DataProvider.Ins.Entities.DiaDiems.Remove(item);
                }
                List<KhachSan> khachsan = new List<KhachSan>(DataProvider.Ins.Entities
                    .KhachSans.Where(x => x.MaKhuVuc == SelectedItem.MaKhuVuc).ToList());
                foreach (KhachSan item in khachsan)
                {
                    DataProvider.Ins.Entities.KhachSans.Remove(item);
                }
                KhuVuc kv = DataProvider.Ins.Entities.KhuVucs.Where(x => x.MaKhuVuc == SelectedItem.MaKhuVuc).FirstOrDefault();
                DataProvider.Ins.Entities.KhuVucs.Remove(kv);
                DataProvider.Ins.Entities.SaveChanges();
                lstArea.Remove(SelectedItem);

            });
        }
        private bool isCommandEnable()
        {
            if (string.IsNullOrEmpty(TenKhuVuc))
            {
                return false;
            }

            return true;
        }
        private bool AreaFilter(object item)
        {
            KhuVuc kv = item as KhuVuc;
            if (filterTenKhuVuc(kv))
            {
                return true;
            }
            return false;
        }
        #region Filter
        private bool filterTenKhuVuc(KhuVuc kv)
        {
            if (string.IsNullOrEmpty(TenKhuVuc) || kv.TenKhuVuc.ToLower().Contains(TenKhuVuc.ToLower()))
            {
                return true;
            }
            return false;
        }
        #endregion 
    }
}
