using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Asterism.Engine
{
    using Common;
    using Scriptable;

#pragma warning disable CS0234 // �^�܂��͖��O��Ԃ̖��O 'Search' �����O��� 'UnityEditor' �ɑ��݂��܂��� (�A�Z���u���Q�Ƃ����邱�Ƃ��m�F���Ă�������)
    using UnityEditor.Search;
#pragma warning restore CS0234 // �^�܂��͖��O��Ԃ̖��O 'Search' �����O��� 'UnityEditor' �ɑ��݂��܂��� (�A�Z���u���Q�Ƃ����邱�Ƃ��m�F���Ă�������)

    public abstract class TextManager : EngineBase<TextManager>
    {
        public enum TextLoadType
        {
            Resource,   // ���\�[�X����ǂݍ���
            Addressable // Addressable����ǂݍ���
        }

        

        protected abstract string LoadAddressableTag { get; }
        protected abstract TextLoadType LoadType { get; }

        protected List<TextTableScriptable> tableList;

        [System.NonSerialized]
        public LoadState StateLoaded = LoadState.None;
        /// <summary> �|�󃊃X�g�ǂݍ��� </summary>
        public bool IsLoaded { get { return StateLoaded != LoadState.None; } }
        /// <summary> �I�𒆂̌��� </summary>
        public CountryCode CurrentCountry = CountryCode.JPN;

        private void Awake()
        {
            StateLoaded = LoadState.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadCountry"></param>
        public async UniTask Initialized()
        {
            if (IsLoaded) return;

            StateLoaded = LoadState.Load;
            if (LoadType == TextLoadType.Addressable) {
                await ResourceReciver.LoadTagAsync<TextTableScriptable>(
                    LoadAddressableTag,
                    SettableData
                );
            }
            else if (LoadType == TextLoadType.Resource) {
                SettableData(
                    new List<TextTableScriptable>((TextTableScriptable[])Resources.LoadAll(LoadAddressableTag, typeof(TextTableScriptable)))
                );
                
            }
        }

        private void SettableData(List<TextTableScriptable> tableList)
        {
            this.tableList = tableList;
            StateLoaded = LoadState.End;
        }

        public async UniTask<string> CurrentTextAsync(string key)
        {
            string value = "";
            if (StateLoaded != LoadState.End) {
                await Initialized();
                await UniTask.WaitWhile( () => StateLoaded != LoadState.End);
            }

            var current = tableList.Where(p => p.CountryCode == CurrentCountry).FirstOrDefault();
            if (current != default(TextTableScriptable) && current.Data.Any( p => p.key == key)) {
                value = current.Data.Where(p => p.key == key).First().Value;

            }
            else {
                value = $"not set key, {key} :Check language file data {CurrentCountry}";
                Debug.LogWarning(value);
            }

            return value;
        }

        public void Clear()
        {
            StateLoaded = LoadState.None;
        }
    }
}
