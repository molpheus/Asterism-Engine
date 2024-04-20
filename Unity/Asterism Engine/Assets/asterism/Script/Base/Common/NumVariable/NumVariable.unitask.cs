using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Asterism.Common
{
    /// <summary>
    /// Asterism.Common.NumVariableのUniTask拡張
    /// </summary>
    public static class NumVariableUniTaskExtension
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
        /// UniTaskで徐々に数値に引くようにする
        /// </summary>
        /// <param name="addValue">引く値</param>
        /// <param name="speed">速度</param>
        /// <param name="loopTiming"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="update">更新タイミングでの呼び出し</param>
        /// <param name="finish">完了タイミングでの呼び出し</param>
        /// <param name="cancel">キャンセルされたら呼び出し</param>
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
