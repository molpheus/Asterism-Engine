using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Common.Crypt
{
    [TestClass]
    public class AESTest
    {
        [TestMethod]
        public void EncryptTest()
        {
            var aes = new Asterism.Common.Crypt.AES();
            var text = "test";
            var encrypted = aes.Encrypt(text);
            var decrypted = aes.Decrypt(encrypted);
            Assert.AreEqual(text, decrypted);
        }
    }
}
