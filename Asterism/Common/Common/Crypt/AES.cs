using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Asterism.Common.Crypt
{
    public sealed class AES
    {
        public enum BlockSize
        {
            Block128 = 128,
            Block192 = 192,
            Block256 = 256
        }

        public byte[] Key { get; }
        public byte[] IV { get; }

        public BlockSize Block { get; }


        public AES()
        {
            Block = BlockSize.Block256;
            (Key, IV) = GenerateKey(Block);
        }

        public AES(byte[] _key, byte[] _IV, BlockSize _blockSize)
        {
            Key = _key;
            IV = _IV;
            Block = _blockSize;
        }

        /// <summary>
        /// キーを生成する
        /// </summary>
        /// <param name="blockSize"></param>
        /// <returns> ( Key , IV ) </returns>
        public static (byte[], byte[]) GenerateKey(BlockSize blockSize = BlockSize.Block256)
        {
            using (var aes = new AesManaged())
            {
                aes.KeySize = (int)blockSize; // 256ビット（32バイト）キー
                aes.GenerateKey();
                aes.GenerateIV();

                return (aes.Key, aes.IV);
            }
        }

        public byte[] Encrypt(string text)
        {
            using (var aes = new AesManaged())
            {
                aes.KeySize = (int)Block;
                aes.Key = Key;
                aes.IV = IV;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var input = Encoding.UTF8.GetBytes(text);
                var output = encryptor.TransformFinalBlock(input, 0, input.Length);

                return output;
            }
        }

        public string Decrypt(byte[] data)
        {
            using (var aes = new AesManaged())
            {
                aes.KeySize = (int)Block;
                aes.Key = Key;
                aes.IV = IV;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var output = decryptor.TransformFinalBlock(data, 0, data.Length);

                return Encoding.UTF8.GetString(output);
            }
        }
    }
}
