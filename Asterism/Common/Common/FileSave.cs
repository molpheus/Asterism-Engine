using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Asterism.Common
{
    public static class FileSave
    {
        public static void Save<T>(this IFileSave fileSave, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamWriter(fileSave.FilePath))
            {
                serializer.Serialize(stream, data);
            }
        }

        public static bool TrySave<T>(this IFileSave fileSave, T data)
        {
            try
            {
                fileSave.Save(data);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static T Load<T>(this IFileSave fileSave)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var stream = new StreamReader(fileSave.FilePath))
                return (T)serializer.Deserialize(stream);
        }

        public static bool TryLoad<T>(this IFileSave fileSave, out T data)
        {
            data = default;
            if (!File.Exists(fileSave.FilePath))
                return false;

            data = fileSave.Load<T>();
            return true;
        }

        public static bool Exists(this IFileSave fileSave) => File.Exists(fileSave.FilePath);
        public static void Delete(this IFileSave fileSave) => File.Delete(fileSave.FilePath);
    }

    public interface IFileSave
    {
        string FilePath { get; }
    }
}
