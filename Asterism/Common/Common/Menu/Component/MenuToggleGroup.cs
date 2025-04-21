using System;
using System.Collections.Generic;
using System.Linq;

namespace Asterism.Common.Menu.Component
{
    public partial class MenuToggleGroup : MenuBase
    {
        private List<MenuToggle> _toggles;
        public List<MenuToggle> toggles => _toggles;
        private bool _isMulti;
        private Action<int, bool> _onChange;

        public delegate void ChangeToggle(int index, bool isToggle);

        public MenuToggleGroup(string text, bool iaMulti, Action<int, bool> onChange, params MenuToggle[] toggles) : base(_text: text)
        {
            _isMulti = iaMulti;
            _onChange = onChange;
            foreach (var item in toggles)
                AddToggle(item);

        }

        public void AddToggle(MenuToggle toggle)
        {
            if (_toggles == null)
                return;

            toggle.onChangeIndex = OnToggleChange;

            _toggles.Add(toggle);
        }

        public void RemoveToggle(MenuToggle toggle)
        {
            if (_toggles == null)
                return;

            _toggles.Remove(toggle);
        }

        private void OnToggleChange(int index, bool isToggle)
        {
            if (isToggle)
            {
                var list = _toggles.Where(x => x.isOn && x.index != index);
                foreach (var item in list)
                {
                    item.OnToggleChange(false);
                }
            }
        }
    }
}
