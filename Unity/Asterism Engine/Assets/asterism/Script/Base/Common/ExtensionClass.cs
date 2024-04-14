using System.Linq;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Asterism
{
    using Engine;

    public static class ExtensionClass
    {
        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="name"> 参照Addressableファイル名 </param>
        public static async UniTask LoadAddressable(this Image img, string name)
        {
            var sp = await ResourceReciver.LoadAsync<Sprite>(name);
            img.sprite = sp;
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="labelName"> 参照ラベル名 </param>
        /// <param name="name"> 参照Addressableファイル名 </param>
        public static async UniTask LoadAddressable(this Image img, string labelName, string name)
        {
            var list = await ResourceReciver.LoadTagAsync<Sprite>(labelName);
            var sp = list.Where(p => p.name == name).FirstOrDefault();
            if (sp != default(Sprite))
            {
                img.sprite = sp;
            }
        }

        /// <summary>
        /// オーディオソースの取得
        /// </summary>
        /// <param name="soundData"></param>
        /// <returns></returns>
        public static async UniTask<AudioClip> LoadAddressable(this ISoundId soundData)
        {
            AudioClip clip = null;
            var list = await ResourceReciver.LoadTagAsync<AudioClip>(soundData.LabelPath);
            var sp = list.Where(p => p.name == soundData.Path).FirstOrDefault();
            if (sp != default(Sprite))
            {
                clip = sp;
            }
            return clip;
        }
    }
}
