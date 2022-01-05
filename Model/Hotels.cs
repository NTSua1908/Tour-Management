using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_management.ViewModel;

namespace Tour_management.Model
{
    class Hotels:BaseViewModel
    {
        private int _noDay;
        public int noDay
        {
            get { return _noDay; }
            set { _noDay = value; OnPropertyChanged(); }
        }

        private int _noNight;
        public int noNight
        {
            get { return _noNight; }
            set { _noNight = value; OnPropertyChanged(); }
        }

        private KhachSan _Hotel;
        public KhachSan Hotel
        {
            get { return _Hotel; }
            set { _Hotel = value; OnPropertyChanged(); }
        }

        public class HotelsArgs : EventArgs
        {
            public int noDay { get; set; }

            public int noNight { get; set; }
            public KhachSan Hotel { get; set; }

            public HotelsArgs(int noDay, int noNight, KhachSan Hotel)
            {
                this.noDay = noDay;
                this.noNight = noNight;
                this.Hotel = Hotel;
            }
        }
    }
}
