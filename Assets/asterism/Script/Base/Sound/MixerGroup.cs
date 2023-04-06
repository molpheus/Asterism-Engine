using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asterism
{
    using Common;
    namespace Engine
    {
        public abstract class MixerGroup : Enumeration
        {
            public string MixerName { get; }

            public MixerGroup(int id, string name, string mixerName) : base(id, name)
            {
                MixerName = mixerName;
            }
        }
    }
}
