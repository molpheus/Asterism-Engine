using System;

using Asterism.System.Timer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.System
{
    [TestClass]
    public class PomodoroTest
    {
        private PomodoroTimer _pomodoroTimer;

        [TestInitialize]
        public void Setup()
        {
            // 作業時間: 25分, 休憩時間: 5分, 長休憩時間: 15分, ポモドーロ回数: 4
            _pomodoroTimer = new PomodoroTimer(
                TimeSpan.FromMinutes(25),
                TimeSpan.FromMinutes(5),
                TimeSpan.FromMinutes(15),
                4
            );
        }

        [TestMethod]
        public void TestStart()
        {
            // Arrange
            DateTime now = DateTime.Now;

            // Act
            _pomodoroTimer.Start(now, true);

            // Assert
            Assert.AreEqual(PomodoroTimer.PomodoroState.Working, _pomodoroTimer.State);
            Assert.AreEqual(0, _pomodoroTimer.CurrentPomodoroCount);
        }

        [TestMethod]
        public void TestStop()
        {
            // Arrange
            DateTime now = DateTime.Now;
            _pomodoroTimer.Start(now, true);

            // Act
            _pomodoroTimer.Stop();

            // Assert
            Assert.AreEqual(PomodoroTimer.PomodoroState.NotStarted, _pomodoroTimer.State);
        }

        [TestMethod]
        public void TestUpdate_WorkingToResting()
        {
            // Arrange
            DateTime now = DateTime.Now;
            _pomodoroTimer.Start(now, true);

            // Act
            TimeSpan elapsed = _pomodoroTimer.Update(now + TimeSpan.FromMinutes(25), out var _).Value;

            // Assert
            Assert.AreEqual(PomodoroTimer.PomodoroState.Resting, _pomodoroTimer.State);
            Assert.AreEqual(1, _pomodoroTimer.CurrentPomodoroCount);
            Assert.AreEqual(TimeSpan.FromMinutes(25), elapsed);
        }

        [TestMethod]
        public void TestUpdate_RestingToWorking()
        {
            // Arrange
            DateTime now = DateTime.Now;
            _pomodoroTimer.Start(now, true);
            _pomodoroTimer.Update(now + TimeSpan.FromMinutes(25), out var _); // Working -> Resting

            _pomodoroTimer.Play(now + TimeSpan.FromMinutes(25));

            // Act
            TimeSpan elapsed = _pomodoroTimer.Update(now + TimeSpan.FromMinutes(30), out var _).Value; // Resting -> Working

            // Assert
            Assert.AreEqual(PomodoroTimer.PomodoroState.Working, _pomodoroTimer.State);
            Assert.AreEqual(TimeSpan.FromMinutes(5), elapsed);
        }

        [TestMethod]
        public void TestUpdate_WorkingToLongResting()
        {
            // Arrange
            DateTime now = DateTime.Now;
            _pomodoroTimer.Start(now, true);

            // 1
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // 2
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // 3
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // 4
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);

            // Assert
            Assert.AreEqual(PomodoroTimer.PomodoroState.LongResting, _pomodoroTimer.State);
            Assert.AreEqual(4, _pomodoroTimer.CurrentPomodoroCount);
        }

        [TestMethod]
        public void TestUpdate_LongRestingToWorking()
        {
            // Arrange
            DateTime now = DateTime.Now;
            _pomodoroTimer.Start(now, true);

            // 1
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // 2
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // 3
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // 4
            now += TimeSpan.FromMinutes(25);
            _pomodoroTimer.Update(now, out var _); // Working -> Resting
            _pomodoroTimer.Play(now);
            now += TimeSpan.FromMinutes(5);
            _pomodoroTimer.Update(now, out var _); // Resting -> Working
            _pomodoroTimer.Play(now);
            // LongResting
            now += TimeSpan.FromMinutes(15);

            // Act
            TimeSpan elapsed = _pomodoroTimer.Update(now, out var _).Value; // LongResting -> Working

            // Assert
            Assert.AreEqual(PomodoroTimer.PomodoroState.Working, _pomodoroTimer.State);
            Assert.AreEqual(TimeSpan.FromMinutes(15), elapsed);
        }
    }
}
