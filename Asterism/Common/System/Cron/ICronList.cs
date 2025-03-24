using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.System.Cron
{
    public interface ICronList
    {
        public bool Add(string format);
        public bool Add(CronExpression cronExpression);
        public bool Add(IList<CronExpression> list);
        public bool Get(int index, out CronExpression cron);
        public bool Get(string format, out CronExpression cron);
        public bool RemoveAt(int index);
        public bool Remove(string format);
        public void RemoveAll();
    }
}
