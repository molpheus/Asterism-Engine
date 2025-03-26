using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asterism.Common.FileManagement
{
    public interface IFileSaveSettings<T>
    {
        string FilePath { get; }

        /// <summary>
        /// ファイルに保存する
        /// </summary>
        /// <param name="data"></param>
        public bool Save(T data);
        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Load(out T data);
        /// <summary>
        /// 指定のファイルが存在するか
        /// </summary>
        /// <returns></returns>
        public bool Exists();
        /// <summary>
        /// ファイルを削除する
        /// </summary>
        public void Delete();
    }
}
