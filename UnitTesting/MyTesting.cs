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
        [Test]
        public void Login_Test()
        {
            Tour_management.ViewModel.LoginViewModel login = new Tour_management.ViewModel.LoginViewModel();
            login.UserName = "19522003@gm.uit.edu.vn";
            login.Password = "123";
            Assert.True(login.Login());
        }
    }
}
