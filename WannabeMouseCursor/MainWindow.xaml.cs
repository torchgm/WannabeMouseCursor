using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WannabeMouseCursor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        IntPtr windowHandle;
        public MainWindow mw = (MainWindow)Application.Current.MainWindow;
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = -20;
        int offset = 5;
        int update = 0;

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int extendedStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
            SetWindowLong(windowHandle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            while (true)
            {
                Point position = GetCursorPosition();
                SetWindowPos(windowHandle, IntPtr.Zero - 1, Convert.ToInt32(position.X)+offset, Convert.ToInt32(position.Y)+offset, 50, 50, 0x4);
                Dispatcher.Invoke(() =>
                {
                    button.Content = position.X.ToString() + " " + position.Y.ToString() + " also terra is gud " + windowHandle.ToString();
                });
                System.Threading.Thread.Sleep(update);
            }

        }


        [DllImport("User32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("User32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("User32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("User32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        public static Point GetCursorPosition()
        {
            GetCursorPos(out POINT lpPoint);
            return lpPoint;
        }

        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Topmost = true;
            windowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            offset = Convert.ToInt32(OffsetTextBox.Text);
            update = Convert.ToInt32(UpdateTextBox.Text);
            PointyThing.Width = Convert.ToInt32(SizeTextBox.Text);
            PointyThing.Height = PointyThing.Width;
            PointyThing.Visibility = Visibility.Visible;
            worker.RunWorkerAsync();
        }
    }
}
