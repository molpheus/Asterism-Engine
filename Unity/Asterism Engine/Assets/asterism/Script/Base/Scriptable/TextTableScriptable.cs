using System;
using System.Collections.Generic;

using UnityEngine;

using Asterism.Common;

namespace Asterism.Scriptable
{
    [CreateAssetMenu(fileName = "textData", menuName = "Asterism/ScriptableObjects/TextData")]
    public class TextTableScriptable : ScriptableObject
    {
        public CountryCode CountryCode = 0;
        [Serializable]
        public struct TextData
        {
            public string key;
            public string Value;
        }

        public List<TextData> Data;
    }


}
