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
    public class TourViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<Tour> _lstTour;
        public ObservableCollection<Tour> lstTour { get { return _lstTour; } set { _lstTour = value; OnPropertyChanged(); } }

        private ObservableCollection<LoaiTour> _lstTourType;
        public ObservableCollection<LoaiTour> lstTourType { get { return _lstTourType; } set { _lstTourType = value; OnPropertyChanged(); } }

        private ObservableCollection<TourDestination> _lstAllDestination;
        public ObservableCollection<TourDestination> lstAllDestination 
        { get { return _lstAllDestination; } set { _lstAllDestination = value; OnPropertyChanged(); } }

        private ObservableCollection<DiaDiem> _lstPickedDestination;
        public ObservableCollection<DiaDiem> lstPickedDestination { get { return _lstPickedDestination; } set { _lstPickedDestination = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private string _Price;
        public string Price { get { return _Price; } set { _Price = value; OnPropertyChanged(); } }

        private LoaiTour _SelectedTourType;
        public LoaiTour SelectedTourType
        {
            get { return _SelectedTourType; }
            set
            {
                _SelectedTourType = value;
                OnPropertyChanged();
            }
        }

        public Tour _SelectedTour;
        public Tour SelectedTour
        {
            get { return _SelectedTour; }
            set
            {
                _SelectedTour = value;
                OnPropertyChanged();
                if (SelectedTour != null)
                {
                    Name = SelectedTour.TenTour;
                    Price = string.Format("{0:N0}", SelectedTour.GiaTour);
                    SelectedTourType = SelectedTour.LoaiTour;
                    getDestination();
                }
            }
        }

        public TourViewModel()
        {
            lstTour = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);
            lstTourType = new ObservableCollection<LoaiTour>(DataProvider.Ins.Entities.LoaiTours);
            lstTourType.Add(new LoaiTour() { TenLoaiTour = "Null" }); //Search ca 2 loai
            lstPickedDestination = new ObservableCollection<DiaDiem>();

            //Khoi tao du lieu cho combobox
            List<DiaDiem> lstDestination = new List<DiaDiem>(DataProvider.Ins.Entities.DiaDiems);
            lstAllDestination = new ObservableCollection<TourDestination>();
            foreach (DiaDiem item in lstDestination)
            {
                lstAllDestination.Add(new TourDestination
                {
                    Destination = item,
                    isSelected = false,
                });
                lstAllDestination[lstAllDestination.Count - 1].SelectedChanged += TourViewModel_SelectedChanged;
            }

            //Load lại toàn bộ bảng
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstTour = new ObservableCollection<Tour>(DataProvider.Ins.Entities.Tours);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable(); //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) => //Đây là đoạn xử lý khi button được nhấn Các command sau tương tự
            {
                addTour();
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedTour != null;
            }, (p) =>
            {
                int index = lstTour.IndexOf(SelectedTour);

                Tour Tour = DataProvider.Ins.Entities.Tours.Where(w => w.MaTour == SelectedTour.MaTour).FirstOrDefault();
                Tour.TenTour = Name;
                Tour.GiaTour = Convert.ToDecimal(Price);
                Tour.LoaiTour = SelectedTourType;

                List<DSDiaDiem> lstDiaDiem = new List<DSDiaDiem>(DataProvider.Ins.Entities
                    .DSDiaDiems.Where(x => x.MaTour == SelectedTour.MaTour));
                foreach (DSDiaDiem item in lstDiaDiem)
                {
                    var dd = lstPickedDestination.Where(x => x.MaDD == item.MaDD).FirstOrDefault(); //item co con nam trong danh sach dia diem duoc chon hay khong
                    if (dd == null) //item khong con nam trong danh sach dia diem cua tour nua
                        DataProvider.Ins.Entities.DSDiaDiems.Remove(item);
                }

                foreach (DiaDiem item in lstPickedDestination)
                {
                    var dd = lstDiaDiem.Where(x => x.MaDD == item.MaDD).FirstOrDefault(); //Xem dia diem duoc chon da nam trong danh sach dia diem ban dau hay khong
                    if (dd == null) //Neu chua co thi them vao
                        DataProvider.Ins.Entities.DSDiaDiems.Add(new DSDiaDiem
                        {
                            MaDD = item.MaDD,
                            MaTour = Tour.MaTour
                        });
                }

                DataProvider.Ins.Entities.SaveChanges();

                lstTour[index] = new Tour()
                {
                    MaTour = Tour.MaTour,
                    TenTour = Name,
                    GiaTour = Convert.ToDecimal(Price),
                    LoaiTour = SelectedTourType,
                };
                SelectedTour = lstTour[index];
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTour);
                view.Filter = TourFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedTour != null;
            }, (p) =>
            {
                //Hoi lai cho chac
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                deleteTour();
            });
        }

        public bool addTour()
        {
            try
            {
                Tour Tour = new Tour()
                {
                    TenTour = Name,
                    GiaTour = Convert.ToDecimal(Price),
                    LoaiTour = SelectedTourType
                };

                DataProvider.Ins.Entities.Tours.Add(Tour);

                foreach (DiaDiem item in lstPickedDestination)
                {
                    DataProvider.Ins.Entities.DSDiaDiems.Add(new DSDiaDiem
                    {
                        MaDD = item.MaDD,
                        MaTour = Tour.MaTour
                    });
                }

                DataProvider.Ins.Entities.SaveChanges();

                lstTour.Add(Tour);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteTour()
        {
            try
            {
                //Danh sách các đoàn du lịch thuộc tour
                List<DoanDuLich> lstGroup = new List<DoanDuLich>(DataProvider.Ins.Entities
                    .DoanDuLiches.Where(x => x.MaTour == SelectedTour.MaTour).ToList());

                foreach (DoanDuLich doan in lstGroup)
                {
                    //Xoa danh sach khach san cua doan do
                    List<DSKhachSan> lstHotel = new List<DSKhachSan>(DataProvider.Ins.Entities
                        .DSKhachSans.Where(x => x.MaDoan == doan.MaDoan));
                    foreach (DSKhachSan hotel in lstHotel)
                    {
                        DataProvider.Ins.Entities.DSKhachSans.Remove(hotel);
                    }

                    //Xoa danh sach phuong tien cua doan do
                    List<DSPhuongTien> lstPhuongTien = new List<DSPhuongTien>(DataProvider.Ins.Entities
                        .DSPhuongTiens.Where(x => x.MaDoan == doan.MaDoan));
                    foreach (DSPhuongTien phuongTien in lstPhuongTien)
                    {
                        DataProvider.Ins.Entities.DSPhuongTiens.Remove(phuongTien);
                    }

                    //Xoa danh sach nhan vien cua doan do
                    List<DSNhanVien> lstNhanVien = new List<DSNhanVien>(DataProvider.Ins.Entities
                        .DSNhanViens.Where(x => x.MaDoan == doan.MaDoan));
                    foreach (DSNhanVien nhanvien in lstNhanVien)
                    {
                        DataProvider.Ins.Entities.DSNhanViens.Remove(nhanvien);
                    }

                    //Xoa danh sach khach hang cua doan do
                    List<KhachDuLich> lstKhachHang = new List<KhachDuLich>(DataProvider.Ins.Entities
                        .KhachDuLiches.Where(x => x.MaDoan == doan.MaDoan));
                    foreach (KhachDuLich khachHang in lstKhachHang)
                    {
                        DataProvider.Ins.Entities.KhachDuLiches.Remove(khachHang);
                    }

                    DataProvider.Ins.Entities.DoanDuLiches.Remove(doan);
                }

                //Xoa danh sach dia diem cua tour dinh xoa
                List<DSDiaDiem> lstDiaDiem = new List<DSDiaDiem>(DataProvider.Ins.Entities
                    .DSDiaDiems.Where(x => x.MaTour == SelectedTour.MaTour).ToList());

                foreach (DSDiaDiem diaDiem in lstDiaDiem)
                {
                    DataProvider.Ins.Entities.DSDiaDiems.Remove(diaDiem);
                }

                Tour tour = DataProvider.Ins.Entities.Tours.Where(x => x.MaTour == SelectedTour.MaTour).FirstOrDefault();
                DataProvider.Ins.Entities.Tours.Remove(tour);
                DataProvider.Ins.Entities.SaveChanges();

                lstTour.Remove(SelectedTour);
                SelectedTour = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void TourViewModel_SelectedChanged(object sender, DestinationArgs e)
        {
            TourDestination destination = sender as TourDestination;

            if (destination.isSelected)
            {
                if (!lstPickedDestination.Contains(destination.Destination))
                    lstPickedDestination.Add(destination.Destination);
            }
            else lstPickedDestination.Remove(destination.Destination);
        }

        private void getDestination()
        {
            List<DSDiaDiem> lstTourDestination = new List<DSDiaDiem>(DataProvider.Ins.Entities
                .DSDiaDiems.Where(x => x.MaTour == SelectedTour.MaTour));

            lstPickedDestination.Clear();
            foreach (TourDestination item in lstAllDestination)
            {
                var check = lstTourDestination.Where(x => x.MaDD == item.Destination.MaDD).FirstOrDefault();

                if (check != null)
                {
                    item.isSelected = true;
                    if (!lstPickedDestination.Contains(item.Destination))
                        lstPickedDestination.Add(item.Destination);
                }
                else item.isSelected = false;
            }
        }

        private bool TourFilter(object item)
        {
            Tour Tour = item as Tour;
            if (filterName(Tour) && filterPrice(Tour) && filterTourType(Tour))
                return true;

            return false;
        }

        #region Filter

        private bool filterName(Tour Tour)
        {
            if (string.IsNullOrEmpty(Name) || Tour.TenTour.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterPrice(Tour Tour)
        {
            if (string.IsNullOrEmpty(Price) || Tour.GiaTour == Convert.ToDecimal(Price))
            {
                return true;
            }
            return false;
        }

        private bool filterTourType(Tour Tour)
        {
            if (SelectedTourType == null || Tour.LoaiTour == SelectedTourType)
            {
                return true;
            }

            return false;
        }
        #endregion


        private bool isCommandEnable() //Đúng chỉ khi những thông tin cần thiết đã được nhập
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Price)
                || SelectedTourType == null || SelectedTourType.TenLoaiTour == "Null"
                || lstPickedDestination.Count == 0)
            {
                return false;
            }

            return true;
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
    }
}
