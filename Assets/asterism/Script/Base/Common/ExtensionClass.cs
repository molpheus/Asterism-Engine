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
        /// �摜�̓ǂݍ���
        /// </summary>
        /// <param name="name"> �Q��Addressable�t�@�C���� </param>
        public static async UniTask LoadAddressable(this Image img, string name)
        {
            var sp = await ResourceReciver.LoadAsync<Sprite>(name);
            img.sprite = sp;
        }

        /// <summary>
        /// �摜�̓ǂݍ���
        /// </summary>
        /// <param name="labelName"> �Q�ƃ��x���� </param>
        /// <param name="name"> �Q��Addressable�t�@�C���� </param>
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
        /// �I�[�f�B�I�\�[�X�̎擾
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
