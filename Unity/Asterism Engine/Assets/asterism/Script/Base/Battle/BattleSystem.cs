using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Asterism.Battle
{
    public abstract class BattleSystem
    {
        /// <summary> シングルトン </summary>
        public static BattleSystem I { get; private set; } = null;

        /// <summary> 現在のステータス </summary>
        public MainStatus mainStatus { get; private set; }

        /// <summary> コントローラリスト </summary>
        private Dictionary<MainStatus, StatusControllerBase> _controllerList = new Dictionary<MainStatus, StatusControllerBase>();

        private void Awake ()
        {
            if (I == null) {
                I = this;
            }
        }

        private void OnDestroy()
        {
            if (I == this) {

            }
        }

        /// <summary>
        /// 登録全解除
        /// </summary>
        public void AllClearController()
        {
            foreach (var str in Enum.GetNames(typeof(MainStatus))) {
                var e = (MainStatus)Enum.Parse(typeof(MainStatus), str);
                ClearController(e);
            }

            _controllerList.Clear();
        }

        /// <summary>
        /// 登録解除
        /// </summary>
        /// <param name="status"></param>
        public void ClearController(MainStatus status)
        {
            if (_controllerList.ContainsKey(status)) {
                _controllerList[status].Close();
                _controllerList.Remove(status);
            }
        }

        /// <summary>
        /// コントローラ情報登録
        /// </summary>
        /// <param name="status"></param>
        /// <param name="controller"></param>
        public void SetController(MainStatus status, StatusControllerBase controller)
        {
            if (_controllerList.ContainsKey(status)) {
                _controllerList[status] = controller;
            }
            else {
                _controllerList.Add(status, controller);
            }
        }

        /// <summary>
        /// 開始
        /// </summary>
        /// <returns></returns>
        public async UniTask Play()
        {
            mainStatus = MainStatus.Start;
            object obj = null;
            foreach(var str in Enum.GetNames(typeof(MainStatus))) {
                var e = (MainStatus)Enum.Parse(typeof(MainStatus), str);

                if (_controllerList.ContainsKey(e)) {
                    await _controllerList[e].OnStart(obj);
                    await UniTask.WaitWhile(() => !_controllerList[e].isEndAction);
                    obj = _controllerList[e].NextStatusObject();
                }
            }
        }
    }
}
