using System;

namespace Asterism.Common.Menu.Component
{
    public partial class MenuToggle : MenuBase
    {
        public bool isOn { get; protected set; }
        public Action<bool> onChange { get; set; } = null;

        public int index { get; }
        public Action<int, bool> onChangeIndex { get; set; } = null;

        public MenuToggle(string text, bool isOn, Action<bool> onChange) : base(_text: text)
        {
            this.isOn = isOn;
            this.onChange = onChange;
            this.onClick = OnClick;
        }

        public MenuToggle(string text, bool isOn, int index, Action<bool> onChange) : base(_text: text)
        {
            this.isOn = isOn;
            this.index = index;
            this.onChange = onChange;
            this.onClick = OnClick;
        }


        private void OnClick()
        {
            isOn = !isOn;
            onChange?.Invoke(isOn);
            onChangeIndex?.Invoke(index, isOn);
        }

        public void OnToggleChange(bool isToggle)
        {
            isOn = isToggle;
            onChange?.Invoke(isOn);
        }
    }
}
