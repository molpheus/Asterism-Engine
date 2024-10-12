#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN

using System;

namespace Asterism.Windows
{
    public enum MFType : uint
    {
        MF_STRING = 0x0000,
        MF_SEPARATOR = 0x0800,
        MF_POPUP = 0x0010,
        MF_DISABLED= 0x0002,
        MF_CHECKED = 0x0008,
        MF_UNCHECKED = 0x0000

    }

    public interface IContextMenuItem
    {
        public uint uFlags { get; }
        public uint uId { get; }
        public string lpNewItem { get; }
    }

    public abstract class ContextMenuItem
    {
        public uint uFlags;
        public uint uId;
        public string lpNewItem;

        public ContextMenuItem(MFType _flag, uint _uId, string _label)
        {
            uFlags = (uint)_flag;
            uId = _uId;
            lpNewItem = _label;
        }
    }

    /// <summary>
    /// メニュー項目が文字列であることを指定します。
    /// </summary>
    public class ContextMenuItemString : ContextMenuItem
    {
        public ContextMenuItemString(uint _uId, string _label) : base(MFType.MF_STRING, _uId, _label)
        { }
    }

    /// <summary>
    /// 水平分割線を描画します。ポップアップ メニューでのみ使用できます。
    /// その他のパラメーターは無視されます。
    /// </summary>
    public class ContextMenuItemSeparator : ContextMenuItem
    {
        public ContextMenuItemSeparator() : base(MFType.MF_SEPARATOR, 0, "")
        { }
    }

    /// <summary>
    /// チェックボックスを持つメニュー項目を作成します。
    /// </summary>
    public class ContextMenuItemChecked : ContextMenuItem
    {
        public bool isChecked { get; private set; }
        public ContextMenuItemChecked(uint _uId, string _label, bool _isChecked) : base(_isChecked ? MFType.MF_CHECKED : MFType.MF_UNCHECKED, _uId, _label)
        {
            isChecked = _isChecked;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ContextMenuItemDisabled : ContextMenuItem
    {
        public ContextMenuItemDisabled(uint _uId, string _label) : base(MFType.MF_DISABLED, _uId, _label)
        { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ContextMenuItemPopup : ContextMenuItem
    {
        public IntPtr hMenu { get; private set; }
        public ContextMenuItem[] items { get; }
        public ContextMenuItemPopup(string _label, ContextMenuItem[] items) : base(MFType.MF_POPUP, 0, _label)
        {
            this.items = items;
        }

        public void SetMenu(IntPtr hMenu)
        {
            this.hMenu = hMenu;
        }
    }
}

#endif
