using System;
using System.Diagnostics;
using System.Drawing;
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

        // an event for windows focus changes
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            try
            {
                IntPtr handle = GetForegroundWindow();
                ((Func<object, Task<object>>)callback)(handle);
            }
            catch (Exception e)
            {
                ((Func<object, Task<object>>)callback)("Error: " + e.Message);
            }
        }
        public async Task<object> focus(dynamic input)
        {
            try
            {
                // get the fisrt focused window
                IntPtr handle = GetForegroundWindow();
                ((Func<object, Task<object>>)input)(handle);

                // event listener for focus changes
                callback = input;
                WinEventDelegate dele = new WinEventDelegate(WinEventProc);
                SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
            }
            catch (Exception e)
            {
                ((Func<object, Task<object>>)callback)("Error: " + e.Message);
            }

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

            try
            {
                Bitmap img = ScreenCapture.CaptureApplication(hWnd);
                return ScreenCapture.ImgtoBase64(img);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
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
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }

            return null;
        }
    }
}