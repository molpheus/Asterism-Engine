using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Asterism.Battle
{
    public abstract class BattleSystem : MonoBehaviour
    {
        /// <summary> シングルトン </summary>
        public static BattleSystem I => _I;
        /// <summary> シングルトン </summary>
        private static BattleSystem _I = null;

        /// <summary> 現在のステータス </summary>
        public MainStatus mainStatus { get; private set; }

        /// <summary> コントローラリスト </summary>
        private Dictionary<MainStatus, StatusControllerBase> controllerList = new Dictionary<MainStatus, StatusControllerBase>();

        private void Awake ()
        {
            if (_I == null) {
                _I = this;
            }
        }

        private void OnDestroy()
        {
            if (_I == this) {

            }
        }

        /// <summary>
        /// 登録全解除
        /// </summary>
        public void AllClearController()
        {
            foreach (var str in System.Enum.GetNames(typeof(MainStatus))) {
                var e = (MainStatus)System.Enum.Parse(typeof(MainStatus), str);
                ClearController(e);
            }

            controllerList.Clear();
        }

        /// <summary>
        /// 登録解除
        /// </summary>
        /// <param name="status"></param>
        public void ClearController(MainStatus status)
        {
            if (controllerList.ContainsKey(status)) {
                controllerList[status].Close();
                controllerList.Remove(status);
            }
        }

        /// <summary>
        /// コントローラ情報登録
        /// </summary>
        /// <param name="status"></param>
        /// <param name="controller"></param>
        public void SetController(MainStatus status, StatusControllerBase controller)
        {
            if (controllerList.ContainsKey(status)) {
                controllerList[status] = controller;
            }
            else {
                controllerList.Add(status, controller);
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
            foreach(var str in System.Enum.GetNames(typeof(MainStatus))) {
                var e = (MainStatus)System.Enum.Parse(typeof(MainStatus), str);

                if (controllerList.ContainsKey(e)) {
                    await controllerList[e].OnStart(obj);
                    await UniTask.WaitWhile(() => !controllerList[e].isEndAction);
                    obj = controllerList[e].NextStatusObject();
                }
            }
        }
    }
}
