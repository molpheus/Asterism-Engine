using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asterism.Battle.Turn
{
    public class TurnBattleSystem<T, U> : StatusController<T, U>
        where T : TurnBattleData
        where U : TurnBattleData
    {
        protected override MainStatus status => MainStatus.Main;

        protected int _turnCnt;

        protected override async UniTask Action()
        {

            // ���I
            var lotList = await Lottery(currentObject.PlayerParameters, currentObject.EnemyParameters);

            _turnCnt = 0;
            int currentId = 0;
            do {
                _turnCnt++;
                // �^�[���J�n����
                await StartTurn(lotList[currentId]);

                // �s������
                var actionData = lotList[currentId].IsCPU
                    ? await CPUTurn(lotList[currentId], lotList)
                    : await UserTurn(lotList[currentId], lotList);

                if (actionData.IsForceExit) {
                    break;
                }

                // �s�����ʌv�Z
                lotList = await Calculation(actionData, lotList[currentId], lotList);

                // �^�[����i�߂�
                var prevId = currentId;
                currentId++;
                if (lotList.Count < currentId) {
                    currentId = 0;
                }
                // �^�[���I����
                await EndTurn(lotList[prevId], lotList[currentId]);

            } while (!lotList.Any(p => p.HP.Value == 0));

            await base.Action();
        }

        /// <summary>
        /// �퓬���Ԓ��I
        /// </summary>
        /// <param name="players"> �v���C���[���X�g </param>
        /// <param name="enemys"> �G�l�~�[���X�g </param>
        /// <returns> ���I���� </returns>
        protected virtual async UniTask<List<IParameter>> Lottery(IParameter[] players, IParameter[] enemys) { await UniTask.CompletedTask; return new List<IParameter>(); }

        /// <summary>
        /// �^�[���J�n��
        /// </summary>
        /// <param name="current"> ���̃^�[���̃��C���L�����N�^ </param>
        /// <returns> �Ȃ� </returns>
        protected virtual async UniTask StartTurn(IParameter current) { await UniTask.CompletedTask; }

        /// <summary>
        /// CPU�^�[���̓��쏈��
        /// </summary>
        /// <param name="current"> ���̃^�[���̃��C���L�����N�^ </param>
        /// <param name="lotList"> ���I���e </param>
        /// <returns> �I���A�N�V������� </returns>
        protected virtual async UniTask<IActionData> CPUTurn(IParameter current, List<IParameter> lotList) { await UniTask.CompletedTask; return default(IActionData); }
        /// <summary>
        /// �v���C���[�^�[���̓��쏈��
        /// </summary>
        /// <param name="current"> ���̃^�[���̃��C���L�����N�^ </param>
        /// <param name="lotList"> ���I���e </param>
        /// <returns> �I���A�N�V������� </returns>
        protected virtual async UniTask<IActionData> UserTurn(IParameter current, List<IParameter> lotList) { await UniTask.CompletedTask; return default(IActionData); }

        /// <summary>
        /// �A�N�V�������ʂɍ��킹�����ʂ̌v�Z
        /// </summary>
        /// <param name="actionData"> �A�N�V������� </param>
        /// <param name="current"> ���̃A�N�V�����������L�����N�^ </param>
        /// <param name="lotList"> ���I���e </param>
        /// <returns> ���I���e�̃f�[�^�X�V���� </returns>
        protected virtual async UniTask<List<IParameter>> Calculation(IActionData actionData, IParameter current,List<IParameter> lotList) { await UniTask.CompletedTask; return lotList; }

        /// <summary>
        /// �^�[���I����
        /// </summary>
        /// <param name="prev"> ���̃^�[���̃L�����N�^ </param>
        /// <param name="next"> ���̃^�[���̃L�����N�^ </param>
        /// <returns> �Ȃ� </returns>
        protected virtual async UniTask EndTurn(IParameter prev, IParameter next) { await UniTask.CompletedTask; }
    }
}
