using System.IO;

using Asterism.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common
{
    [TestClass]
    public class SaveTest
    {
        public class Save : IFileSave
        {
            public string FilePath { get; }

            public Save(string filePath)
            {
                FilePath = filePath;
            }
        }

        public class SaveData
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [TestInitialize]
        public void Initialize()
        {
            _instance = new Save("./test.xml");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_instance.Exists())
                _instance.Delete();

            _instance = null;
        }

        private Save _instance;

        [TestMethod]
        public void ファイルをセーブする()
        {
            SaveData data = new();

            _instance.Save(data);

            Assert.IsTrue(_instance.Exists());
        }

        [TestMethod]
        public void セーブしたファイルをロードする()
        {
            SaveData data = new()
            {
                Name = "Test",
                Age = 20
            };

            _instance.Save(data);

            Assert.IsTrue(_instance.Exists());

            SaveData loadData = _instance.Load<SaveData>();

            Assert.AreEqual(data.Name, loadData.Name);
            Assert.AreEqual(data.Age, loadData.Age);
        }

        [TestMethod]
        public void ファイルを削除する()
        {
            SaveData data = new();

            _instance.Save(data);

            Assert.IsTrue(_instance.Exists());

            _instance.Delete();

            Assert.IsFalse(_instance.Exists());
        }

        [TestMethod]
        public void ファイルが存在しない場合()
        {
            Assert.IsFalse(_instance.Exists());
        }

        [TestMethod]
        public void ファイルが存在しない場合にロードする()
        {
            SaveData data = new();

            Assert.IsFalse(_instance.TryLoad(out SaveData loadData));
            Assert.IsNull(loadData);
        }

        [TestMethod]
        public void ファイルが存在しない場合に削除する()
        {
            _instance.Delete();
            Assert.IsFalse(_instance.Exists());
        }

        [TestMethod]
        public void ファイルが存在しない場合にセーブする()
        {
            SaveData data = new();

            Assert.IsTrue(_instance.TrySave(data));
            Assert.IsTrue(_instance.Exists());
        }
    }
}
