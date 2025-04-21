using System;
using System.Collections.Generic;
using System.Linq;

using Asterism.Common.Extension;
using Asterism.Common.FileManagement;

namespace Asterism.System.Cron
{
    public partial class CronSchedule
    {
        protected List<CronExpression> _cronList = null;
        public int Count => _cronList.Count;
        private readonly IFileSaveSettings<List<CronExpression>> _fileHandler;

        public CronSchedule(string filePath = null)
        {
            _cronList = new List<CronExpression>();
            _observers = new List<IObserver<CronExpression>>();
            _fileHandler = new XmlFileHandler<List<CronExpression>>("cron.xml", filePath);
        }
    }

    public partial class CronSchedule : IFileSave
    {
        public bool Save() => _fileHandler.Save(_cronList);
        public bool Load() => _fileHandler.Load(out _cronList);
        public bool Exists() => _fileHandler.Exists();
        public void Delete() => _fileHandler.Delete();
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
        public bool Add(IList<CronExpression> list) => _cronList.TryAddRange(list);
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

        private List<IObserver<CronExpression>> _observers = null;

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
