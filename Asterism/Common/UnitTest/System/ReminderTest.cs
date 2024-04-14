using System;

using Asterism.System.Reminder;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.System
{
    [TestClass]
    public class ReminderTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var reminder = new Reminder();
            reminder.Add(DateTime.Now, "Test");
            Assert.AreEqual(1, reminder.Count);
        }

        [TestMethod]
        public void TestRemoveDatetime()
        {
            var reminder = new Reminder();
            var currentTime = DateTime.Now;
            reminder.Add(currentTime, "Test");
            reminder.Add(currentTime.AddMinutes(1), "Test");
            reminder.Add(currentTime.AddMinutes(2), "Test");
            reminder.Remove(currentTime);
            Assert.AreEqual(2, reminder.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var reminder = new Reminder();
            reminder.Add(DateTime.Now.AddSeconds(-1), "Test");
            reminder.Update();
            Assert.AreEqual(0, reminder.Count);
        }

        [TestMethod]
        public void TestSave()
        {
            var reminder = new Reminder();
            reminder.DeleteFile();
            reminder.Add(DateTime.Now, "Test");
            reminder.Save();
            Assert.AreEqual(true, reminder.CheckFile());
        }

        [TestMethod]
        public void TestLoad()
        {
            var reminder = new Reminder();
            reminder.DeleteFile();
            reminder.Add(DateTime.Now, "Test");
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
            reminder.Add(currentTime, "Test");
            var isUpdated = false;
            var disposable = reminder.Subscribe(_ => { isUpdated = true; });
            reminder.Update();
            disposable.Dispose();
            Assert.AreEqual(true, isUpdated);
        }
    }
}
