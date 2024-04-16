using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.Common.Extension
{
    public static class Extension
    {
        public static bool IsNullOrDefault<T>(this INullable<T> value)
        {
            return value is null || value.Equals(default(T));
        }
    }

    public interface INullable<T> {}
}
