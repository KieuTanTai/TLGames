using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace TLGames.Views
{
    /// <summary>
    /// Interaction logic for ResizeableBorder.xaml
    /// </summary>
    public partial class ResizeableBorder : UserControl
    {
        public ResizeableBorder()
        {
            InitializeComponent();
        }

        private void Resize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (sender is not FrameworkElement element) return;

            Window parentWindow = Window.GetWindow(this);
            if (parentWindow == null) return;

            ResizeDirection direction = GetResizeDirection(element.Name);
            if (direction == ResizeDirection.None) return;

            System.IntPtr hwnd = new WindowInteropHelper(parentWindow).Handle;
            if (hwnd == IntPtr.Zero) return;

            SendMessage(hwnd, WM_SYSCOMMAND, (IntPtr)(SC_SIZE + direction), IntPtr.Zero);
        }

        private static ResizeDirection GetResizeDirection(string name) => name switch
        {
            "Left" => ResizeDirection.Left,
            "Right" => ResizeDirection.Right,
            "Top" => ResizeDirection.Top,
            "Bottom" => ResizeDirection.Bottom,
            "TopLeft" => ResizeDirection.TopLeft,
            "TopRight" => ResizeDirection.TopRight,
            "BottomLeft" => ResizeDirection.BottomLeft,
            "BottomRight" => ResizeDirection.BottomRight,
            _ => ResizeDirection.None
        };

        #region Win32

        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private enum ResizeDirection
        {
            None = -1,
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8
        }

        #endregion
    }
}
