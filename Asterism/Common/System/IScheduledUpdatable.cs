using System;

namespace Asterism.System
{
    public interface IScheduledUpdatable
    {
        void Update(DateTime now);
    }
}
