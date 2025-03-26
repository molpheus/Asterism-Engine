using System;
using System.Collections.Generic;

using Asterism.Common.Extension;
using Asterism.Common.FileManagement;

namespace Asterism.System.Reminder
{
    public partial class ReminderSchedule
    {
        protected RemindList _remindList = new();
        public int Count => _remindList.Count;

        private readonly IFileSaveSettings<RemindList> _fileHandler;

        public ReminderSchedule(string filePath = null)
        {
            _remindList = new();
            _fileHandler = new XmlFileHandler<RemindList>("reminder.xml");
        }
    }

    public partial class ReminderSchedule : IRemindList
    {
        public bool Add(DateTime time, string message) => _remindList.TryAdd(new(time, message));
        public bool Add(RemindData remindData) => _remindList.TryAdd(remindData);
        public bool AddList(params RemindData[] remindData) => _remindList.TryAddRange(remindData);
        public bool Get(int index, out RemindData remindData) => _remindList.TryGet(index, out remindData);
        public bool Remove(DateTime time) => _remindList.RemoveOlderThen(time);
        public void RemoveAll() => _remindList.Clear();
    }

    public partial class ReminderSchedule : IScheduledUpdatable
    {
        public void Update(DateTime now)
        {
            foreach (var remind in _remindList)
            {
                if (remind.Time <= now)
                {
                    _observers.ForEach(x => x.OnNext(remind));
                }
            }

            Remove(now);
        }
    }

    public partial class ReminderSchedule : IFileSave
    {
        public bool Save() => _fileHandler.Save(_remindList);
        public bool Load() => _fileHandler.Load(out _remindList);
        public bool Exists() => _fileHandler.Exists();
        public void Delete() => _fileHandler.Delete();
    }

    public partial class ReminderSchedule : IObservable<RemindData>
    {
        private List<IObserver<RemindData>> _observers = new List<IObserver<RemindData>>();

        public IDisposable Subscribe(IObserver<RemindData> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber(List<IObserver<RemindData>> observers, IObserver<RemindData> observer) : IDisposable
        {
            void IDisposable.Dispose()
            {
                if (observer != null && observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
        }
    }

    public partial class ReminderSchedule : INullable<RemindData>
    { }
}
