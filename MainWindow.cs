using System;
using MerulaShellController.ManageWindows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Taskbar
{
    public partial class MainWindow
    {
        public class Window
        {
            public string id { get; set; }
            public int handle { get; set; }
            public string title { get; set; }
            public string path { get; set; }
            public string icon { get; set; }
        }

        private static ManageWindows windowManager;

        public MainWindow()
        {
            try
            {
                windowManager = new ManageWindows(); //create a new windowmanager / only one needed
                windowManager.WindowListChanged += WindowManagerWindowListChanged; //when the list of windows is changed
                LoadWindows(); //load the windows
            }
            catch (Exception e)
            {
                ((Func<object, Task<object>>)Main.callback)("Error: " + e.Message);
            }
        }

        void WindowManagerWindowListChanged(object sender, EventArgs e)
        {
            try
            {
                LoadWindows();
            }
            catch (Exception e2)
            {
                ((Func<object, Task<object>>)Main.callback)("Error: " + e2.Message);
            }
        }

        private void LoadWindows()
        {
            try
            {
                var windows = windowManager.GetWindows();// returns all the active windows
                List<Window> all_windows = new List<Window>();

                foreach (var window in windows)
                {
                    new Task(window);
                    Window win = new Window();
                    // path
                    int processid = 0;
                    GetWindowThreadProcessId((IntPtr)window.Handler, out processid);
                    string path = (Process.GetProcessById(processid)).Modules[0].FileName;

                    //icon
                    dynamic icon = BitmapFromSource(window.ProgramIcon);
                    icon = ScreenCapture.ImgtoBase64(icon);

                    win.id = Guid.NewGuid().ToString();
                    win.handle = window.Handler.ToInt32();
                    win.title = Task.checkTitle(window.Title, window.Handler);
                    win.path = path;
                    win.icon = icon;
                    if (window.Title != "")
                        all_windows.Add(win);
                }
                ((Func<object, Task<object>>)Main.callback)(all_windows);
            }
            catch (Exception e)
            {
                ((Func<object, Task<object>>)Main.callback)("Error: " + e.Message);
            }
        }

        public static bool check(IntPtr handle)
        {
            try
            {
                var windows = windowManager.GetWindows();

                foreach (var window in windows)
                {
                    if (window.Handler == handle)
                        return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return  false;
            }
        }

        public Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            try
            {
                //convert image format
                var src = new FormatConvertedBitmap();
                src.BeginInit();
                src.Source = bitmapsource;
                src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
                src.EndInit();

                //copy to bitmap
                Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, PixelFormat.Format32bppArgb);
                var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bitmap.UnlockBits(data);

                return bitmap;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);
    }
}
