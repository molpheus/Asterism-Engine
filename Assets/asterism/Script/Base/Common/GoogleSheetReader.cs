using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Networking;

namespace Asterism.Common
{
    // https://qiita.com/simanezumi1989/items/32436230dadf7a123de8
    public class GoogleSheetReader
    {
        private const string URLBase = "https://docs.google.com/spreadsheets/d/e/{0}/pub?gid=0&single=true&output=csv&sheet={1}";

#pragma warning disable CS1998 // この非同期メソッドには 'await' 演算子がないため、同期的に実行されます。'await' 演算子を使用して非ブロッキング API 呼び出しを待機するか、'await Task.Run(...)' を使用してバックグラウンドのスレッドに対して CPU 主体の処理を実行することを検討してください。
        public static async void LoadSheet(string sheetID, string sheetName, Action<string> complete = null, Action<string> error = null)
#pragma warning restore CS1998 // この非同期メソッドには 'await' 演算子がないため、同期的に実行されます。'await' 演算子を使用して非ブロッキング API 呼び出しを待機するか、'await Task.Run(...)' を使用してバックグラウンドのスレッドに対して CPU 主体の処理を実行することを検討してください。
        {
            var url = string.Format(URLBase, sheetID, sheetName);
            LoadSheet(url, complete, error);
        }

        public static async void LoadSheet(string URL, Action<string> complete = null, Action<string> error = null)
        {
            var request = UnityWebRequest.Get(URL);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError) {
                error?.Invoke(request.error);
            }
            else {
                complete?.Invoke(request.downloadHandler.text);
            }
        }


        public static void LoadCSVStringLine(string str, Action<string[]> readLine, int skipLine = 0)
        {
            var reader = new StringReader(str);
            int lineCnt = 0;
            while (reader.Peek() != -1) {
                var line = reader.ReadLine();
                if (skipLine > lineCnt) {
                    lineCnt++;
                    continue;
                }
                readLine(line.Split(','));
            }
        }
    }
}