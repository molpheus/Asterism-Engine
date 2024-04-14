namespace Asterism
{
    using Common;
    namespace Engine.Audio
    {
        public abstract class SoundId : Enumeration, ISoundId
        {
            public string Path { get; }
            public string MixerGroupTag { get; }
            public string LabelPath { get; }
            protected SoundId(int id, string name, string path, string labelName, string mixerGroupTag = "") : base(id, name)
            {
                LabelPath = labelName;
                Path = path;
                MixerGroupTag = mixerGroupTag;
            }

            //public static IEnumerable<SoundId> GetItems() => GetItems<SoundId>();

            //public static ISoundId GetSoundData(int id)
            //{
            //    var data = GetItems().Where(p => p.Id == id).FirstOrDefault();
            //    if (data == default(SoundId)) {
            //        return null;
            //    }
            //    return data;
            //}

        }
    }
}
