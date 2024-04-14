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

        public static T Load<T>(this IFileSave fileSave, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamReader(path))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }

    public interface IFileSave { }
}
