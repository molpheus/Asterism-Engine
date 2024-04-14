using System;

namespace Asterism
{
    /// <summary>
    /// �p�����[�^����
    /// </summary>
    public partial class NumVariable<T>
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        /// <summary> ���ݒl�̎擾 </summary>
        public T Value => _value;
        private T _value;
        /// <summary> �ő�l�̎擾 </summary>
        public T MaxValue => _maxValue;
        private T _maxValue;

        /// <summary>
        /// ���ݒl�ƍő�l�������ꍇ
        /// </summary>
        /// <param name="value"> �l </param>
        public NumVariable(T value)
        {
            _value = value;
            _maxValue = value;
        }

        /// <summary>
        /// ���ݒl�ƍő�l���Ⴄ�ꍇ
        /// </summary>
        /// <param name="current"> ���ݒl </param>
        /// <param name="max"> �ő�l </param>
        public NumVariable(T current, T max)
        {
            _value = current;
            _maxValue = max;
        }

        /// <summary>
        /// ���݂̐��l����w��̐��l�𑫂�
        /// </summary>
        /// <param name="value"> �����l </param>
        public T Add(T value, bool isReturnOnly = false)
        {
            T tmp = (dynamic)_value + (dynamic)value;
            if (tmp < (dynamic)0) tmp = (dynamic)0;
            if (tmp > (dynamic)_maxValue) tmp = (dynamic)_maxValue;
            if (!isReturnOnly) _value = tmp;
            return tmp;
        }

        /// <summary>
        /// ���݂̐��l����w��̐��l������
        /// </summary>
        /// <param name="value"> �����l </param>
        public T Sub(T value, bool isReturnOnly = false)
        {
            T tmp = (dynamic)_value - (dynamic)value;
            if (tmp < (dynamic)0) tmp = (dynamic)0;
            if (tmp > (dynamic)_maxValue) tmp = (dynamic)_maxValue;
            if (!isReturnOnly) _value = tmp;
            return tmp;
        }

        /// <summary>
        /// �ő�l�ɂ���
        /// </summary>
        public void SetMax()
        {
            _value = _maxValue;
        }

        /// <summary>
        /// �ŏ��l�ɂ���
        /// </summary>
        public void SetMin()
        {
            _value = (dynamic)0;
        }

        /// <summary>
        /// �ő�l��ݒ肷��
        /// </summary>
        /// <param name="max"> �V�����ő�l </param>
        /// <param name="isMax"> ���݂̒l���ő�l�ɂ��邩 </param>
        public void SetMaxValue(T max, bool isMax)
        {
            _maxValue = max;

            if (isMax) SetMax();
        }

        /// <summary>
        /// ���ݒl�ƍő�l�̍ő�`�F�b�N
        /// </summary>
        public float Progress()
        {
            return (dynamic)_value == 0 ? 0 : (dynamic)_value / (dynamic)_maxValue;
        }
    }
}
