using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Asterism.Common.FileManagement;
using Asterism.Common.Extension;

namespace Asterism.System.Cron
{
    public partial class CronSchedule
    {
        protected List<CronExpression> _cronList = null;
        public int Count => _cronList.Count;
        private List<IObserver<CronExpression>> _observers = null;
        public IFileSave<List<CronExpression>> FileHandler => _fileHandler;
        private readonly IFileSave<List<CronExpression>> _fileHandler;

        public CronSchedule(string filePath = null)
        {
            _cronList = new List<CronExpression>();
            _observers = new List<IObserver<CronExpression>>();
            _fileHandler = new XmlFileHandler<List<CronExpression>>("cron.xml", filePath);
        }
    }

    public partial class CronSchedule : IScheduledUpdatable
    {
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
    }

    public partial class CronSchedule : ICronList
    {
        public bool Add(string format) => _cronList.TryAdd(new CronExpression(format));
        public bool Add(CronExpression cronExpression) => _cronList.TryAdd(cronExpression);
        public bool Add(IList<CronExpression> list) => _cronList.TryAdd(list);
        public bool Get(int index, out CronExpression cron) => _cronList.TryGet(index, out cron);

        public bool Get(string format, out CronExpression cron)
        {
            cron = _cronList.FirstOrDefault(x => x.IsMatchFormat(format));

            return cron is not { };
        }

        public bool Remove(string format)
        {
            var cron = _cronList.FirstOrDefault(x => x.IsMatchFormat(format));
            if (cron is not { })
                return false;

            _cronList.Remove(cron);
            return true;
        }

        public void RemoveAll() => _cronList.Clear();

        public bool RemoveAt(int index)
        {
            _cronList.TryGet(index, out var result);
            if (result is not { })
                return false;

            _cronList.Remove(result);
            return true;
        }
    }

    public partial class CronSchedule : INullable<CronSchedule>
    { }

    public partial class CronSchedule : IObservable<CronExpression>
    {
        public IDisposable Subscribe(IObserver<CronExpression> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<CronExpression>(_observers, observer);
        }

        private class Unsubscriber<CronExpression>(List<IObserver<CronExpression>> observers, IObserver<CronExpression> observer) : IDisposable
        {
            void IDisposable.Dispose()
            {
                if (observer is not null && observers.Contains(observer))
                    observers.Remove(observer);
            }
        }
    }
}
