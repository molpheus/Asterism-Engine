using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Audio;

namespace Asterism.Audio
{
    public class SoundSource : MonoBehaviour
    {
        public enum TimeScaleType
        {
            GameTime,
            Unscaled,
            Fixed,
            Fixed_unscaled,
        }

        public TimeScaleType TimeScale { get; set; } = TimeScaleType.GameTime;

        private AudioSource _source;

        public bool IsPlaying { get { return _source.isPlaying; } }

        public static SoundSource Create(Transform parent = null)
        {
            var obj = new GameObject("SoundSource", typeof(SoundSource));
            obj.transform.SetParent(parent);
            return obj.GetComponent<SoundSource>();
        }

        private void Awake()
        {
            _source = gameObject.AddComponent<AudioSource>();
        }

        public SoundSource SetClip(AudioClip clip)
        {
            _source.clip = clip;
            return this;
        }

        public SoundSource SetMixerGroup(AudioMixerGroup group)
        {
            _source.outputAudioMixerGroup = group;
            return this;
        }

        public SoundSource SetVolume(float volume)
        {
            _source.volume = volume;
            return this;
        }

        public SoundSource SetLoop(bool isLoop)
        {
            _source.loop = isLoop;
            return this;
        }

        public async UniTask Play(bool isAutoDelete, float fade = 0f)
        {
            if (fade != 0f)
            {
                var delta = 0f;
                var v = _source.volume;
                SetVolume(0f);
                _source.Play();
                while (delta <= fade)
                {
                    delta += Mathf.Clamp01(TimeScale switch {
                        TimeScaleType.Unscaled => Time.unscaledDeltaTime,
                        TimeScaleType.Fixed => Time.fixedDeltaTime,
                        TimeScaleType.Fixed_unscaled => Time.fixedUnscaledDeltaTime,
                        _ => Time.deltaTime,
                    });

                    _source.volume = Mathf.Clamp(delta / fade, 0, v);

                    await UniTask.Delay(1);
                }
            }
            else
            {
                _source.Play();
            }

            if (!_source.loop)
            {
                await UniTask.WaitWhile(() => _source.isPlaying);
                if (isAutoDelete) Delete();
            }
        }

        public async UniTask Stop(float fade = 0f)
        {
            if (fade != 0)
            {
                var delta = 0f;
                var v = _source.volume;
                while (delta <= fade)
                {
                    delta += Mathf.Clamp01(TimeScale switch {
                        TimeScaleType.Unscaled => Time.unscaledDeltaTime,
                        TimeScaleType.Fixed => Time.fixedDeltaTime,
                        TimeScaleType.Fixed_unscaled => Time.fixedUnscaledDeltaTime,
                        _ => Time.deltaTime,
                    });

                    _source.volume = Mathf.Clamp(delta / fade, v, 0f);

                    await UniTask.Delay(1);
                }
            }

            _source.Stop();
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}
