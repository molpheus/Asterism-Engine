using System;
using System.Collections.Generic;

using Asterism.Common.Extension;
using Asterism.Common.FileManagement;

namespace Asterism.System.RubberDuck
{

    public partial class RubberDuck
    {
        private readonly IFileSaveSettings<string> _fileHandler;

        private string _fileData = string.Empty;

        private TimeSpan _waitingDuck;

        public RubberDuck(string filePath = null, TimeSpan waitingDuck = default)
        {
            _fileHandler = new StringFileHandler("rubberduck.txt", filePath);

            if (_fileHandler.Exists())
            {
                _fileHandler.Load(out _fileData);
            }

            _waitingDuck = waitingDuck;
        }
    }

    public partial class RubberDuck
    {
        public bool Add(string format)
        {
            if (string.IsNullOrEmpty(format))
                return false;
            _fileData += format + Environment.NewLine;
            return true;
        }

        public bool Add(IList<string> list)
        {
            if (list is null || list.Count == 0)
                return false;
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item))
                    _fileData += item + Environment.NewLine;
            }
            return true;
        }
    }


    public partial class RubberDuck : IFileSave
    {
        public bool Save() => _fileHandler.Save(_fileData);
        public bool Load() => _fileHandler.Load(out _fileData);
        public bool Exists() => _fileHandler.Exists();
        public void Delete() => _fileHandler.Delete();
    }


    public partial class RubberDuck : IScheduledUpdatable
    {
        public void Update(DateTime now)
        {
            if (now.TimeOfDay > _waitingDuck)
            {
                _observers.ForEach(x => x.OnNext(""));
            }
        }
    }

    public partial class RubberDuck : INullable<RubberDuck>
    { }

    public partial class RubberDuck : IObservable<string>
    {

        private List<IObserver<string>> _observers = null;

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<string>(_observers, observer);
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
