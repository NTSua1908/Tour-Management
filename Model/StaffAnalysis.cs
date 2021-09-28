using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_management.ViewModel;

namespace Tour_management.Model
{
    class StaffAnalysis : BaseViewModel
    {
        private int _Quantity;
        public int Quantity { get { return _Quantity; } set { _Quantity = value; OnPropertyChanged(); } }
        public NhanVien NhanVien { get; set; }
    }
}
