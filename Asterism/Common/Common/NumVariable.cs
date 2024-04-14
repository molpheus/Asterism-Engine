using System;

namespace Asterism.Common
{
    /// <summary>
    /// パラメータ制御用クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class NumVariable<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        /// <summary> 現在値の取得 </summary>
        public T Value => _value;
        private T _value;
        /// <summary> 最大値の取得 </summary>
        public T MaxValue => _maxValue;
        private T _maxValue;

        /// <summary>
        /// 現在値と最大値が同じ場合
        /// </summary>
        /// <param name="value"> 値 </param>
        public NumVariable(T value)
        {
            _value = value;
            _maxValue = value;
        }

        /// <summary>
        /// 現在値と最大値が違う場合
        /// </summary>
        /// <param name="current"> 現在値 </param>
        /// <param name="max"> 最大値 </param>
        public NumVariable(T current, T max)
        {
            _value = current;
            _maxValue = max;
        }

        /// <summary>
        /// 現在の数値から指定の数値を足す
        /// </summary>
        /// <param name="value"> 足す値 </param>
        public T Add(T value, bool isReturnOnly = false)
        {
            T tmp = (dynamic)_value + (dynamic)value;
            if (tmp < (dynamic)0) tmp = (dynamic)0;
            if (tmp > (dynamic)_maxValue) tmp = (dynamic)_maxValue;
            if (!isReturnOnly) _value = tmp;
            return tmp;
        }

        /// <summary>
        /// 現在の数値から指定の数値を引く
        /// </summary>
        /// <param name="value"> 引く値 </param>
        public T Sub(T value, bool isReturnOnly = false)
        {
            T tmp = (dynamic)_value - (dynamic)value;
            if (tmp < (dynamic)0) tmp = (dynamic)0;
            if (tmp > (dynamic)_maxValue) tmp = (dynamic)_maxValue;
            if (!isReturnOnly) _value = tmp;
            return tmp;
        }

        /// <summary>
        /// 最大値にする
        /// </summary>
        public void SetMax()
            => _value = _maxValue;

        /// <summary>
        /// 最小値にする
        /// </summary>
        public void SetMin()
            => _value = (dynamic)0;

        /// <summary>
        /// 最大値を設定する
        /// </summary>
        /// <param name="max"> 新しい最大値 </param>
        /// <param name="isMax"> 現在の値も最大値にするか </param>
        public void SetMaxValue(T max, bool isMax)
        {
            _maxValue = max;

            if (isMax) SetMax();
        }

        /// <summary>
        /// 現在値と最大値の最大チェック
        /// </summary>
        public float Progress()
            => (dynamic)_value == 0 ? 0 : (dynamic)_value / (dynamic)_maxValue;
    }
}
