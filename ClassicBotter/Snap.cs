using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot
{
    class WindowSnap
    {

        #region Win32

        private const string PROGRAMMANAGER = "Program Manager";
        private const string RUNDLL = "RunDLL";

        private const uint WM_PAINT = 0x000F;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 0x2;

        private enum ShowWindowEnum { Hide = 0, ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3, Maximize = 3, ShowNormalNoActivate = 4, Show = 5, Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8, Restore = 9, ShowDefault = 10, ForceMinimized = 11 };

        private struct RECT
        {
            int left;
            int top;
            int right;
            int bottom;

            public int Left
            {
                get { return this.left; }
            }

            public int Top
            {
                get { return this.top; }
            }

            public int Width
            {
                get { return right - left; }
            }

            public int Height
            {
                get { return bottom - top; }
            }

            public static implicit operator Rectangle(RECT rect)
            {
                return new Rectangle(rect.left, rect.top, rect.Width, rect.Height);
            }
        }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [DllImport("user32.dll")]
        private static extern uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32")]
        private static extern int GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32")]
        private static extern int PrintWindow(IntPtr hWnd, IntPtr dc, uint flags);

        [DllImport("user32")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("user32")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        private delegate bool EnumWindowsCallbackHandler(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsCallbackHandler lpEnumFunc, IntPtr lParam);

        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int index, int dwNewLong);

        [DllImport("user32")]
        private static extern int SetLayeredWindowAttributes(IntPtr hWnd, byte crey, byte alpha, int flags);

        #region Update for 4th October 2007

        [DllImport("user32")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder name, int maxCount);

        [DllImport("user32")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool reDraw);

        [DllImport("user32.dll")]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public WindowStyles dwStyle;
            public ExtendedWindowStyles dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public static uint GetSize()
            {
                return (uint)Marshal.SizeOf(typeof(WINDOWINFO));
            }
        }

        [Flags]
        private enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,
        }

        #endregion

        #region Update for 10October 2007

        [Flags]
        private enum ExtendedWindowStyles : uint
        {
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST)
        }
        #endregion

        #endregion

        #region Statics

        #region Update for 10 Ocrober 2007

        private static bool forceMDI = true;

        /// <summary>
        /// if is true ,will force the mdi child to be captured completely ,maybe incompatible with some programs
        /// </summary>
        public static bool ForceMDICapturing
        {
            get { return forceMDI; }
            set { forceMDI = value; }
        }
        #endregion

        [ThreadStatic]
        private static bool countMinimizedWindows;

        [ThreadStatic]
        private static bool useSpecialCapturing;

        [ThreadStatic]
        private static WindowSnapCollection windowSnaps;

        [ThreadStatic]
        private static int winLong;

        [ThreadStatic]
        private static bool minAnimateChanged = false;


        private static bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            bool specialCapturing = false;

            if (hWnd == IntPtr.Zero) return false;

            if (!IsWindowVisible(hWnd)) return true;

            if (!countMinimizedWindows)
            {
                if (IsIconic(hWnd)) return true;
            }
            else if (IsIconic(hWnd) && useSpecialCapturing) specialCapturing = true;

            if (GetWindowText(hWnd) == PROGRAMMANAGER) return true;

            windowSnaps.Add(new WindowSnap(hWnd, specialCapturing));

            return true;
        }

        /// <summary>
        /// Get the collection of WindowSnap instances fro all available windows
        /// </summary>
        /// <param name="minimized">Capture a window even it's Minimized</param>
        /// <param name="specialCapturring">use special capturing method to capture minmized windows</param>
        /// <returns>return collections of WindowSnap instances</returns>
        public static WindowSnapCollection GetAllWindows(bool minimized, bool specialCapturring)
        {
            windowSnaps = new WindowSnapCollection();
            countMinimizedWindows = minimized;//set minimized flag capture
            useSpecialCapturing = specialCapturring;//set specialcapturing flag

            EnumWindowsCallbackHandler callback = new EnumWindowsCallbackHandler(EnumWindowsCallback);
            EnumWindows(callback, IntPtr.Zero);

            return new WindowSnapCollection(windowSnaps.ToArray(), true);
        }

        /// <summary>
        /// Get the collection of WindowSnap instances fro all available windows
        /// </summary>
        /// <returns>return collections of WindowSnap instances</returns>
        public static WindowSnapCollection GetAllWindows()
        {
            return GetAllWindows(false, false);
        }

        /// <summary>
        /// Take a Snap from the specific Window
        /// </summary>
        /// <param name="hWnd">Handle of the Window</param>
        /// <param name="useSpecialCapturing">if you need to capture from the minimized windows set it true,otherwise false</param>
        /// <returns></returns>
        public static WindowSnap GetWindowSnap(IntPtr hWnd, bool useSpecialCapturing)
        {
            if (!useSpecialCapturing)
                return new WindowSnap(hWnd, false);
            return new WindowSnap(hWnd, NeedSpecialCapturing(hWnd));
        }

        private static bool NeedSpecialCapturing(IntPtr hWnd)
        {
            if (IsIconic(hWnd)) return true;
            return false;
        }

        private static Bitmap GetWindowImage(IntPtr hWnd, Size size)
        {
            try
            {
                if (size.IsEmpty || size.Height < 0 || size.Width < 0) return null;

                Bitmap bmp = new Bitmap(size.Width, size.Height);
                Graphics g = Graphics.FromImage(bmp);
                IntPtr dc = g.GetHdc();

                PrintWindow(hWnd, dc, 0);

                g.ReleaseHdc();
                g.Dispose();

                return bmp;
            }
            catch { return null; }
        }

        private static string GetWindowText(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd) + 1;
            StringBuilder name = new StringBuilder(length);

            GetWindowText(hWnd, name, length);

            return name.ToString();
        }

        private static Rectangle GetWindowPlacement(IntPtr hWnd)
        {
            RECT rect = new RECT();

            GetWindowRect(hWnd, ref rect);

            return rect;
        }

        private static void EnterSpecialCapturing(IntPtr hWnd)
        {
            if (XPAppearance.MinAnimate)
            {
                XPAppearance.MinAnimate = false;
                minAnimateChanged = true;
            }


            winLong = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, winLong | WS_EX_LAYERED);
            SetLayeredWindowAttributes(hWnd, 0, 1, LWA_ALPHA);

            ShowWindow(hWnd, ShowWindowEnum.Restore);

            SendMessage(hWnd, WM_PAINT, 0, 0);
        }

        private static void ExitSpecialCapturing(IntPtr hWnd)
        {
            ShowWindow(hWnd, ShowWindowEnum.Minimize);
            SetWindowLong(hWnd, GWL_EXSTYLE, winLong);

            if (minAnimateChanged)
            {
                XPAppearance.MinAnimate = true;
                minAnimateChanged = false;
            }

        }

        #endregion

        private Bitmap image;
        private Size size;
        private Point location;
        private string text;
        private IntPtr hWnd;
        private bool isIconic;

        /// <summary>
        /// Get the Captured Image of the Window
        /// </summary>
        public Bitmap Image
        {
            get
            {
                if (this.image != null)
                    return this.image;
                return null;
            }
        }
        /// <summary>
        /// Get Size of Snapped Window
        /// </summary>
        public Size Size
        {
            get { return this.size; }
        }
        /// <summary>
        /// Get Location of Snapped Window
        /// </summary>
        public Point Location
        {
            get { return this.location; }
        }
        /// <summary>
        /// Get Title of Snapped Window
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }
        /// <summary>
        /// Get Handle of Snapped Window
        /// </summary>
        public IntPtr Handle
        {
            get { return this.hWnd; }
        }
        /// <summary>
        /// if the state of the window is minimized return true otherwise returns false
        /// </summary>
        public bool IsMinimized
        {
            get { return this.isIconic; }
        }

        private WindowSnap(IntPtr hWnd, bool specialCapturing)
        {
            this.isIconic = IsIconic(hWnd);
            this.hWnd = hWnd;

            if (specialCapturing)
                EnterSpecialCapturing(hWnd);

            #region Child Support (Enter)

            WINDOWINFO wInfo = new WINDOWINFO();
            wInfo.cbSize = WINDOWINFO.GetSize();
            GetWindowInfo(hWnd, ref wInfo);

            bool isChild = false;
            IntPtr parent = GetParent(hWnd);
            Rectangle pos = new Rectangle();
            Rectangle parentPos = new Rectangle();

            if (forceMDI && parent != IntPtr.Zero && (wInfo.dwExStyle & ExtendedWindowStyles.WS_EX_MDICHILD) == ExtendedWindowStyles.WS_EX_MDICHILD
                //&& (
                //(wInfo.dwStyle & WindowStyles.WS_CHILDWINDOW) == WindowStyles.WS_CHILDWINDOW||
                //(wInfo.dwStyle & WindowStyles.WS_CHILD) == WindowStyles.WS_CHILD)
                )//added 10 october 2007
                
            {
                StringBuilder name = new StringBuilder();
                GetClassName(parent, name, RUNDLL.Length + 1);
                if (name.ToString() != RUNDLL)
                {
                    isChild = true;
                    pos = GetWindowPlacement(hWnd);
                    MoveWindow(hWnd, int.MaxValue, int.MaxValue, pos.Width, pos.Height, true);

                    SetParent(hWnd, IntPtr.Zero);

                    parentPos = GetWindowPlacement(parent);
                }
            }
            #endregion

            Rectangle rect = GetWindowPlacement(hWnd);

            this.size = rect.Size;
            this.location = rect.Location;
            this.text = GetWindowText(hWnd);
            this.image = GetWindowImage(hWnd, this.size);

            #region Child Support (Exit)

            if (isChild)
            {
                SetParent(hWnd, parent);

                //int x = pos.X - parentPos.X;
                //int y = pos.Y - parentPos.Y;

                int x = wInfo.rcWindow.Left - parentPos.X;
                int y = wInfo.rcWindow.Top - parentPos.Y;

                if ((wInfo.dwStyle & WindowStyles.WS_THICKFRAME) == WindowStyles.WS_THICKFRAME)
                {
                    x -= SystemInformation.Border3DSize.Width;
                    y -= SystemInformation.Border3DSize.Height;
                }

                MoveWindow(hWnd, x, y, pos.Width, pos.Height, true);
            }
            #endregion

            if (specialCapturing)
                ExitSpecialCapturing(hWnd);
        }

        /// <summary>
        /// Gets the Name and Handle of the Snapped Window
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("Window Text: {0}, Handle: {1}", this.text, this.hWnd.ToString());

            return str.ToString();
        }
    }

}
