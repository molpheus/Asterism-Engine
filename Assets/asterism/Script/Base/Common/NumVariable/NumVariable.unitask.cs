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
        /// UniTaskで徐々に数値に足すようにする
        /// </summary>
        /// <param name="addValue">足す値</param>
        /// <param name="speed">速度</param>
        /// <param name="loopTiming"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="update">更新タイミングでの呼び出し</param>
        /// <param name="finish">完了タイミングでの呼び出し</param>
        /// <param name="cancel">キャンセルされたら呼び出し</param>
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
        /// UniTaskで徐々に数値に引くようにする
        /// </summary>
        /// <param name="addValue">引く値</param>
        /// <param name="speed">速度</param>
        /// <param name="loopTiming"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="update">更新タイミングでの呼び出し</param>
        /// <param name="finish">完了タイミングでの呼び出し</param>
        /// <param name="cancel">キャンセルされたら呼び出し</param>
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
