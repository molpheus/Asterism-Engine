using System;

namespace Asterism.Common.Menu.Component
{
    public abstract partial class MenuBase
    {
        public MenuBase(string _text = "", string _saveKey = "", Action _onClick = null)
        {
            this.text = _text;
            this.saveKey = _saveKey;
            this.onClick = _onClick;
        }

        public string text { get; protected set; }
        public string saveKey { get; protected set; }
        public Action onClick { get; protected set; }
    }
}
