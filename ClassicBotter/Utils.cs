using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace CrystalBot
{
    internal static class Utils
    {
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out Point lpPoint);
        
        const int VK_ESCAPE = 0x1B;
        const int VK_RETURN = 0x0D;
        public const int F1 = 0x70;
        public const int F2 = 0x71;
        public const int F3 = 0x72;
        public const int F4 = 0x73;
        public const int F5 = 0x74;
        public const int F6 = 0x75;
        public const int F7 = 0x76;
        public const int F8 = 0x77;
        public const int F9 = 0x78;
        public const int F10 = 0x79;
        public const int F11 = 0x7A;
        public const int F12 = 0x7B;
        const int VK_LCONTROL = 162;
        const int VK_RCONTROL = 163;
        const int VK_LSHIFT = 160;
        const int VK_RSHIFT = 161;
        public const int WM_CHAR = 0x0102;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SETTEXT = 0x0C;
        public const int CONTROL = 0x11;
        public const int SHIFT = 0x10;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;

        const int VK_LBUTTON = 0x01;
        const int VK_RBUTTON = 0x02;

        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;

        private static void ClearMouseClick()
        {
            int LParam = WinApi.MakeLParam(0,0);
            //WinApi.mouse_event(0x0010, 0, 0, 0, 0); //release
            //WinApi.mouse_event(0x0004, 0, 0, 0, 0); //release
            PostMessage(Memory.process.MainWindowHandle, 0x202, 0, LParam);
            PostMessage(Memory.process.MainWindowHandle, 0x205, 0, LParam);
        }

        public static void MakeRightClick(int x, int y)
        {
            ClearMouseClick();
            int LParam = WinApi.MakeLParam(x, y -= 20);
            //SendMessage(Memory.process.MainWindowHandle, 0x0008, x, y);
            //SendMessage(Memory.process.MainWindowHandle, 0x0010, x, y);
            PostMessage(Memory.process.MainWindowHandle, 0x204, 0, LParam);
            PostMessage(Memory.process.MainWindowHandle, 0x205, 0, LParam);
        }

        public static void MakeLeftClick(int x, int y)
        {
            ClearMouseClick();
            int LParam = WinApi.MakeLParam(x, y -= 20);
            PostMessage(Memory.process.MainWindowHandle, 0x201, 0, LParam);
            PostMessage(Memory.process.MainWindowHandle, 0x202, 0, LParam);
            //SendMessage(Memory.process.MainWindowHandle, 0x0002, x, y);
            //SendMessage(Memory.process.MainWindowHandle, 0x0004, x, y);
        }

        //public static void DragMouse(Point from, Point to)
        //{
        //    //ClearMouseClick();
        //    int LParam = WinApi.MakeLParam(from.X, from.Y -= 20);
        //    int LParam2 = WinApi.MakeLParam(to.X, to.Y-=20);
        //    Point PreCursor = new Point();
        //    GetCursorPos(out PreCursor);
        //    PostMessage(Memory.process.MainWindowHandle, 0x201, 0, LParam);
        //    SetCursorPos(from.X, from.Y);
            
        //    Thread.Sleep(125);
        //    //PostMessage(Memory.process.MainWindowHandle, 0x200, 0, LParam2);
        //    SetCursorPos(to.X, to.Y);
        //    PostMessage(Memory.process.MainWindowHandle, 0x202, 0, LParam2);
        //    //SetCursorPos(to.X, to.Y);
        //    SetCursorPos(PreCursor.X, PreCursor.Y);
        //}

        public static void DragMouse(Point from, Point to)
        {
            int LParam = WinApi.MakeLParam(from.X, from.Y-=20);
            int LParam2 = WinApi.MakeLParam(to.X, to.Y-=20);
            Point PreCursor = new Point();
            GetCursorPos(out PreCursor);

            PostMessage(Memory.process.MainWindowHandle, 0x201, 0, LParam);

            SetCursorPos(PreCursor.X + 1, PreCursor.Y + 1);

            PostMessage(Memory.process.MainWindowHandle, 0x200, 0, LParam2);

            Thread.Sleep(200);

            PostMessage(Memory.process.MainWindowHandle, 0x202, 0, LParam2);

            GetCursorPos(out PreCursor);

            SetCursorPos(PreCursor.X - 1, PreCursor.Y - 1);

        }







        //        POINT cur;
        //GetCursorPos(&cur);
        //PostMessage(getHWND(), WM_LBUTTONDOWN, 0, MAKELPARAM(frompos.x, frompos.y));
        //SetCursorPos(cur.x+1, cur.y+1);
        //PostMessage(getHWND(), WM_MOUSEMOVE, 0, MAKELPARAM(topos.x, topos.y));
        //Sleep(random_range(500, 1000));
        //PostMessage(getHWND(), WM_LBUTTONUP, 0, MAKELPARAM(topos.x, topos.y));
        //GetCursorPos(&cur);
        //SetCursorPos(cur.x-1, cur.y-1);

        public static void DragStart(int x, int y)
        {
            int LParam = WinApi.MakeLParam(x, y);
            //PostMessage(Memory.process.MainWindowHandle, 0x201, 0, LParam);
            SendMessage(Memory.process.MainWindowHandle, 0x0002, x, y);
        }

        public static void DragEnd(int x, int y)
        {
            int LParam = WinApi.MakeLParam(x, y);
            //PostMessage(Memory.process.MainWindowHandle, 0x202, 0, LParam);
            SendMessage(Memory.process.MainWindowHandle, 0x0004, x, y);
        }

        internal static void SendKeys(string s)
        {
            IntPtr hWnd = Memory.process.MainWindowHandle;
            switch (s.ToUpper())
            {
                case "ESCAPE":
                    PostMessage(hWnd, WM_KEYDOWN, VK_ESCAPE, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_ESCAPE, 0);
                    break;
                case "ENTER":
                    PostMessage(hWnd, WM_KEYDOWN, VK_RETURN, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_RETURN, 0);
                    break;
                case "UP":
                    PostMessage(hWnd, WM_KEYDOWN, VK_UP, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_UP, 0);
                    break;
                case "LEFT":
                    PostMessage(hWnd, WM_KEYDOWN, VK_LEFT, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_LEFT, 0);
                    break;
                case "DOWN":
                    PostMessage(hWnd, WM_KEYDOWN, VK_DOWN, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_DOWN, 0);
                    break;
                case "RIGHT":
                    PostMessage(hWnd, WM_KEYDOWN, VK_RIGHT, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_RIGHT, 0);
                    break;
                case "DOWNLEFT":
                    PostMessage(hWnd, WM_KEYDOWN, 35, 0);
                    PostMessage(hWnd, WM_KEYUP, 35, 0);
                    break;
                case "DOWNRIGHT":
                    PostMessage(hWnd, WM_KEYDOWN, 34, 0);
                    PostMessage(hWnd, WM_KEYUP, 34, 0);
                    break;
                case "UPLEFT":
                    PostMessage(hWnd, WM_KEYDOWN, 36, 0);
                    PostMessage(hWnd, WM_KEYUP, 36, 0);
                    break;
                case "UPRIGHT":
                    PostMessage(hWnd, WM_KEYDOWN, 33, 0);
                    PostMessage(hWnd, WM_KEYUP, 33, 0);
                    break;
                case "F1":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F1, 0);
                    PostMessage(hWnd, WM_KEYUP, F1, 0);
                    break;
                case "F2":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F2, 0);
                    PostMessage(hWnd, WM_KEYUP, F2, 0);
                    break;
                case "F3":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F3, 0);
                    PostMessage(hWnd, WM_KEYUP, F3, 0);
                    break;
                case "F4":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F4, 0);
                    PostMessage(hWnd, WM_KEYUP, F4, 0);
                    break;
                case "F5":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F5, 0);
                    PostMessage(hWnd, WM_KEYUP, F5, 0);
                    break;
                case "F6":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F6, 0);
                    PostMessage(hWnd, WM_KEYUP, F6, 0);
                    break;
                case "F7":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F7, 0);
                    PostMessage(hWnd, WM_KEYUP, F7, 0);
                    break;
                case "F8":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F8, 0);
                    PostMessage(hWnd, WM_KEYUP, F8, 0);
                    break;
                case "F9":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F9, 0);
                    PostMessage(hWnd, WM_KEYUP, F9, 0);
                    break;
                case "F10":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F10, 0);
                    PostMessage(hWnd, WM_KEYUP, F10, 0);
                    break;
                case "F11":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F11, 0);
                    PostMessage(hWnd, WM_KEYUP, F11, 0);
                    break;
                case "F12":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F12, 0);
                    PostMessage(hWnd, WM_KEYUP, F12, 0);
                    break;
                case "SHIFT+F1":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F1, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F1, 0);
                    break;
                case "SHIFT+F2":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F2, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F2, 0);
                    break;
                case "SHIFT+F3":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F3, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F3, 0);
                    break;
                case "SHIFT+F4":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F4, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F4, 0);
                    break;
                case "SHIFT+F5":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F5, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F5, 0);
                    break;
                case "SHIFT+F6":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F6, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F6, 0);
                    break;
                case "SHIFT+F7":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F7, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F7, 0);
                    break;
                case "SHIFT+F8":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F8, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F8, 0);
                    break;
                case "SHIFT+F9":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F9, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F9, 0);
                    break;
                case "SHIFT+F10":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F10, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F10, 0);
                    break;
                case "SHIFT+F11":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F11, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F11, 0);
                    break;
                case "SHIFT+F12":
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F12, 0);
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYUP, F12, 0);
                    break;
                case "CTRL+F1":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F1, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F1, 0);
                    break;
                case "CTRL+F2":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F2, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F2, 0);
                    break;
                case "CTRL+F3":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F3, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F3, 0);
                    break;
                case "CTRL+F4":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F4, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F4, 0);
                    break;
                case "CTRL+F5":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F5, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F5, 0);
                    break;
                case "CTRL+F6":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F6, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F6, 0);
                    break;
                case "CTRL+F7":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F7, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F7, 0);
                    break;
                case "CTRL+F8":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F8, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F8, 0);
                    break;
                case "CTRL+F9":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F9, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F9, 0);
                    break;
                case "CTRL+F10":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F10, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F10, 0);
                    break;
                case "CTRL+F11":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F11, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F11, 0);
                    break;
                case "CTRL+F12":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, F12, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYUP, F12, 0);
                    break;
                case "CTRL+UP":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, VK_UP, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_UP, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    break;
                case "CTRL+LEFT":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, VK_LEFT, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_LEFT, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    break;
                case "CTRL+DOWN":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, VK_DOWN, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_DOWN, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    break;
                case "CTRL+RIGHT":
                    PostMessage(hWnd, WM_KEYUP, SHIFT, 0);
                    PostMessage(hWnd, WM_KEYDOWN, CONTROL, 0);
                    PostMessage(hWnd, WM_KEYDOWN, VK_RIGHT, 0);
                    PostMessage(hWnd, WM_KEYUP, VK_RIGHT, 0);
                    PostMessage(hWnd, WM_KEYUP, CONTROL, 0);
                    break;
               /* default:
                    foreach (char c in s.ToCharArray())
                    {
                        PostMessage(hWnd, WM_CHAR, (int)c, 0);
                    }
                    Utils.SendKeys("ENTER");
                    break; */
            }
        }
    }
}
