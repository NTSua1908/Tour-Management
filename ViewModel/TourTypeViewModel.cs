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
    class TourTypeViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<LoaiTour> _lstTourType;
        public ObservableCollection<LoaiTour> lstTourType { get { return _lstTourType; } set { _lstTourType = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        private string _Coefficient;
        public string Coefficient { get { return _Coefficient; } set { _Coefficient = value; OnPropertyChanged(); } }

        private LoaiTour _SelectedType;
        public LoaiTour SelectedType
        {
            get { return _SelectedType; }
            set
            {
                _SelectedType = value;
                OnPropertyChanged();
                if (SelectedType != null)
                {
                    Name = SelectedType.TenLoaiTour;
                    Coefficient = SelectedType.HeSo.ToString();
                }
            }
        }

        public TourTypeViewModel()
        {
            lstTourType = new ObservableCollection<LoaiTour>(DataProvider.Ins.Entities.LoaiTours);

            //Load lại toàn bộ bảng
            ReloadCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                lstTourType = new ObservableCollection<LoaiTour>(DataProvider.Ins.Entities.LoaiTours);
            });

            AddCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable(); //Điều kiện để button enable (return true => button enable và ngược lại)
            }, (p) => //Đây là đoạn xử lý khi button được nhấn Các command sau tương tự
            {
                LoaiTour loai = new LoaiTour()
                {
                    TenLoaiTour = Name,
                    HeSo = Convert.ToInt32(Coefficient)
                };

                DataProvider.Ins.Entities.LoaiTours.Add(loai);
                DataProvider.Ins.Entities.SaveChanges();

                lstTourType.Add(loai);
            });

            EditCommand = new RelayCommand<Window>((p) =>
            {
                return isCommandEnable() && SelectedType != null;
            }, (p) =>
            {
                int index = lstTourType.IndexOf(SelectedType);

                LoaiTour type = DataProvider.Ins.Entities.LoaiTours.Where(w => w.MaLoaiTour == SelectedType.MaLoaiTour).FirstOrDefault();
                type.TenLoaiTour = Name;
                type.HeSo = Convert.ToInt32(Coefficient);
                DataProvider.Ins.Entities.SaveChanges();

                lstTourType[index] = new LoaiTour()
                {
                    MaLoaiTour = type.MaLoaiTour,
                    TenLoaiTour = Name,
                    HeSo = Convert.ToInt32(Coefficient)
                };
                SelectedType = lstTourType[index];
            });

            SearchCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstTourType);
                view.Filter = TourTypeFilter;
            });

            DeleteCommand = new RelayCommand<Window>((p) =>
            {
                return SelectedType != null;
            }, (p) =>
            {
                //Hoi lai cho chac
                MessageBoxResult Result = MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.No)
                    return;

                List<Tour> lstTour = new List<Tour>(DataProvider.Ins.Entities
                    .Tours.Where(x => x.MaLoaiTour == SelectedType.MaLoaiTour).ToList());

                foreach (Tour item in lstTour)
                {
                    //Danh sách các đoàn du lịch thuộc tour có mang tour định xóa
                    List<DoanDuLich> lstGroup = new List<DoanDuLich>(DataProvider.Ins.Entities
                        .DoanDuLiches.Where(x => x.MaTour == item.MaTour).ToList());

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
                        .DSDiaDiems.Where(x => x.MaTour == item.MaTour).ToList());

                    foreach (DSDiaDiem diaDiem in lstDiaDiem)
                    {
                        DataProvider.Ins.Entities.DSDiaDiems.Remove(diaDiem);
                    }

                    DataProvider.Ins.Entities.Tours.Remove(item);
                }

                LoaiTour loai = DataProvider.Ins.Entities.LoaiTours.Where(x => x.MaLoaiTour == SelectedType.MaLoaiTour).FirstOrDefault();

                DataProvider.Ins.Entities.LoaiTours.Remove(loai);
                DataProvider.Ins.Entities.SaveChanges();

                lstTourType.Remove(SelectedType);
                SelectedType = null;

            });
        }

        private bool TourTypeFilter(object item)
        {
            LoaiTour loai = item as LoaiTour;
            if (filterName(loai) && filterCoefficient(loai))
                return true;

            return false;
        }

        #region Filter

        private bool filterName(LoaiTour loai)
        {
            if (string.IsNullOrEmpty(Name) || loai.TenLoaiTour.ToLower().Contains(Name.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool filterCoefficient(LoaiTour loai)
        {
            if (string.IsNullOrEmpty(Coefficient) || loai.HeSo == Convert.ToDouble(Coefficient))
            {
                return true;
            }
            return false;
        }

        #endregion


        private bool isCommandEnable() //Đúng chỉ khi những thông tin cần thiết đã được nhập
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Coefficient))
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
