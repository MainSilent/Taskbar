using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using static MerulaShell.workspace.WorkArea;

namespace Taskbar
{
    public class ScreenCapture
    {


        // Convert image to base64
        public static string ImgtoBase64(dynamic img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }
    }
}