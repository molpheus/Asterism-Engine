using System;
using System.Diagnostics;
using System.IO;

namespace Asterism.Common.FileManagement
{
    public sealed class StringFileHandler : IFileSaveSettings<string>
    {
        public string FilePath { get; }
        public StringFileHandler(string fileName, string filePath = null)
        {
            filePath ??= Directory.GetCurrentDirectory();
            FilePath = Path.Combine(filePath, fileName);
        }

        public bool Save(string data)
        {
            try
            {
                using (var stream = new StreamWriter(FilePath))
                {
                    stream.Write(data);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public bool Load(out string data)
        {
            data = default;
            if (!File.Exists(FilePath))
                return false;
            using (var stream = new StreamReader(FilePath))
                data = stream.ReadToEnd();
            return true;
        }
        public bool Exists() => File.Exists(FilePath);
        public void Delete() => File.Delete(FilePath);
    }
}
