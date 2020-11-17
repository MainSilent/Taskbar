using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Taskbar
{
    public class Main
    {
        public static dynamic callback;
        // Get all opened programs
        public async Task<object> init(dynamic input)
        {
            callback = input;
            new MainWindow();

            return null;
        }

        // toggle the window
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        public int SW_MINIMIZE = 6;
        public int SW_RESTORE = 9;

        public async Task<object> toggleWindow(int handle)
        {
            IntPtr hWnd = new IntPtr(handle);

            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }
            else
            {
                if (GetForegroundWindow() == hWnd)
                    ShowWindow(hWnd, SW_MINIMIZE);
                else
                    SetForegroundWindow(hWnd);
            }

            return true;
        }

        // Get screenshot of the window
        public async Task<object> screenshot(int handle)
        {
            IntPtr hWnd = new IntPtr(handle);

            return "ScreenCapture.ImgtoBase64(img)";
        }

        // Close the program
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        public async Task<object> close(int handle)
        {
            IntPtr hWnd = new IntPtr(handle);

            int processid = 0;
            GetWindowThreadProcessId((IntPtr)hWnd, out processid);
            try
            {
                Process tempProc = Process.GetProcessById(processid);
                tempProc.CloseMainWindow();
                tempProc.WaitForExit();
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }

            return null;
        }
    }
}