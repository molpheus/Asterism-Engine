using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asterism.Battle
{
    /// <summary>
    /// ゲームメインのステータス
    /// </summary>
    public enum MainStatus
    {
        None,

        /// <summary> 開始 </summary>
        Start,
        /// <summary> オープニング </summary>
        Opning,
        /// <summary> バトル開始直前 </summary>
        BattleStart,
        /// <summary> メイン部分 </summary>
        Main,
        /// <summary> バトル終了直後 </summary>
        BattleEnd,
        /// <summary> リザルト </summary>
        Result,
        /// <summary> 終了 </summary>
        End,
    }
}
