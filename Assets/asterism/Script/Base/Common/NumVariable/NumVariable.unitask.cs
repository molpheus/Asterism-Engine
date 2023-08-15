using System;
using System.Threading;

using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asterism
{
    public partial class NumVariable<T>
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
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
        public async UniTask AnimatedAdd(
            T addValue,
            float speed = 1f,
            PlayerLoopTiming loopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default,
            Action<T> update = null,
            Action<T> finish = null,
            Action cancel = null
        ) {
            T value = _value;
            T newValue = Add(addValue, true);

            float delta = 0f;
            while (delta < 1f)
            {
                if (cancellationToken.IsCancellationRequested) break;

                delta += Time.deltaTime * speed;
                var v = Mathf.Lerp((dynamic)value, (dynamic)newValue, delta);
                update?.Invoke(v);
                await UniTask.DelayFrame(1, loopTiming, cancellationToken);
            }

            _value = newValue;

            if (cancellationToken.IsCancellationRequested)
                cancel?.Invoke();
            else
                finish?.Invoke(_value);
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
        public async UniTask AnimatedSub(
            T addValue,
            float speed = 1f,
            PlayerLoopTiming loopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default,
            Action<T> update = null,
            Action<T> finish = null,
            Action cancel = null
        )
        {
            T value = _value;
            T newValue = Sub(addValue, true);

            float delta = 0f;
            while (delta < 1f)
            {
                if (cancellationToken.IsCancellationRequested) break;

                delta += Time.deltaTime * speed;
                var v = Mathf.Lerp((dynamic)value, (dynamic)newValue, delta);
                update?.Invoke(v);
                await UniTask.DelayFrame(1, loopTiming, cancellationToken);
            }

            _value = newValue;

            if (cancellationToken.IsCancellationRequested)
                cancel?.Invoke();
            else
                finish?.Invoke(_value);
        }
    }
}
