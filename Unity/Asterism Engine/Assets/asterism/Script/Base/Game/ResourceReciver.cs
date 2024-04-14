using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asterism.Engine
{
    public class ResourceReciver : MonoBehaviour
    {
        /// <summary>
        /// �w��p�X�̃t�@�C�����P�����擾����
        /// </summary>
        /// <typeparam name="T"> �擾�I�u�W�F�N�g�̃^�C�v�w�� </typeparam>
        /// <param name="path"> Addressable Group��Path </param>
        /// <returns></returns>
        public static async UniTask<T> LoadAsync<T>(string path)
        {
            var data = await Addressables.LoadAssetAsync<T>(path);
            return data;
        }

        /// <summary>
        /// �w�胉�x�����̃I�u�W�F�N�g�̃��X�g���擾����
        /// </summary>
        /// <typeparam name="T"> �擾�I�u�W�F�N�g�̃^�C�v�w�� </typeparam>
        /// <param name="labelName"> Addressable Group��Labels </param>
        /// <returns></returns>
        public static async UniTask<List<T>> LoadTagAsync<T>(string labelName)
        {
            var handle = Addressables.LoadAssetsAsync<T>(labelName, null);
            await handle;
            return (List<T>)handle.Result;
        }
    }
}
