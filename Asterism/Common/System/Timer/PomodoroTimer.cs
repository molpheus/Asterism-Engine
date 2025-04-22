using System;

namespace Asterism.System.Timer
{
    public class PomodoroTimer(TimeSpan _workTime, TimeSpan _restingTime, TimeSpan _longRestingTime, int _repeat)
    {

        public enum PomodoroState
        {
            NotStarted,
            Working,
            Resting,
            LongResting,
            Pause,
        }

        /// <summary>
        /// 現在の状態
        /// </summary>
        public PomodoroState State { get; private set; } = PomodoroState.NotStarted;

        /// <summary>
        /// 作業時間
        /// </summary>
        public TimeSpan WorkDuration => _workTime;
        /// <summary>
        /// 休憩時間
        /// </summary>
        public TimeSpan RestingDuration => _restingTime;
        /// <summary>
        /// 長休憩時間
        /// </summary>
        public TimeSpan LongRestingDuration => _longRestingTime;

        /// <summary>
        /// ポモドーロ回数
        /// </summary>
        public int PomodoroCount => _repeat;

        public int CurrentPomodoroCount { get; private set; } = 0;

        /// <summary>
        /// 開始時間
        /// </summary>
        private DateTime _playDatetime = default;

        /// <summary>
        /// 稼働中かどうか
        /// </summary>
        private bool _isPlaying = false;

        public void Start(DateTime now, bool isPlaying)
        {
            CurrentPomodoroCount = 0;
            State = PomodoroState.Working;
            if (isPlaying) Play(now);
        }

        public void Stop()
        {
            State = PomodoroState.NotStarted;
            _isPlaying = false;
        }

        public void Play(DateTime now)
        {
            _playDatetime = now;
            _isPlaying = true;
        }

        public TimeSpan? Update(DateTime now, out float progress)
        {
            progress = 0;
            if (!_isPlaying) return null;

            TimeSpan elapsed = now - _playDatetime;

            switch (State)
            {
                case PomodoroState.Working:
                progress = (float)elapsed.TotalSeconds / (float)_workTime.TotalSeconds;
                if (elapsed >= _workTime)
                {
                    CurrentPomodoroCount++;
                    if (CurrentPomodoroCount % PomodoroCount == 0)
                    {
                        State = PomodoroState.LongResting;
                    }
                    else
                    {
                        State = PomodoroState.Resting;
                    }
                    _isPlaying = false;
                }
                break;

                case PomodoroState.Resting:
                progress = (float)elapsed.TotalSeconds / (float)_restingTime.TotalSeconds;
                if (elapsed >= _restingTime)
                {
                    State = PomodoroState.Working;
                    _isPlaying = false;
                }
                break;

                case PomodoroState.LongResting:
                progress = (float)elapsed.TotalSeconds / (float)_longRestingTime.TotalSeconds;
                if (elapsed >= _longRestingTime)
                {
                    State = PomodoroState.Working;
                    _isPlaying = false;
                }
                break;
            }

            return elapsed;
        }
    }
}
