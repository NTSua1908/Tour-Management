using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Tour_management.Model;

namespace Tour_management.ViewModel
{
    class DestinationViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<DiaDiem> _lstDestination;
        public ObservableCollection<DiaDiem> lstDestination { get { return _lstDestination; } set { _lstDestination = value; OnPropertyChanged(); } }

        private ObservableCollection<KhuVuc> _lstArea;
        public ObservableCollection<KhuVuc> lstArea { get { return _lstArea; } set { _lstArea = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private KhuVuc _SelectedArea;
        public KhuVuc SelectedArea
        {
            get { return _SelectedArea; }
            set
            {
                _SelectedArea = value;
                OnPropertyChanged();
            }
        }

        public DiaDiem _SelectedDestination;
        public DiaDiem SelectedDestination
        {
            get { return _SelectedDestination; }
            set
            {
                _SelectedDestination = value;
                OnPropertyChanged();
                if (SelectedDestination != null)
                {
                    Name = SelectedDestination.TenDD;
                    SelectedArea = SelectedDestination.KhuVuc;
                }
            }
        }

        public DestinationViewModel()
        {
            lstDestination = new ObservableCollection<DiaDiem>(DataProvider.Ins.Entities.DiaDiems);
            lstArea = new ObservableCollection<KhuVuc>(DataProvider.Ins.Entities.KhuVucs);

            //Load lại toàn bộ bảng
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstDestination = new ObservableCollection<DiaDiem>(DataProvider.Ins.Entities.DiaDiems);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable(); //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) => //Đây là đoạn xử lý khi button được nhấn Các command sau tương tự
            {
                DiaDiem diaDiem = new DiaDiem()
                {
                    TenDD = Name,
                    KhuVuc = SelectedArea
                };

                DataProvider.Ins.Entities.DiaDiems.Add(diaDiem);
                DataProvider.Ins.Entities.SaveChanges();

                lstDestination.Add(diaDiem);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedDestination != null;
            }, (p) =>
            {
                int index = lstDestination.IndexOf(SelectedDestination);

                DiaDiem diaDiem = DataProvider.Ins.Entities.DiaDiems.Where(w => w.MaDD == SelectedDestination.MaDD).FirstOrDefault();
                diaDiem.TenDD = Name;
                diaDiem.KhuVuc = SelectedArea;
                DataProvider.Ins.Entities.SaveChanges();

                lstDestination[index] = new DiaDiem()
                {
                    MaDD = diaDiem.MaDD,
                    TenDD = Name,
                    KhuVuc = SelectedArea,
                    //MaKhuVuc = diaDiem.MaKhuVuc
                };
                SelectedDestination = lstDestination[index];
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstDestination);
                view.Filter = CustomerFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedDestination != null;
            }, (p) =>
            {
                //Hoi lai cho chac
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                List<DSDiaDiem> lstDiaDiem = new List<DSDiaDiem>(DataProvider.Ins.Entities
                    .DSDiaDiems.Where(x => x.MaDD == SelectedDestination.MaDD).ToList());
                
                foreach (DSDiaDiem item in lstDiaDiem)
                {
                    DataProvider.Ins.Entities.DSDiaDiems.Remove(item);
                }

                DiaDiem dd = DataProvider.Ins.Entities.DiaDiems.Where(x => x.MaDD == SelectedDestination.MaDD).FirstOrDefault();

                DataProvider.Ins.Entities.DiaDiems.Remove(dd);
                DataProvider.Ins.Entities.SaveChanges();

                lstDestination.Remove(SelectedDestination);

            });
        }

        private bool CustomerFilter(object item)
        {
            DiaDiem diaDiem = item as DiaDiem;
            if (filterName(diaDiem) && filterArea(diaDiem))
                return true;

            return false;
        }

        #region Filter

        private bool filterName(DiaDiem diadiem)
        {
            if (string.IsNullOrEmpty(Name) || diadiem.TenDD.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterArea(DiaDiem diadiem)
        {
            if (SelectedArea == null || diadiem.KhuVuc == SelectedArea)
            {
                return true;
            }

            return false;
        }
        #endregion


        private bool isCommandEnable() //Đúng chỉ khi những thông tin cần thiết đã được nhập
        {
            if (string.IsNullOrEmpty(Name) || SelectedArea == null)
            {
                return false;
            }

            return true;
        }
    }
}
