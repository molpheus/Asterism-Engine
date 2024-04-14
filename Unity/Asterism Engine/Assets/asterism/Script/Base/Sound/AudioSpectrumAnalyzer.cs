using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Asterism.Audio
{
    public class AudioSpectrumAnalyzer : MonoBehaviour
    {
        /// <summary>
        /// ���g���̃��X�g���擾����p
        /// </summary>
        public struct Frequency
        {
            /// <summary> ������ </summary>
            private const int Partial = 8;

            /// <summary> �ŏ����g�� </summary>
            public const int Low = 16;
            /// <summary> SUB-BASS </summary>
            public const int SubBass = 60;
            /// <summary> BASS </summary>
            public const int Bass = 250;
            /// <summary> LOWER MIDRANGE </summary>
            public const int LowMid = 500;
            /// <summary> MIDRANGE </summary>
            public const int Mid = 2000;
            /// <summary> HIGHER MIDRANGE </summary>
            public const int MidHigh = 4000;
            /// <summary> PRESENCE </summary>
            public const int Presence = 6000;
            /// <summary> BRILLIANCE </summary>
            public const int Shine = 20000;
            /// <summary> �ő���g�� </summary>
            public const int Max = 24000;

            /// <summary>
            /// ���g���̃��X�g���擾����
            /// </summary>
            /// <param name="num"> ������ </param>
            /// <returns></returns>
            public static float[] GetFrequency(int num)
            {
                if (num < Partial) num = Partial;

                var splits = new int[Partial];

                var div = num / Partial;
                for (int i = 0; i < Partial; i++)
                {
                    splits[i] = div;
                }
                var rem = num % Partial;
                for (int i = 0; i < rem; i++)
                {
                    splits[i] += 1;
                }

                return GetFrequency(
                    splits[0],
                    splits[1],
                    splits[2],
                    splits[3],
                    splits[4],
                    splits[5],
                    splits[6],
                    splits[7]
                );
            }

            /// <summary>
            /// ���g���̃��X�g���擾����
            /// </summary>
            /// <param name="subBassSplit"> �T�u�o�X�͈̔͂ł̕����� </param>
            /// <param name="bassSplit"> �o�X�͈̔͂ł̕����� </param>
            /// <param name="lowMidSplit"> ��`������ł̕����� </param>
            /// <param name="midSplit"> ������ł̕����� </param>
            /// <param name="midHighSplit"> ���`������ł̕����� </param>
            /// <param name="presenceSplit"> �����g��ł̕����� </param>
            /// <param name="shineSplit"> ����ȏ�̍�����ł̕����� </param>
            /// <param name="overSplit"> ���̈�𒴂�������ł̕����� </param>
            /// <returns></returns>
            public static float[] GetFrequency(
                int subBassSplit = 1
                , int bassSplit = 1
                , int lowMidSplit = 1
                , int midSplit = 1
                , int midHighSplit = 1
                , int presenceSplit = 1
                , int shineSplit = 1
                , int overSplit = 1
            )
            {
                var list = new List<float>();

                list.AddRange(GetSplitData(subBassSplit, Low, SubBass));
                list.AddRange(GetSplitData(bassSplit, SubBass, Bass));
                list.AddRange(GetSplitData(lowMidSplit, Bass, LowMid));
                list.AddRange(GetSplitData(midSplit, LowMid, Mid));
                list.AddRange(GetSplitData(midHighSplit, Mid, MidHigh));
                list.AddRange(GetSplitData(presenceSplit, MidHigh, Presence));
                list.AddRange(GetSplitData(shineSplit, Presence, Shine));
                list.AddRange(GetSplitData(overSplit, Shine, AudioSettings.outputSampleRate));

                return list.ToArray();
            }

            /// <summary>
            /// �w�肳�ꂽ�������Ŕ͈͂𕪊�����
            /// </summary>
            /// <param name="split"> ������ </param>
            /// <param name="min"> �ŏ��l </param>
            /// <param name="max"> �ő�l </param>
            /// <returns></returns>
            private static float[] GetSplitData(int split, int min, int max)
            {
                var list = new float[split];

                if (split == 0) return list;

                var m = max - min;
                var s = max / split;

                list[0] = split == 1 ? max : min;
                for (int i = 1; i < split; i++)
                {
                    list[i] = (i * s) + min;
                }

                return list;
            }
        }

        [SerializeField]
        private int _resolution = 1024;
        [SerializeField]
        private float[] _threshold;

        public int ThresholdLength => _threshold.Length;

        public UnityEvent<float[]> Meter;

        /** �X�y�N�g�����̓E�B���h�E�̎��
         * https://docs.unity3d.com/ScriptReference/FFTWindow.html
            Rectangular     W[n] = 1.0.
            Triangle	    W[n] = TRI(2n/N).
            Hamming	        W[n] = 0.54 - (0.46 * COS(n/N) ).
            Hanning	        W[n] = 0.5 * (1.0 - COS(n/N) ).
            Blackman	    W[n] = 0.42 - (0.5 * COS(n/N) ) + (0.08 * COS(2.0 * n/N) ).
            BlackmanHarris	W[n] = 0.35875 - (0.48829 * COS(1.0 * n/N)) + (0.14128 * COS(2.0 * n/N)) - (0.01168 * COS(3.0 * n/N)).
         * */
        public FFTWindow spectrumType { get; set; } = FFTWindow.Hamming;

        /// <summary>
        /// �T���v�����̐ݒ�
        /// </summary>
        /// <param name="value"> �Q�̗ݐςł���K�v������ </param>
        public AudioSpectrumAnalyzer SetResolution(int value)
        {
            if (!ExtraMath.IsPowerOfTwo(value))
            {
                Debugger.LogError("error because it is not a power of 2");
            }
            else
            {
                _resolution = value;
            }

            return this;
        }

        /// <summary>
        /// �K�����g���т̐ݒ�
        /// </summary>
        /// <param name="thresholds"> �K�����g���ݒ� </param>
        public AudioSpectrumAnalyzer SetThreshold(params float[] thresholds)
        {
            _threshold = thresholds;
            return this;
        }

        /// <summary>
        /// �K�����g���т̐ݒ�
        /// </summary>
        /// <param name="splitCnt"> �ݒ肳��Ă�����g���斳���̂ŕ����� </param>
        public AudioSpectrumAnalyzer SetThreshold(int splitCnt)
        {
            _threshold = Frequency.GetFrequency(splitCnt);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            // �T���v�����͂Q�̗ݏ�ł͂Ȃ��Ă͂����Ȃ�
            if (!ExtraMath.IsPowerOfTwo(_resolution))
            {
                // �Q�ׂ̂���ł͖������߃G���[
                return;
            }

            // �T���v���z��̒����͂Q�̗ݏ�ōŏ���64�A�ő��8192
            var spectrum = new float[_resolution];

            // ���̊֐��ł̓t�[���G�ϊ��ɂ���Ď��g���ʂ̐������擾���邱�Ƃ��\
            // �X�y�N�g�����̓E�B���h�E�̎w��𕡎G�Ȃ��̂ɂ���ΐ��x�͏オ�邪�d���Ȃ�
            // �T���v�����O���[�g�̓I�[�f�B�I�N���b�v�Ɏw�肳��Ă�����̂ł͂Ȃ��AAudioSettings.outputSampleRate�Ŏw�肳�ꂽ�T���v�����O���[�g���g�p�����
            AudioListener.GetSpectrumData(spectrum, 0, spectrumType);
            var deltaFreq = AudioSettings.outputSampleRate / _resolution;

            var meterValue = new float[_threshold.Length];
            for (int i = 0; i < meterValue.Length; i++) { meterValue[i] = 0; }
            for (int i = 0; i < _resolution; i++)
            {
                var freq = deltaFreq * i;
                for (int t = 0; t < _threshold.Length; t++)
                {
                    if (freq <= _threshold[t])
                    {
                        meterValue[t] += spectrum[i];
                        break;
                    }
                }
            }

            Meter.Invoke(meterValue);
        }

        public enum Preset
        {
            Type1,
            Type2
        }

        /// <summary>
        /// �v���Z�b�g�̐ݒ�
        /// </summary>
        /// <param name="set"></param>
        public void SetThresholdPreset(Preset set)
        {
            switch (set)
            {
                case Preset.Type1:
                SetThreshold(60, 250, 500, 2000, 4000, 6000, 20000, 24000);
                break;

                case Preset.Type2:
                SetThreshold(32, 64, 125, 250, 500, 1000, 2000, 4000, 8000, 16000, 20000, 24000, 48000);
                break;
            }
        }

        [ContextMenu("Threshold/Preset_1")]
        private void SetThresholdPreset1()
        {
            SetThresholdPreset(Preset.Type1);
        }

        [ContextMenu("Threshold/Preset_2")]
        private void SetThresholdPreset2()
        {
            SetThresholdPreset(Preset.Type2);
        }

    }
}
