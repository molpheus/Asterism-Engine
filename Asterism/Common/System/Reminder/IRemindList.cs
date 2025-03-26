using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.System.Reminder
{
    public interface IRemindList
    {
        public bool Add(DateTime time, string message);
        public bool Add(RemindData remindData);
        public bool AddList(params RemindData[] remindData);
        public bool Get(int index, out RemindData remindData);
        public bool Remove(DateTime time);
        public void RemoveAll();
    }
}
