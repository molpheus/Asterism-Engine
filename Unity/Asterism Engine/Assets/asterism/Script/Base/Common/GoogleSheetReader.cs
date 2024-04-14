using System;
using System.IO;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

namespace Asterism.Common
{
    // https://qiita.com/simanezumi1989/items/32436230dadf7a123de8
    public class GoogleSheetReader
    {
        private const string _uRLBase = "https://docs.google.com/spreadsheets/d/e/{0}/pub?gid=0&single=true&output=csv&sheet={1}";

#pragma warning disable CS1998 // ���̔񓯊����\�b�h�ɂ� 'await' ���Z�q���Ȃ����߁A�����I�Ɏ��s����܂��B'await' ���Z�q���g�p���Ĕ�u���b�L���O API �Ăяo����ҋ@���邩�A'await Task.Run(...)' ���g�p���ăo�b�N�O���E���h�̃X���b�h�ɑ΂��� CPU ��̂̏��������s���邱�Ƃ��������Ă��������B
        public static async void LoadSheet(string sheetID, string sheetName, Action<string> complete = null, Action<string> error = null)
#pragma warning restore CS1998 // ���̔񓯊����\�b�h�ɂ� 'await' ���Z�q���Ȃ����߁A�����I�Ɏ��s����܂��B'await' ���Z�q���g�p���Ĕ�u���b�L���O API �Ăяo����ҋ@���邩�A'await Task.Run(...)' ���g�p���ăo�b�N�O���E���h�̃X���b�h�ɑ΂��� CPU ��̂̏��������s���邱�Ƃ��������Ă��������B
        {
            var url = string.Format(_uRLBase, sheetID, sheetName);
            LoadSheet(url, complete, error);
        }

        public static async void LoadSheet(string URL, Action<string> complete = null, Action<string> error = null)
        {
            var request = UnityWebRequest.Get(URL);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                error?.Invoke(request.error);
            }
            else
            {
                complete?.Invoke(request.downloadHandler.text);
            }
        }


        public static void LoadCSVStringLine(string str, Action<string[]> readLine, int skipLine = 0)
        {
            var reader = new StringReader(str);
            int lineCnt = 0;
            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                if (skipLine > lineCnt)
                {
                    lineCnt++;
                    continue;
                }
                readLine(line.Split(','));
            }
        }
    }
}