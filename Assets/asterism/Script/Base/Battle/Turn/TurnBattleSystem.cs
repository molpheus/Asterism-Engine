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

            // 抽選
            var lotList = await Lottery(currentObject.PlayerParameters, currentObject.EnemyParameters);

            _turnCnt = 0;
            int currentId = 0;
            do {
                _turnCnt++;
                // ターン開始直後
                await StartTurn(lotList[currentId]);

                // 行動決定
                var actionData = lotList[currentId].IsCPU
                    ? await CPUTurn(lotList[currentId], lotList)
                    : await UserTurn(lotList[currentId], lotList);

                if (actionData.IsForceExit) {
                    break;
                }

                // 行動結果計算
                lotList = await Calculation(actionData, lotList[currentId], lotList);

                // ターンを進める
                var prevId = currentId;
                currentId++;
                if (lotList.Count < currentId) {
                    currentId = 0;
                }
                // ターン終了時
                await EndTurn(lotList[prevId], lotList[currentId]);

            } while (!lotList.Any(p => p.HP.Value == 0));

            await base.Action();
        }

        /// <summary>
        /// 戦闘順番抽選
        /// </summary>
        /// <param name="players"> プレイヤーリスト </param>
        /// <param name="enemys"> エネミーリスト </param>
        /// <returns> 抽選結果 </returns>
        protected virtual async UniTask<List<IParameter>> Lottery(IParameter[] players, IParameter[] enemys) { await UniTask.CompletedTask; return new List<IParameter>(); }

        /// <summary>
        /// ターン開始時
        /// </summary>
        /// <param name="current"> そのターンのメインキャラクタ </param>
        /// <returns> なし </returns>
        protected virtual async UniTask StartTurn(IParameter current) { await UniTask.CompletedTask; }

        /// <summary>
        /// CPUターンの動作処理
        /// </summary>
        /// <param name="current"> そのターンのメインキャラクタ </param>
        /// <param name="lotList"> 抽選内容 </param>
        /// <returns> 選択アクション情報 </returns>
        protected virtual async UniTask<IActionData> CPUTurn(IParameter current, List<IParameter> lotList) { await UniTask.CompletedTask; return default(IActionData); }
        /// <summary>
        /// プレイヤーターンの動作処理
        /// </summary>
        /// <param name="current"> そのターンのメインキャラクタ </param>
        /// <param name="lotList"> 抽選内容 </param>
        /// <returns> 選択アクション情報 </returns>
        protected virtual async UniTask<IActionData> UserTurn(IParameter current, List<IParameter> lotList) { await UniTask.CompletedTask; return default(IActionData); }

        /// <summary>
        /// アクション結果に合わせた結果の計算
        /// </summary>
        /// <param name="actionData"> アクション情報 </param>
        /// <param name="current"> そのアクションをしたキャラクタ </param>
        /// <param name="lotList"> 抽選内容 </param>
        /// <returns> 抽選内容のデータ更新結果 </returns>
        protected virtual async UniTask<List<IParameter>> Calculation(IActionData actionData, IParameter current,List<IParameter> lotList) { await UniTask.CompletedTask; return lotList; }

        /// <summary>
        /// ターン終了時
        /// </summary>
        /// <param name="prev"> そのターンのキャラクタ </param>
        /// <param name="next"> 次のターンのキャラクタ </param>
        /// <returns> なし </returns>
        protected virtual async UniTask EndTurn(IParameter prev, IParameter next) { await UniTask.CompletedTask; }
    }
}
