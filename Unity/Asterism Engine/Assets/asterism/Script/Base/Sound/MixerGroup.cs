namespace Asterism
{
    using Common;
    namespace Engine
    {
        public abstract class MixerGroup : Enumeration
        {
            public string MixerName { get; }

            public MixerGroup(int id, string name, string mixerName) : base(name, id)
            {
                MixerName = mixerName;
            }
        }
    }
}
