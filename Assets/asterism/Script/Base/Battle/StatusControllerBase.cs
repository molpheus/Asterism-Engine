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
        /// ������
        /// </summary>
        protected virtual void Initialize(object obj) { }
        /// <summary>
        /// �A�N�V����
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask Action() { isEndAction = true; await UniTask.CompletedTask; }
        /// <summary>
        /// ���̃X�e�[�^�X�R���g���[���ɓn�������f�[�^��ԋp����
        /// </summary>
        /// <returns>  </returns>
        public virtual object NextStatusObject() { return null; }
        /// <summary>
        /// ����
        /// </summary>
        public virtual void Close() { }
    }
}
