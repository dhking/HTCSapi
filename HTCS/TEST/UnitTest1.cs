using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using Model;
using System.Collections.Generic;
using System.Collections;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using AutoTaskService.DB;
using DBHelp;
using Newtonsoft.Json;
using System.Net.WebSockets;
using Service;

namespace TEST
{
   
    public class user
    {
        public string name { get; set; }
        public string sex { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                int a = 10;

                int b = 100;

                double percent = (double)10 / 100;

                string percentText = percent.ToString("0.0%");//最后percentText的值为10.0%
            }
            catch (Exception ex)
            {

            }
            

        }
    }
}
