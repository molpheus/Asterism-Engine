using UnityEngine.UI;

namespace Asterism.Battle.Turn.UI
{
    public class TurnBattleUIButton : Button
    {
        protected override void Awake()
        {
            onClick.AddListener(OnClick);
            base.Awake();
        }

        private void OnClick()
        { }

        public void SetText(string str)
        {
            TMPro.TextMeshProUGUI tmProLabel;
            if ((tmProLabel = transform.GetComponentInChildren<TMPro.TextMeshProUGUI>()) != null)
            {
                tmProLabel.text = str;
                return;
            }

            Text textLabel;
            if ((textLabel = transform.GetComponentInChildren<Text>()) != null)
            {
                textLabel.text = str;
            }
        }
    }
}
