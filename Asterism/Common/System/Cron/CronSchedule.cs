using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Asterism.Common;
using Asterism.Common.Extension;

namespace Asterism.System
{
    public partial class CronSchedule : IObservable<Cron>, INullable<CronSchedule>,  IFileSave
    {
        public string FilePath { get; }
        public const string FileName = "cron.xml";

        protected List<Cron> _cronList = new List<Cron>();
        public int Count => _cronList.Count;
        private List<IObserver<Cron>> _observers = new List<IObserver<Cron>>();
        public void Update(DateTime now)
        {
            foreach (var cron in _cronList)
            {
                if (cron.IsMatch(now))
                {
                    _observers.ForEach(x => x.OnNext(cron));
                }
            }
        }

        public bool Add(string minute, string hour, string day, string month, string week) => _cronList.TryAdd(new Cron(minute, hour, day, month, week));
        public bool Add(Cron cron) => _cronList.TryAdd(cron);
        public bool AddList(params Cron[] cron) => _cronList.TryAdd(cron);
        public bool Get(int index, out Cron cron) => _cronList.TryGet(index, out cron);
        public bool Remove(string minute, string hour, string day, string month, string week) => _cronList.RemoveAll(x => x.Minute == minute && x.Hour == hour && x.Day == day && x.Month == month && x.Week == week) is not 0;
        public void RemoveAll() => _cronList.Clear();
        public bool RemoveAt(int index) => _cronList.TryRemoveAt(index);

        #region SAVE
        public bool CheckFile() => this.Exists();

        public void DeleteFile() => this.Delete();

        public bool Load() => this.TryLoad(out _cronList);

        public bool Save() => this.TrySave(_cronList);
        #endregion

        #region IObservable
        public IDisposable Subscribe(IObserver<Cron> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<Cron>(_observers, observer);
        }

        private class Unsubscriber<Cron>(List<IObserver<Cron>> observers, IObserver<Cron> observer) : IDisposable
        {
            void IDisposable.Dispose()
            {
                if (observer is not null && observers.Contains(observer))
                    observers.Remove(observer);
            }
        }
        #endregion
    }
}
