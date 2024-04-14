using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Asterism.Battle
{
    public abstract class BattleSystem : MonoBehaviour
    {
        /// <summary> �V���O���g�� </summary>
        public static BattleSystem I => _I;
        /// <summary> �V���O���g�� </summary>
        private static BattleSystem _I = null;

        /// <summary> ���݂̃X�e�[�^�X </summary>
        public MainStatus mainStatus { get; private set; }

        /// <summary> �R���g���[�����X�g </summary>
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
        /// �o�^�S����
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
        /// �o�^����
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
        /// �R���g���[�����o�^
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
        /// �J�n
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
