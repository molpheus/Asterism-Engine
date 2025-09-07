using System;
using System.Collections.Generic;

using Asterism.System.Cron;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.System
{
    [TestClass]
    public class CronTest
    {
        private CronSchedule _cronSchedule;

        [TestInitialize]
        public void Setup()
        {
            _cronSchedule = new CronSchedule();
        }

        [TestMethod]
        public void TestAdd_追加1()
        {
            _cronSchedule.Add("0 0 * * *");
            Assert.AreEqual(1, _cronSchedule.Count);
        }

        [TestMethod]
        public void TestAdd_追加2()
        {
            _cronSchedule.Add(new CronExpression("0 0 * * *"));
            Assert.AreEqual(1, _cronSchedule.Count);
        }

        [TestMethod]
        public void TestAdd_複数追加()
        {
            _cronSchedule.Add(
                new List<CronExpression>
                {
                    new("0 0 * * *"),
                    new("0 1 * * *"),
                    new("0 2 * * *")
                }
            );

            Assert.AreEqual(3, _cronSchedule.Count);
        }

        [TestMethod]
        public void TestAdd_InvalidFormat_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _cronSchedule.Add("invalid format"));
        }

        [TestMethod]
        public void TestAdd_Nullが代入された()
        {
            Assert.Throws<ArgumentException>(() => _cronSchedule.Add(""));
        }

        [TestMethod]
        public void TestRemove_削除()
        {
            _cronSchedule.Add("0 0 * * *");
            _cronSchedule.Remove("0 0 * * *");
            Assert.AreEqual(0, _cronSchedule.Count);
        }

        [TestMethod]
        public void TestRemoveAt_削除()
        {
            _cronSchedule.Add("0 0 * * *");
            _cronSchedule.RemoveAt(0);
            Assert.AreEqual(0, _cronSchedule.Count);
        }

        [TestMethod]
        public void TestRemoveAll_全削除()
        {
            _cronSchedule.Add("0 0 * * *");
            _cronSchedule.Add("0 1 * * *");
            _cronSchedule.RemoveAll();
            Assert.AreEqual(0, _cronSchedule.Count);
        }

        [TestMethod]
        public void TestGet_取得()
        {
            _cronSchedule.Add("0 0 * * *");
            var result = _cronSchedule.Get(0, out var cron);
            Assert.IsTrue(result);
            Assert.IsNotNull(cron);
        }

        [TestMethod]
        public void TestUpdate_通知()
        {
            var cronExpression = new CronExpression("0 0 * * *");
            _cronSchedule.Add(cronExpression);
            var observer = new TestObserver();
            _cronSchedule.Subscribe(observer);

            _cronSchedule.Update(new DateTime(2023, 10, 10, 0, 0, 0));

            Assert.HasCount(1, observer.NotifiedExpressions);
            Assert.AreEqual(cronExpression, observer.NotifiedExpressions[0]);
        }

        private class TestObserver : IObserver<CronExpression>
        {
            public List<CronExpression> NotifiedExpressions { get; } = new List<CronExpression>();

            public void OnCompleted() { }

            public void OnError(Exception error) { }

            public void OnNext(CronExpression value)
            {
                NotifiedExpressions.Add(value);
            }
        }
    }
}
