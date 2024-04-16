using System;
using System.Collections.Generic;
using System.IO;

using Asterism.Common;
using Asterism.Common.Extension;

namespace Asterism.System.Reminder
{
    public partial class Reminder
    {
        public const string FileName = "reminder.dat";
        protected List<RemindData> _remindList = new List<RemindData>();
        public int Count => _remindList.Count;
        public RemindData this[int index] => _remindList[index];
    }


    public partial class Reminder
    {        
        protected string _retentionPath;

        public Reminder()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            _retentionPath = Path.Combine(currentDirectory, FileName);
        }

        public Reminder(string retentionDirectory)
        {
            _retentionPath = Path.Combine(retentionDirectory, FileName);
        }
    }


    public partial class Reminder
    {
        public bool Add(DateTime time, string message) => Add(new RemindData(time, message));
        public bool Add(RemindData remindData) => AddList([remindData]);
        public bool AddList(params RemindData[] remindData) => _remindList.TryAdd(remindData);
        public void Get(ref RemindData remindData) => _remindList.TryGetOrAdd(0, ref remindData);
        public bool Remove(DateTime time) => _remindList.RemoveAll(x => x.Time == time) is not 0;
        public void RemoveAll() => _remindList.Clear();

        public void Update()
        {
            var now = DateTime.Now;
            var removeList = new List<RemindData>();
            foreach (var remind in _remindList)
            {
                if (now >= remind.Time)
                {
                    _observers.ForEach(x => x.OnNext(remind));
                    removeList.Add(remind);
                }
            }

            removeList.ForEach(x => _remindList.Remove(x));
        }
    }

    public partial class Reminder : IFileSave
    {
        public void Save() => this.Save(_retentionPath, _remindList);
        public void Load() => _remindList = this.Load<List<RemindData>>(_retentionPath);
        public bool CheckFile() => File.Exists(_retentionPath);
        public void DeleteFile() => File.Delete(_retentionPath);
    }

    public partial class Reminder : IObservable<RemindData>
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
}
