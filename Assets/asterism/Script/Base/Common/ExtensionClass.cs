using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public static async void LoadAddressable(this Image img, string name)
        {
            await ResourceReciver.LoadAsync<Sprite>(name, _ => {
                img.sprite = _;
            });
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="labelName"> 参照ラベル名 </param>
        /// <param name="name"> 参照Addressableファイル名 </param>
        public static async void LoadAddressable(this Image img, string labelName, string name)
        {
            await ResourceReciver.LoadTagAsync<Sprite>(labelName, _ => {
                var sp = _.Where(p => p.name == name).FirstOrDefault();
                if (sp != default(Sprite)) {
                    img.sprite = sp;
                }
            });
        }

        /// <summary>
        /// オーディオソースの取得
        /// </summary>
        /// <param name="soundData"></param>
        /// <returns></returns>
        public static async UniTask<AudioClip> LoadAddressable(this ISoundId soundData)
        {
            AudioClip clip = null;
            bool isLoad = true;
            await ResourceReciver.LoadTagAsync<AudioClip>(soundData.LabelPath, _ => {
                var sp = _.Where(p => p.name == soundData.Path).FirstOrDefault();
                if (sp != default(Sprite)) {
                    clip = sp;
                }
                isLoad = false;
            });

            await UniTask.WaitWhile(() => isLoad);

            return clip;
        }
    }
}
