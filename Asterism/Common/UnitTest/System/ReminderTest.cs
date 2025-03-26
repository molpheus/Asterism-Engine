using System;

using Asterism.Common.Extension;
using Asterism.System.Reminder;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.System
{
    [TestClass]
    public class ReminderTest
    {
        #region Add
        [TestMethod]
        public void TestAdd_追加1()
        {
            var reminder = new ReminderSchedule();
            reminder.Add(DateTime.Now, "Test");
            Assert.AreEqual(1, reminder.Count);
        }

        [TestMethod]
        public void TestAdd_追加2()
        {
            var reminder = new ReminderSchedule();
            reminder.Add(new RemindData(DateTime.Now, "Test1"));
            Assert.AreEqual(1, reminder.Count);
        }

        [TestMethod]
        public void TestAdd_複数追加()
        {
            var reminder = new ReminderSchedule();
            reminder.AddList([
                new RemindData(DateTime.Now, "Test1"),
                new RemindData(DateTime.Now.AddMinutes(1), "Test2"),
                new RemindData(DateTime.Now.AddMinutes(2), "Test3")
            ]);

            Assert.AreEqual(3, reminder.Count);
        }

        [TestMethod]
        public void TestAdd_Nullが代入された()
        {
            var reminder = new ReminderSchedule();
            reminder.Add(null);
            Assert.AreEqual(0, reminder.Count);
        }

        [TestMethod]
        public void TestAdd_重複()
        {
            var reminder = new ReminderSchedule();
            var now = DateTime.Now;
            reminder.Add(now, "Test");
            bool isResult = reminder.Add(now, "Test");
            Assert.AreEqual(false, isResult);
        }

        #endregion

        #region Get
        [TestMethod]
        public void TestGet()
        {
            var reminder = new ReminderSchedule();
            reminder.Add(DateTime.Now, "Test");
            bool isResult = reminder.Get(0, out var remindData);
            Assert.AreEqual(true, isResult);
            Assert.AreEqual("Test", remindData.Message);
        }

        [TestMethod]
        public void TestGet_取得失敗()
        {
            var reminder = new ReminderSchedule();
            bool isResult = reminder.Get(0, out var remindData);
            Assert.AreEqual(false, isResult);
            Assert.AreEqual(true, remindData.IsNullOrDefault());
        }

        #endregion

        [TestMethod]
        public void TestRemoveDatetime()
        {
            var reminder = new ReminderSchedule();
            var currentTime = DateTime.Now;
            reminder.Add(currentTime, "Test");
            reminder.Add(currentTime.AddMinutes(1), "Test");
            reminder.Add(currentTime.AddMinutes(2), "Test");
            reminder.Remove(currentTime);
            Assert.AreEqual(2, reminder.Count);
        }

        [TestMethod]
        public void TestRemove_削除項目なし()
        {
            var reminder = new ReminderSchedule();
            reminder.Add(DateTime.Now.AddMinutes(1), "Test");
            bool isResult = reminder.Remove(DateTime.Now);
            Assert.AreEqual(false, isResult);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var reminder = new ReminderSchedule();
            reminder.Add(DateTime.Now.AddSeconds(-1), "Test");
            reminder.Update(DateTime.Now);
            Assert.AreEqual(0, reminder.Count);
        }
    }
}
