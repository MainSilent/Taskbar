using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Taskbar
{
    public class ScreenCapture
    {
        // get the screenshot
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static Bitmap CaptureApplication(IntPtr hWnd)
        {
            try
            {
                Rect rect = new Rect();
                IntPtr error = GetWindowRect(hWnd, ref rect);

                // sometimes it gives error.
                while (error == (IntPtr)0)
                {
                    error = GetWindowRect(hWnd, ref rect);
                }

                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics.FromImage(bmp).CopyFromScreen(rect.left,
                                                       rect.top,
                                                       0,
                                                       0,
                                                       new System.Drawing.Size(width, height),
                                                       CopyPixelOperation.SourceCopy);
                return bmp;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Convert image to base64
        public static string ImgtoBase64(dynamic img)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                return Convert.ToBase64String(byteImage);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}