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

        // create new dekstop
        public async Task<object> create(dynamic input)
        {
            try
            {
                Desktop.Create();
                return true;
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
        public async Task<object> toggle(bool mode)
        {
            try
            {
                if(mode)
                {
                    Taskbar.KeyboardSend.KeyDown(Keys.LWin);
                    Taskbar.KeyboardSend.KeyDown(Keys.M);
                    Taskbar.KeyboardSend.KeyUp(Keys.LWin);
                    Taskbar.KeyboardSend.KeyUp(Keys.M);
                }
                else
                {
                    Taskbar.KeyboardSend.KeyDown(Keys.LWin);
                    Taskbar.KeyboardSend.KeyDown(Keys.LShiftKey);
                    Taskbar.KeyboardSend.KeyDown(Keys.M);
                    Taskbar.KeyboardSend.KeyUp(Keys.LWin);
                    Taskbar.KeyboardSend.KeyUp(Keys.LShiftKey);
                    Taskbar.KeyboardSend.KeyUp(Keys.M);
                }

                return true;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}
