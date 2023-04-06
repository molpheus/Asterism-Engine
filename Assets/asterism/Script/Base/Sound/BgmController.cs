using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Asterism.Audio
{
    public class BgmController : MonoBehaviour
    {
        private SoundSource _source1;
        private SoundSource _source2;

        private void Awake()
        {
            _source1 = SoundSource.Create(transform);
            _source2 = SoundSource.Create(transform);
        }

        public void SetMixer(AudioMixerGroup group)
        {
            _source1.SetMixerGroup(group);
            _source2.SetMixerGroup(group);
        }

        public void Play(AudioClip clip, bool isLoop = false, float volume = 1f, float fade = 0f, bool isCrossFade = false)
        {
            if (_source1.IsPlaying) {
                _source2.SetClip(clip)
                        .SetLoop(isLoop)
                        .SetVolume(volume);
                _source1.Stop(isCrossFade ? fade : 0).Forget();
                _source2.Play(false, fade).Forget();
            }
            else if (_source2.IsPlaying) {
                _source1.SetClip(clip)
                        .SetLoop(isLoop)
                        .SetVolume(volume);
                _source2.Stop(isCrossFade ? fade : 0).Forget();
                _source1.Play(false, fade).Forget();
            }
            else {
                _source1.SetClip(clip)
                        .SetLoop(isLoop)
                        .SetVolume(volume);
                _source1.Play(false, fade).Forget();
            }
        }
    }
}
