using System;
using System.Linq;

using UnityEngine;

namespace Asterism.Battle
{
    [CreateAssetMenu(fileName = "CharacterParameterData", menuName = "Battle/ScriptableObject/CharacterParameterData", order = 1)]
    public class CharacterParameterData : ScriptableObject
    {
        [Serializable]
        public class Parameter
        {
            public string name;
            public int id;
            public int currentHP;
            public int hp;
            public int currentMP;
            public int mp;
            public int atk;
            public int def;
            public int spd;
            public int exp;
        }

        public Parameter[] param;

        public Parameter GetParameter(int id)
        {
            return param.Where(p => p.id == id).FirstOrDefault();
        }
    }
}
