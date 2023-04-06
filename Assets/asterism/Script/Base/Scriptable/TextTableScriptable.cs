using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asterism.Scriptable
{
    using Common;

    [CreateAssetMenu(fileName = "textData", menuName = "Asterism/ScriptableObjects/TextData")]
    public class TextTableScriptable : ScriptableObject
    {
        public CountryCode CountryCode = 0;
        [System.Serializable]
        public struct TextData
        {
            public string key;
            public string Value;
        }

        public List<TextData> Data;
    }

    
}
