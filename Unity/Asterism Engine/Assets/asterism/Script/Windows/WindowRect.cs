using System;
using System.Runtime.InteropServices;

using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || PLATFORM_STANDALONE_WIN

namespace Asterism.Windows
{
    public class WindowRect : MonoBehaviour
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int windowX => left;
            public int windowY => top;
            public int width => right - left;
            public int height => bottom - top;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public static RECT GetWindowRect()
        {
            var hWnd = GetForegroundWindow();
            GetWindowRect(hWnd, out var rect);
            return rect;
        }
    }
}

#endif
