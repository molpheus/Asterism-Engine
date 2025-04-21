using System;

namespace Asterism.Common.Menu.Component
{
    public partial class MenuLabel(string text, Action onClick) : MenuBase(_text: text, _onClick: onClick)
    { }
}
