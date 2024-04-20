using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Asterism.Common
{
    /// <summary>
    /// Asterism.Common.NumVariable��UniTask�g��
    /// </summary>
    public static class NumVariableUniTaskExtension
    {
        /// <summary>
        /// UniTask�ŏ��X�ɐ��l�ɑ����悤�ɂ���
        /// </summary>
        /// <param name="addValue">�����l</param>
        /// <param name="speed">���x</param>
        /// <param name="loopTiming"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="update">�X�V�^�C�~���O�ł̌Ăяo��</param>
        /// <param name="finish">�����^�C�~���O�ł̌Ăяo��</param>
        /// <param name="cancel">�L�����Z�����ꂽ��Ăяo��</param>
        public static async UniTask AnimatedAdd<T>(
            this NumVariable<T> numVariable,
            T addValue,
            float speed = 1f,
            PlayerLoopTiming loopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default,
            Action<T> update = null,
            Action<T> finish = null,
            Action cancel = null
        ) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            T value = numVariable.Value;
            T newValue = numVariable.Add(addValue, true);

            float delta = 0f;
            while (delta < 1f)
            {
                if (cancellationToken.IsCancellationRequested) break;

                delta += Time.deltaTime * speed;
                var v = Mathf.Lerp((dynamic)value, (dynamic)newValue, delta);
                update?.Invoke(v);
                await UniTask.DelayFrame(1, loopTiming, cancellationToken);
            }

            numVariable.SetValue(newValue);

            if (cancellationToken.IsCancellationRequested)
                cancel?.Invoke();
            else
                finish?.Invoke(numVariable.Value);  
        }

        /// <summary>
        /// UniTask�ŏ��X�ɐ��l�Ɉ����悤�ɂ���
        /// </summary>
        /// <param name="addValue">�����l</param>
        /// <param name="speed">���x</param>
        /// <param name="loopTiming"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="update">�X�V�^�C�~���O�ł̌Ăяo��</param>
        /// <param name="finish">�����^�C�~���O�ł̌Ăяo��</param>
        /// <param name="cancel">�L�����Z�����ꂽ��Ăяo��</param>
        public static async UniTask AnimatedSub<T>(
            this NumVariable<T> numVariable,
            T addValue,
            float speed = 1f,
            PlayerLoopTiming loopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default,
            Action<T> update = null,
            Action<T> finish = null,
            Action cancel = null
        ) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            T value = numVariable.Value;
            T newValue = numVariable.Sub(addValue, true);

            float delta = 0f;
            while (delta < 1f)
            {
                if (cancellationToken.IsCancellationRequested) break;

                delta += Time.deltaTime * speed;
                var v = Mathf.Lerp((dynamic)value, (dynamic)newValue, delta);
                update?.Invoke(v);
                await UniTask.DelayFrame(1, loopTiming, cancellationToken);
            }
            numVariable.SetValue(newValue);

            if (cancellationToken.IsCancellationRequested)
                cancel?.Invoke();
            else
                finish?.Invoke(numVariable.Value);
        }
    }
}
