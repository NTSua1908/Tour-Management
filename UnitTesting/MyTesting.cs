using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tour_management;
namespace UnitTesting
{
    public class MyTesting
    {
        //Test tài khoản staff
        [Test]
        public void Login_Test0()
        {
            Tour_management.ViewModel.LoginViewModel login = new Tour_management.ViewModel.LoginViewModel();
            login.UserName = "19522003@gm.uit.edu.vn";
            login.Password = "123";
            Assert.True(login.Login());
        }
        //Test tài khoản staff trường hợp sai mật khẩu
        [Test]
        public void Login_Test1()
        {
            Tour_management.ViewModel.LoginViewModel login = new Tour_management.ViewModel.LoginViewModel();
            login.UserName = "19522003@gm.uit.edu.vn";
            login.Password = "321";
            Assert.True(login.Login());
        }
        //Test tài khoản admin
        [Test]
        public void Login_Test2()
        {
            Tour_management.ViewModel.LoginViewModel login = new Tour_management.ViewModel.LoginViewModel();
            login.UserName = "admin";
            login.Password = "admin";
            Assert.True(login.Login());
        }
        //Test nút thêm khách hàng, vào CustomerManagement để kiểm tra thông tin đã được thêm
        [Test]
        public void Add_Customer0()
        {
            Tour_management.ViewModel.CustomerViewModel customer = new Tour_management.ViewModel.CustomerViewModel();
            customer.Name = "Thanh Phat";
            customer.Address = "HCM";
            customer.SelectedGender = "Nam";
            customer.CMND = "1234";
            customer.Age = "32";
            customer.Phone = "09098393";
            customer.SelectedCustomerType = customer.lstCustomerType[0];
            Assert.True(customer.addCustomer());
        }
        //Test nút thêm khách hàng, với loại customer là : Foreign
        [Test]
        public void Add_Customer1()
        {
            Tour_management.ViewModel.CustomerViewModel customer = new Tour_management.ViewModel.CustomerViewModel();
            customer.Name = "Thanh";
            customer.Address = "HCM";
            customer.SelectedGender = "Nữ";
            customer.CMND = "4321";
            customer.Age = "20";
            customer.Phone = "09098393";
            customer.SelectedCustomerType = customer.lstCustomerType[1];
            customer.Visa = Convert.ToDateTime("2 / 10 / 2021");
            customer.Passport = Convert.ToDateTime("10 / 10 / 2021");
            Assert.True(customer.addCustomer());
        }
        //Test nút xóa khách hàng, vào CustomerManagement để kiểm tra thông tin đã được xóa
        [Test]
        public void Delete_Customer0()
        {
            Tour_management.ViewModel.CustomerViewModel customer = new Tour_management.ViewModel.CustomerViewModel();
            if (customer.lstCustomer != null)
            {
                customer.SelectedItem = customer.lstCustomer[5];
                Assert.IsTrue(customer.deleteCustomer());
            }
            else
            {
                Assert.IsTrue(false);
            }    
        }
        //Test nút xóa khách hàng ở vị trí không có khách hàng nào, vào CustomerManagement để kiểm tra thông tin đã được xóa
        [Test]
        public void Delete_Customer1()
        {
            Tour_management.ViewModel.CustomerViewModel customer = new Tour_management.ViewModel.CustomerViewModel();
            if (customer.lstCustomer != null)
            {
                customer.SelectedItem = customer.lstCustomer[10];
                Assert.IsTrue(customer.deleteCustomer());
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        public void Add_Tour0()
        {
            Tour_management.ViewModel.TourViewModel tour = new Tour_management.ViewModel.TourViewModel();
            tour.Name = "H21";
            tour.SelectedTourType = tour.lstTourType[0];
            tour.Price = "10000000";
            Assert.True(tour.addTour());
        }
        [Test]
        public void Delete_Tour0()
        {
            Tour_management.ViewModel.TourViewModel tour = new Tour_management.ViewModel.TourViewModel();
            if (tour.lstTour != null)
            {
                tour.SelectedTour = tour.lstTour[0];
                Assert.IsTrue(tour.deleteTour());
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        //Test nút xóa tour ở vị trí không có tour nào
        [Test]
        public void Delete_Tour1()
        {
            Tour_management.ViewModel.TourViewModel tour = new Tour_management.ViewModel.TourViewModel();
            if (tour.lstTour != null)
            {
                tour.SelectedTour = tour.lstTour[10];
                Assert.IsTrue(tour.deleteTour());
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        //Test add user loại 1
        [Test]
        public void Create_User0()
        {
            Tour_management.ViewModel.RegisterAccountViewModel user = new Tour_management.ViewModel.RegisterAccountViewModel();
            user.DisplayName = "Phat";
            user.CMND = "1234";
            user.AvatarIndex = 1;
            user.Age = "32";
            user.Phone = "0390913013";
            user.UserName = "phat";
            user.Password = "1234";
            user.RePassword = "1234";
            user.SelectedUserType = user.lstUserType[0];
            Assert.IsTrue(user.addUser());
        }
        //Test add user loại 2
        [Test]
        public void Create_User1()
        {
            Tour_management.ViewModel.RegisterAccountViewModel user = new Tour_management.ViewModel.RegisterAccountViewModel();
            user.DisplayName = "Phat";
            user.CMND = "1234";
            user.AvatarIndex = 1;
            user.Age = "32";
            user.Phone = "0390913013";
            user.UserName = "phat";
            user.Password = "1234";
            user.RePassword = "1234";
            user.SelectedUserType = user.lstUserType[1];
            Assert.IsTrue(user.addUser());
        }
        //Test delete user
        [Test]
        public void Delete_User0()
        {
            Tour_management.ViewModel.UserManagementViewModel user = new Tour_management.ViewModel.UserManagementViewModel();
            if (user.lstUser!= null)
            {
                user.SelectedUser= user.lstUser[5];
                Assert.IsTrue(user.deleteUser());
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        //Test delete user ko có trong list
        [Test]
        public void Delete_User1()
        {
            Tour_management.ViewModel.UserManagementViewModel user = new Tour_management.ViewModel.UserManagementViewModel();
            if (user.lstUser != null)
            {
                user.SelectedUser = user.lstUser[15];
                Assert.IsTrue(user.deleteUser());
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
    }
}
