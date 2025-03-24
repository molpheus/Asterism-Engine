using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.System
{
    public interface IScheduledUpdatable
    {
        void Update(DateTime now);
    }
}
