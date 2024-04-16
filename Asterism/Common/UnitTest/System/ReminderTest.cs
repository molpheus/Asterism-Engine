using System;
using System.Reactive.Linq;
using Asterism.Common;
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
            var reminder = new Reminder();
            reminder.Add(DateTime.Now, "Test");
            Assert.AreEqual(1, reminder.Count);
        }

        [TestMethod]
        public void TestAdd_追加2()
        {
            var reminder = new Reminder();
            reminder.Add(new RemindData(DateTime.Now, "Test1"));
            Assert.AreEqual(1, reminder.Count);
        }

        [TestMethod]
        public void TestAdd_複数追加()
        {
            var reminder = new Reminder();
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
            var reminder = new Reminder();
            bool isResult = reminder.TryAdd(null);
            Assert.AreEqual(false, isResult);
        }

        #endregion

        [TestMethod]
        public void TestRemoveDatetime()
        {
            var reminder = new Reminder();
            var currentTime = DateTime.Now;
            reminder.TryAdd(currentTime, "Test");
            reminder.TryAdd(currentTime.AddMinutes(1), "Test");
            reminder.TryAdd(currentTime.AddMinutes(2), "Test");
            reminder.Remove(currentTime);
            Assert.AreEqual(2, reminder.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var reminder = new Reminder();
            reminder.TryAdd(DateTime.Now.AddSeconds(-1), "Test");
            reminder.Update();
            Assert.AreEqual(0, reminder.Count);
        }

        [TestMethod]
        public void TestSave()
        {
            var reminder = new Reminder();
            reminder.DeleteFile();
            reminder.TryAdd(DateTime.Now, "Test");
            reminder.Save();
            Assert.AreEqual(true, reminder.CheckFile());
        }

        [TestMethod]
        public void TestLoad()
        {
            var reminder = new Reminder();
            reminder.DeleteFile();
            reminder.TryAdd(DateTime.Now, "Test");
            reminder.Save();
            reminder.RemoveAll();

            reminder.Load();
            Assert.AreEqual(1, reminder.Count);
        }

        [TestMethod]
        public void TestDeleteFile()
        {
            var reminder = new Reminder();
            reminder.DeleteFile();
            Assert.AreEqual(false, reminder.CheckFile());
        }

        [TestMethod]
        public void TestSubscribe()
        {
            var reminder = new Reminder();
            var currentTime = DateTime.Now;
            reminder.TryAdd(currentTime, "Test");
            var isUpdated = false;
            var disposable = reminder.Subscribe(_ => { isUpdated = true; });
            reminder.Update();
            disposable.Dispose();
            Assert.AreEqual(true, isUpdated);
        }

        [TestMethod]
        public void TestRemove_削除項目なし()
        {
            var reminder = new Reminder();
            reminder.TryAdd(DateTime.Now, "Test");
            bool isResult = reminder.Remove(DateTime.Now.AddMinutes(1));
            Assert.AreEqual(false, isResult);
        }
    }
}
