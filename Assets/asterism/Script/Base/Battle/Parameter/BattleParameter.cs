using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

using UnityEngine;


namespace Asterism.Battle
{
    /// <summary>
    /// �p�����[�^����
    /// </summary>
    /// <typeparam name="T"> �p�����[�^�̌^ </typeparam>
    public class ProgressParameter<T>
    {
        /// <summary> Normalize </summary>
        public float Progress {
            get {
                switch ((object)_value) {
                    case int v:
                        return ((int)(object)_value / (int)(object)_maxValue);

                    case float v:
                        return ((float)(object)_value / (float)(object)_maxValue);

                    default:
                        return 0;
                }
            }
        }
        /// <summary> ���ݒl�̎擾 </summary>
        public T Value { get => _value; }
        private T _value;
        private T _maxValue;

        /// <summary>
        /// ���ݒl�ƍő�l�������ꍇ
        /// </summary>
        /// <param name="value"> �l </param>
        public ProgressParameter(T value)
        {
            _value = value;
            _maxValue = value;
        }

        /// <summary>
        /// ���ݒl�ƍő�l���Ⴄ�ꍇ
        /// </summary>
        /// <param name="current"> ���ݒl </param>
        /// <param name="max"> �ő�l </param>
        public ProgressParameter(T current, T max)
        {
            _value = current;
            _maxValue = max;
        }

        /// <summary>
        /// ���݂̐��l����w��̐��l�𑫂�
        /// </summary>
        /// <param name="value"> �����l </param>
        public void Add(T value)
        {
            switch((object)value) {
                case int v:
                    _value = (T)(object)Mathf.Clamp((int)(object)_value + v, 0, (int)(object)_maxValue);
                break;
                case float v:
                    _value = (T)(object)Mathf.Clamp((float)(object)_value + v, 0, (float)(object)_maxValue);
                break;
            }
        }

        /// <summary>
        /// ���݂̐��l����w��̐��l������
        /// </summary>
        /// <param name="value"> �����l </param>
        public void Sub(T value)
        {
            switch ((object)value) {
                case int v:
                _value = (T)(object)Mathf.Clamp((int)(object)_value - v, 0, (int)(object)_maxValue);
                break;
                case float v:
                _value = (T)(object)Mathf.Clamp((float)(object)_value - v, 0, (float)(object)_maxValue);
                break;
            }
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
            _value = (T)(object)0;
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
    }

    public interface IActionData
    {
        public int ActionType { get; set; }
        public int ActionId { get; set; }
        public bool IsForceExit { get; set; }
        public List<int> targetIdList { get; set; }
    }

    public interface IParameter
    {
        int ID { get; }
        bool IsCPU { get; }
        string Name { get; }

        ProgressParameter<int> LV { get; }

        ProgressParameter<int> HP { get; }
        ProgressParameter<int> MP { get; }
        ProgressParameter<int> SP { get; }
        ProgressParameter<int> EXP { get; }

        ProgressParameter<int> ATK { get; }
        ProgressParameter<int> DEF { get; }
        ProgressParameter<int> SPD { get; }
        ProgressParameter<int> HIT { get; }
        ProgressParameter<int> CRI { get; }
        ProgressParameter<int> LUK { get; }
    }

    //public class CharacterData : IParameter
    //{
    //    public int ID { get; }

    //    public bool IsCPU { get; }

    //    public string Name { get; set; }

    //    public ProgressParameter<int> LV { get; }

    //    public ProgressParameter<int> HP { get; }
    //    public ProgressParameter<int> MP { get; }
    //    public ProgressParameter<int> SP { get; }
    //    public ProgressParameter<int> EXP { get; }

    //    public ProgressParameter<int> ATK { get; }
    //    public ProgressParameter<int> DEF { get; }
    //    public ProgressParameter<int> SPD { get; }
    //    public ProgressParameter<int> HIT { get; }
    //    public ProgressParameter<int> CRI { get; }
    //    public ProgressParameter<int> LUK { get; }

    //    public CharacterData(int id, bool isCpu = false)
    //    {
    //        ID = id;
    //        IsCPU = isCpu;
    //        SetCharacterName("");
    //        LV = new ProgressParameter<int>(1, 99);
    //        HP = new ProgressParameter<int>(0);
    //        MP = new ProgressParameter<int>(0);
    //        ATK = new ProgressParameter<int>(0);
    //        DEF = new ProgressParameter<int>(0);
    //        SPD = new ProgressParameter<int>(0);

    //        EXP = new ProgressParameter<int>(0);
    //    }

    //    public CharacterData(CharacterParameterData.Parameter param, bool isCpu = false)
    //    {
    //        ID = param.id;
    //        IsCPU = isCpu;
    //        SetCharacterName(param.name);
    //        HP = new ProgressParameter<int>(param.currentHP, param.hp);
    //        MP = new ProgressParameter<int>(param.currentMP, param.mp);
    //        ATK = new ProgressParameter<int>(param.atk);
    //        DEF = new ProgressParameter<int>(param.def);
    //        SPD = new ProgressParameter<int>(param.spd);

    //        EXP = new ProgressParameter<int>(param.exp);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="characterName"></param>
    //    public void SetCharacterName(string characterName)
    //    {
    //        Name = characterName;
    //    }

    //    public void Save()
    //    {

    //    }
    //}

    
}
