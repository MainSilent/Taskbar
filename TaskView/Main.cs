using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using VirtualDesktop;
using System.Runtime.InteropServices;

namespace TaskView
{
    class Main
    {
        // count all virtual desktop
        public async Task<object> count(dynamic input)
        {
            try
            {
                return Desktop.Count;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        // get current desktop
        public async Task<object> current(dynamic input)
        {
            try
            {
                return DesktopManager.GetDesktopIndex(
                    DesktopManager.VirtualDesktopManagerInternal.GetCurrentDesktop()
                );
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        // go to left desktop
        public async Task<object> left(dynamic data)
        {
            try
            {
                Desktop.Current.Left.MakeVisible();
                return true;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        // go to right desktop
        public async Task<object> right(dynamic data)
        {
            try
            {
                Desktop.Current.Right.MakeVisible();
                return true;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        // move window to current desktop
        public async Task<object> moveCurrent(int handle)
        {
            try
            {
                IntPtr hWnd = new IntPtr(handle);
                Desktop.Current.MoveWindow(hWnd);

                return true;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        // remove desktop
        public async Task<object> remove(int index)
        {
            try
            {
                Desktop.FromIndex(index).Remove();
                return true;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        // toggle windows same as pressing 'winkey + d'
        public async Task<object> toggle(int input)
        {
            try
            {
                Taskbar.KeyboardSend.KeyDown(Keys.LWin);
                Taskbar.KeyboardSend.KeyDown(Keys.D);
                Taskbar.KeyboardSend.KeyUp(Keys.LWin);
                Taskbar.KeyboardSend.KeyUp(Keys.D);

                return true;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}
