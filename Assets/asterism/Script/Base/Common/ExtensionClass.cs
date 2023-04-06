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
        /// �摜�̓ǂݍ���
        /// </summary>
        /// <param name="name"> �Q��Addressable�t�@�C���� </param>
        public static async void LoadAddressable(this Image img, string name)
        {
            await ResourceReciver.LoadAsync<Sprite>(name, _ => {
                img.sprite = _;
            });
        }

        /// <summary>
        /// �摜�̓ǂݍ���
        /// </summary>
        /// <param name="labelName"> �Q�ƃ��x���� </param>
        /// <param name="name"> �Q��Addressable�t�@�C���� </param>
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
        /// �I�[�f�B�I�\�[�X�̎擾
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
