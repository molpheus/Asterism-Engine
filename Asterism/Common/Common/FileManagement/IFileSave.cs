namespace Asterism.Common.FileManagement
{
    public interface IFileSave
    {
        /// <summary>
        /// ファイルに保存する
        /// </summary>
        /// <param name="data"></param>
        public bool Save();
        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Load();
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
