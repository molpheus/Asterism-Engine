using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asterism.Engine
{
    public class ResourceReciver : MonoBehaviour
    {
        /// <summary>
        /// 指定パスのファイルを１つだけ取得する
        /// </summary>
        /// <typeparam name="T"> 取得オブジェクトのタイプ指定 </typeparam>
        /// <param name="path"> Addressable GroupのPath </param>
        /// <returns></returns>
        public static async UniTask<T> LoadAsync<T>(string path)
        {
            var data = await Addressables.LoadAssetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// 指定ラベル名のオブジェクトのリストを取得する
        /// </summary>
        /// <typeparam name="T"> 取得オブジェクトのタイプ指定 </typeparam>
        /// <param name="labelName"> Addressable GroupのLabels </param>
        /// <returns></returns>
        public static async UniTask<List<T>> LoadTagAsync<T>(string labelName)
        {
            var handle = Addressables.LoadAssetsAsync<T>(labelName, null);
            await handle;
            return (List<T>)handle.Result;
        }
    }
}
