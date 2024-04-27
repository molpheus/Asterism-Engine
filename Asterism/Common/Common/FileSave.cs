using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Asterism.Common
{
    public static class FileSave
    {
        public static void Save<T>(this IFileSave fileSave, string path, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamWriter(path))
            {
                serializer.Serialize(stream, data);
            }
        }

        public static bool TrySave<T>(this IFileSave fileSave, string path, T data)
        {
            try
            {
                fileSave.Save(path, data);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static T Load<T>(this IFileSave fileSave, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamReader(path))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        public static bool TryLoad<T>(this IFileSave fileSave, string path, out T data)
        {
            data = default;
            if (!File.Exists(path))
                return false;

            data = fileSave.Load<T>(path);
            return true;
        }
    }

    public interface IFileSave { }
}
