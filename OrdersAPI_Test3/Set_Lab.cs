using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI_Test3
{
    [TestClass]
    public class Set_Lab : ICluster
    {
        [TestMethod]
        public void SetCluster()
        {
            ConfigurationManager.AppSettings["BaseUri"] = "http://localhost:33991/";
            ConfigurationManager.AppSettings["UserName"] = "user01@gmail.com";
            ConfigurationManager.AppSettings["Password"] = "Password1.";
        }
    }
}
