using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Asterism.Common.FileManagement
{
    public sealed class XmlFileHandler<T> : IFileSave<T>
    {
        public string FileName { get; }
        public string FilePath { get; }

        public XmlFileHandler(string fileName, string filePath = null)
        {
            filePath ??= Directory.GetCurrentDirectory();
            FilePath = Path.Combine(filePath, fileName);
        }

        public bool Save(T data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var stream = new StreamWriter(FilePath))
                {
                    serializer.Serialize(stream, data);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool Load(out T data)
        {
            data = default;
            if (!File.Exists(FilePath))
                return false;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var stream = new StreamReader(FilePath))
                data = (T)serializer.Deserialize(stream);

            return true;
        }

        public bool Exists() => File.Exists(FilePath);
        public void Delete() => File.Delete(FilePath);
    }

    
}
