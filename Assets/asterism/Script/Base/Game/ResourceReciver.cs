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
        public static async UniTask LoadAsync<T>(string path, Action<T> act)
        {
            var data = await Addressables.LoadAssetAsync<T>(path);
            act?.Invoke(data);
        }

        public static async UniTask LoadTagAsync<T>(string labelName, Action<List<T>> act)
        {
            var handle = Addressables.LoadAssetsAsync<T>(labelName, null);
            await handle;
            act?.Invoke((List<T>)handle.Result);
            
        }
    }
}
