using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Asterism.Battle
{
    public abstract class StatusControllerBase : MonoBehaviour
    {
        protected virtual MainStatus status { get; }
        public bool isEndAction { get; set; }

        private void Start()
        {
            BattleSystem.I.SetController(status, this);
        }

        public async UniTask OnStart(object obj)
        {
            isEndAction = false;
            Initialize(obj);
            await Action();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Initialize(object obj) { }
        /// <summary>
        /// アクション
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask Action() { isEndAction = true; await UniTask.CompletedTask; }
        /// <summary>
        /// 次のステータスコントローラに渡したいデータを返却する
        /// </summary>
        /// <returns>  </returns>
        public virtual object NextStatusObject() { return null; }
        /// <summary>
        /// 閉じる
        /// </summary>
        public virtual void Close() { }
    }
}
