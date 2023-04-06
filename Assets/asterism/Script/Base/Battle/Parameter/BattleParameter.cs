using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

using UnityEngine;


namespace Asterism.Battle
{
    /// <summary>
    /// パラメータ制御
    /// </summary>
    /// <typeparam name="T"> パラメータの型 </typeparam>
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
        /// <summary> 現在値の取得 </summary>
        public T Value { get => _value; }
        private T _value;
        private T _maxValue;

        /// <summary>
        /// 現在値と最大値が同じ場合
        /// </summary>
        /// <param name="value"> 値 </param>
        public ProgressParameter(T value)
        {
            _value = value;
            _maxValue = value;
        }

        /// <summary>
        /// 現在値と最大値が違う場合
        /// </summary>
        /// <param name="current"> 現在値 </param>
        /// <param name="max"> 最大値 </param>
        public ProgressParameter(T current, T max)
        {
            _value = current;
            _maxValue = max;
        }

        /// <summary>
        /// 現在の数値から指定の数値を足す
        /// </summary>
        /// <param name="value"> 足す値 </param>
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
        /// 現在の数値から指定の数値を引く
        /// </summary>
        /// <param name="value"> 引く値 </param>
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
        /// 最大値にする
        /// </summary>
        public void SetMax()
        {
            _value = _maxValue;
        }

        /// <summary>
        /// 最小値にする
        /// </summary>
        public void SetMin()
        {
            _value = (T)(object)0;
        }

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
