using System;

namespace Asterism
{
    public abstract class Locator : IDisposable
    {
        public Locator()
        {
            ServiceLocator.Register(this);
        }

        public void Dispose()
        {
            ServiceLocator.Unregister(this);
        }
    }
}
