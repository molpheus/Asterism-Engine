using Asterism.Audio;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Audio;

namespace Asterism.Engine
{
    public abstract class SoundManager : EngineBase<SoundManager>
    {
        protected virtual string MixerPath { get; }
        protected const float MIN_VOLUME = -80f;

        protected AudioMixer _mixer;

        private BgmController _bgmController;

        public bool IsInitialize { get; private set; } = false;

        private async UniTaskVoid Start()
        {
            IsInitialize = false;
            _mixer = await ResourceReciver.LoadAsync<AudioMixer>(MixerPath);
            var masterGroup = _mixer.FindMatchingGroups("Master");

            _bgmController = gameObject.AddComponent<BgmController>();

            IsInitialize = true;
        }

        /// <summary>
        /// MixerÇÃéÊìæ
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        protected virtual AudioMixerGroup GetMixerGroup(string groupName)
        {
            return groupName != "" ? _mixer.FindMatchingGroups(groupName)[0] : null;
        }

        /// <summary>
        /// BGMÇÃçƒê∂
        /// </summary>
        /// <param name="soundData"></param>
        public async void PlayBGM(ISoundId soundData, float volume = 1f, float fade = 0f, bool isCrossFade = false)
        {
            Debugger.Log(soundData);
            if (soundData != null)
            {
                AudioClip clip = await soundData.LoadAddressable();
                _bgmController.SetMixer(GetMixerGroup(soundData.MixerGroupTag));
                _bgmController.Play(clip, true, volume, fade, isCrossFade);
            }
        }

        /// <summary>
        /// SEÇÃçƒê∂
        /// </summary>
        /// <param name="soundData"></param>
        public async void PlaySE(ISoundId soundData)
        {
            if (soundData != null)
            {
                AudioClip clip = await soundData.LoadAddressable();
                var source = SoundSource.Create(transform);
                source.SetClip(clip)
                        .SetLoop(false)
                        .SetMixerGroup(GetMixerGroup(soundData.MixerGroupTag));

                await source.Play(true);
            }
        }
    }

    public interface ISoundId
    {
        string Path { get; }
        string MixerGroupTag { get; }
        string LabelPath { get; }
    }
}
