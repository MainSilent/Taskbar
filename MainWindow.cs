using System;
using MerulaShellController.ManageWindows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace Taskbar
{
    public partial class MainWindow
    {
        public Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            //convert image format
            var src = new FormatConvertedBitmap();
            src.BeginInit();
            src.Source = bitmapsource;
            src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
            src.EndInit();

            //copy to bitmap
            Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bitmap.UnlockBits(data);

            return bitmap;
        }
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

        //private delegate void DelegateVoid();

        void WindowManagerWindowListChanged(object sender, EventArgs e)
        {
            //invoke beacause merula shell runs in another thread
            
        }

        private void LoadWindows()
        {
            var windows = windowManager.GetWindows();// returns all the active windows
            List<dynamic> all_windows = new List<dynamic>();

            foreach (var window in windows) 
            {
                var win = new Window();
                // convert ProgramIcon to base64
                dynamic icon = BitmapFromSource(window.ProgramIcon);
                MemoryStream ms = new MemoryStream();
                icon.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                icon = Convert.ToBase64String(byteImage);
                
                win.handle = window.Handler.ToInt32();
                win.title = window.Title;
                win.icon = icon;
                all_windows.Add(win);
            }
            ((Func<object, Task<object>>)Main.callback)(all_windows);
        }
    }
}
