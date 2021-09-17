﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tour_management.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }

        public ICommand AreaCommand { get; set; }
        public ICommand DestinationCommand { get; set; }
        public ICommand VehicleCommand { get; set; }
        public ICommand HotelCommand { get; set; }
        public ICommand InformationCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand GroupCommand { get; set; }
        public ICommand TourCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand JoinGroupCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand ManageStaffCommand { get; set; }
        public ICommand ManageUserCommand { get; set; }
        public ICommand TourTypeCommand { get; set; }
        public ICommand AddUserCommand { get; set; }
        public ICommand AddGroupCommand { get; set; }

        public MainViewModel()
        {
            //Goi ham nay de thuc hien dang nhap
            LoadedCommand = new RelayCommand<Window>((p) => { return true; },  (p) => { Login(p); } );

            AreaCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                AreaManagement area = new AreaManagement();
                area.ShowDialog();
            });
            
            DestinationCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                DestinationManagement destination = new DestinationManagement();
                destination.ShowDialog();
            });

            VehicleCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                VehicleManagement vehicle = new VehicleManagement();
                vehicle.ShowDialog();
            });

            HotelCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                HotelManagement hotel = new HotelManagement();
                hotel.ShowDialog();
            });

            InformationCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                PersonalInformation information = new PersonalInformation();
                information.ShowDialog();
            });

            TourCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                TourWindow tour = new TourWindow();
                tour.ShowDialog();
            });

            CustomerCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                CustomerManagement customer = new CustomerManagement();
                customer.ShowDialog();
            });

            ManageStaffCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                StaffManagement staff = new StaffManagement();
                staff.ShowDialog();
            });

            ManageUserCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                UserManagement user = new UserManagement();
                user.ShowDialog();
            });
            
            TourTypeCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                TourType type = new TourType();
                type.ShowDialog();
            });
            
            AddUserCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                RegisterAccount register = new RegisterAccount();
                register.ShowDialog();
            });
        }

        void Login(Window w)
        {
            w.Hide();
            LoginWindow login = new LoginWindow();
            LoginViewModel loginViewModel = (LoginViewModel)login.DataContext;
            login.ShowDialog();

            if (loginViewModel.isLogin)
            {
                w.Show();
            }
            else w.Close();

        }
    }
}