using System.Runtime.InteropServices;
using System;

using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN

namespace Asterism.Windows
{
    public class ContextMenu : MonoBehaviour
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool AppendMenu(IntPtr hMenu, uint uFlags, IntPtr uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="items"></param>
        public uint Show(Vector2 screenPoint, ContextMenuItem[] items)
        {
            var hMenu = CreateMenuPtr(items);

            /**
             * フルスクリーン上でのポップアップ座標位置の計算参考
             * Vector2 screenPoint
             * x : mousePosition.x
             * y : Screen.height - mousePosition.y
             * */

            return TrackPopupMenuEx(hMenu, 0x0000, (int)screenPoint.x, (int)screenPoint.y, GetForegroundWindow(), IntPtr.Zero);
        }


        /// <summary>
        /// Menuのポインタを作成する
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private IntPtr CreateMenuPtr(params ContextMenuItem[] items)
        {
            IntPtr hMenu = CreatePopupMenu();

            foreach (var item in items)
            {
                if (item is ContextMenuItemPopup popup)
                {
                    popup.SetMenu(CreateMenuPtr(popup.items));
                    AppendMenu(hMenu, item.uFlags, popup.hMenu, item.lpNewItem);
                }

                AppendMenu(hMenu, item.uFlags, item.uId, item.lpNewItem);
            }

            return hMenu;
        }
    }
}

#endif
