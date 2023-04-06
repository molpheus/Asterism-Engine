using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asterism.Battle.Turn
{
    public class TurnBattleUI : MonoBehaviour
    {
        public bool IsView { get; private set; }
        public bool IsReturn { get; private set; }

        public IActionData actionData { get; protected set; }

        public void View()
        {
            IsView = true;
            IsReturn = false;
        }
    }
}
