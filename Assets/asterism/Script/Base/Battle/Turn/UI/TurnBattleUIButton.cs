using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asterism.Battle.Turn.UI
{
    public class TurnBattleUIButton : Button
    {
        //public IActionData data;
        //public Action<ActionData> act;
        protected override void Awake()
        {
            onClick.AddListener(OnClick);
            base.Awake();
        }

        private void OnClick()
        {
            //act?.Invoke(data);
        }

        public void SetText(string str)
        {
            TMPro.TextMeshProUGUI tmProLabel;
            if ((tmProLabel = transform.GetComponentInChildren<TMPro.TextMeshProUGUI>()) != null) {
                tmProLabel.text = str;
                return;
            }

            Text textLabel;
            if ((textLabel = transform.GetComponentInChildren<Text>()) != null) {
                textLabel.text = str;
            }
        }
    }
}
