using System;

namespace Asterism.System.Timer
{
    public class PomodoroTimer(TimeSpan _workTime, TimeSpan _breakTime, int _repeat)
    {

        public enum PomodoroState
        {
            NotStarted,
            Working,
            OnBreak,
            Pause,
        }

        public PomodoroState State { get; private set; } = PomodoroState.NotStarted;

        public TimeSpan WorkDuration => _workTime;
        public TimeSpan BreakDuration => _breakTime;

        public int RepeatCount => _repeat;
        public int CurrentRepeatCount { get; private set; } = 0;

        public bool isRunning => State != PomodoroState.NotStarted;

        private DateTime _startTime;

        public TimeSpan NowSpan;

        private TimeSpan _remaingTime;
        private PomodoroState _remaindState;

        public void Start(DateTime now)
        {
            if (isRunning)
                return;

            _startTime = now;
            State = PomodoroState.Working;
            CurrentRepeatCount = 0;
            NowSpan = TimeSpan.Zero;
        }
        public void Stop()
        {
            if (!isRunning)
                return;

            State = PomodoroState.NotStarted;
        }

        /// <summary>
        /// Pause the Pomodoro Timer
        /// </summary>
        public void Pause(DateTime now)
        {
            if (!isRunning)
                return;

            if (State != PomodoroState.Pause)
            {
                _remaindState = State;
                State = PomodoroState.Pause;

                _remaingTime = _remaindState == PomodoroState.Working ? WorkDuration - (now - _startTime) : BreakDuration - (now - _startTime);
            }
            else
            {
                _startTime = now - (_remaindState == PomodoroState.Working ? WorkDuration - _remaingTime : BreakDuration - _remaingTime);
                State = _remaindState;
                _remaingTime = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Update Pomodoro Timer
        /// </summary>
        /// <param name="now"> current Time </param>
        public void Update(DateTime now)
        {
            if (!isRunning || State == PomodoroState.Pause)
                return;

            NowSpan = now - _startTime;

            switch (State)
            {
                case PomodoroState.Working:
                if (NowSpan >= WorkDuration)
                {
                    State = PomodoroState.OnBreak;
                    _startTime = now;
                }
                break;
                case PomodoroState.OnBreak:
                if (NowSpan >= BreakDuration)
                {
                    State = PomodoroState.Working;
                    _startTime = now;
                    CurrentRepeatCount++;

                    if (CurrentRepeatCount >= RepeatCount)
                    {
                        // Reset the timer
                        State = PomodoroState.NotStarted;
                        CurrentRepeatCount = 0;
                    }
                }
                break;
            }
        }
    }
}
