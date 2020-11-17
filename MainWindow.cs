using System;
using MerulaShellController.ManageWindows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Text;

namespace Taskbar
{
    public partial class MainWindow
    {
        public class Window
        {
            public int handle { get; set; }
            public string title { get; set; }
            public string icon { get; set; }
        }

        private ManageWindows windowManager;

        public MainWindow()
        {
            windowManager = new ManageWindows(); //create a new windowmanager / only one needed
            windowManager.WindowListChanged += WindowManagerWindowListChanged; //when the list of windows is changed
            LoadWindows(); //load the windows
        }

        void WindowManagerWindowListChanged(object sender, EventArgs e)
        {
            LoadWindows();
        }

        private void LoadWindows()
        {
            var windows = windowManager.GetWindows();// returns all the active windows
            List<Window> all_windows = new List<Window>();
            
            foreach (var window in windows) 
            {
                new Task(window);

                Window win = new Window();
                // convert ProgramIcon to base64
                dynamic icon = BitmapFromSource(window.ProgramIcon);
                MemoryStream ms = new MemoryStream();
                icon.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                icon = Convert.ToBase64String(byteImage);
                
                win.handle = window.Handler.ToInt32();
                win.title = Task.checkTitle(window.Title, window.Handler);
                win.icon = icon;
                all_windows.Add(win);
            }
            ((Func<object, Task<object>>)Main.callback)(all_windows);
        }
        public Bitmap BitmapFromSource(BitmapSource bitmapsource)
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
    }
}
