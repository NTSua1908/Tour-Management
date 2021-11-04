using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tour_management.ViewModel;

namespace Tour_management.Model
{
    public class TourDestination : BaseViewModel //Nhan lai ham OnpropertyChanged
    {
        private bool _isSelected;
        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                if (selectedChanged != null)
                {
                    selectedChanged(this, new DestinationArgs(isSelected, Destination));
                }
            }
        }

        private DiaDiem _Destination;
        public DiaDiem Destination
        {
            get { return _Destination; }
            set { _Destination = value; OnPropertyChanged(); }
        }

        private event EventHandler<DestinationArgs> selectedChanged;
        public event EventHandler<DestinationArgs> SelectedChanged
        {
            add { selectedChanged += value; }
            remove { selectedChanged -= value; }
        }

        //public TourDestination(bool isSeleted, DiaDiem destination)
        //{
        //    this.isSelected = isSelected;
        //    this.Destination = destination;
        //}

        //public bool isSelected { get; set; }
        //public DiaDiem Destination { get; set; }
    }

    public class DestinationArgs : EventArgs
    {
        public bool isChecked { get; set; }
        public DiaDiem Destination { get; set; }

        public DestinationArgs(bool isChecked, DiaDiem Destination)
        {
            this.isChecked = isChecked;
            this.Destination = Destination;
        }
    }
}
