using System;
using System.Collections.Generic;
using System.IO;

using Asterism.Common;
using Asterism.Common.Extension;

namespace Asterism.System.Reminder
{
    public partial class Reminder : IObservable<RemindData>, INullable<Reminder>, IFileSave
    {
        public const string FileName = "reminder.xml";
        protected List<RemindData> _remindList = new List<RemindData>();
        public int Count => _remindList.Count;
        public RemindData this[int index] => _remindList[index];

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

        public bool Add(DateTime time, string message) => _remindList.TryAdd(new RemindData(time, message));
        public bool Add(RemindData remindData) => _remindList.TryAdd(remindData);
        public bool AddList(params RemindData[] remindData) => _remindList.TryAdd(remindData);
        public bool Get(int index, out RemindData remindData) => _remindList.TryGet(index, out remindData);
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

        public bool Save() => this.TrySave(_retentionPath, _remindList);
        public bool Load() => this.TryLoad(_retentionPath, out _remindList);
        public bool CheckFile() => File.Exists(_retentionPath);
        public void DeleteFile() => File.Delete(_retentionPath);
    
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
