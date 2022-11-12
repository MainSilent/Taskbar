using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Taskbar
{
    public class Main
    {
        public static dynamic callback;
        // Get all opened programs
        public async Task<object> init(dynamic input)
        {
            try
            {
                callback = input;
                new MainWindow();

                return null;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
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
            try
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
            catch (Exception e)
            {
                ((Func<object, Task<object>>)Main.callback)("Error: " + e.Message);
            }

            return null;
        }

        // Get screenshot of the window
        public async Task<object> screenshot(int handle)
        {
            try
            {
                IntPtr hWnd = new IntPtr(handle);
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
            try
            {
                IntPtr hWnd = new IntPtr(handle);

                int processid = 0;
                GetWindowThreadProcessId((IntPtr)hWnd, out processid);

                Process tempProc = Process.GetProcessById(processid);
                tempProc.CloseMainWindow();
                return MainWindow.check(hWnd);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }

        }

        public async Task<object> kill(int handle)
        {
            try
            {
                IntPtr hWnd = new IntPtr(handle);

                int processid = 0;
                GetWindowThreadProcessId((IntPtr)hWnd, out processid);

                Process tempProc = Process.GetProcessById(processid);
                tempProc.Kill();
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }

            return null;
        }
    }
    static class KeyboardSend
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}