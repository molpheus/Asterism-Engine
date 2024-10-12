using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asterism.System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.System
{
    [TestClass]
    public class CronTest
    {
        [TestMethod]
        public void TestAdd_追加1()
        {
            var cron = new CronSchedule();
            cron.Add("0", "0", "*", "*", "*");
            Assert.AreEqual(1, cron.Count);
        }

        [TestMethod]
        public void TestAdd_追加2()
        {
            var cron = new CronSchedule();
            cron.Add(new Cron("0", "0", "*", "*", "*"));
            Assert.AreEqual(1, cron.Count);
        }

        [TestMethod]
        public void TestAdd_複数追加()
        {
            var cron = new CronSchedule();
            cron.AddList(
                new Cron("0", "0", "*", "*", "*"),
                new Cron("0", "1", "*", "*", "*"),
                new Cron("0", "2", "*", "*", "*")
            );

            Assert.AreEqual(3, cron.Count);
        }

        [TestMethod]
        public void TestAdd_Nullが代入された()
        {
            var cron = new CronSchedule();
            cron.Add(null);
            Assert.AreEqual(0, cron.Count);
        }
    }
}
