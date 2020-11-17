using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Window = MerulaShell.windows.Window;

namespace Taskbar
{
    public partial class Task
    {
        private readonly Window window;

        public Task(Window window)
        {
            this.window = window;
            window.TitleChanged += WindowTitleChanged; //when the title of the window changes 
        }

        void WindowTitleChanged(object sender, EventArgs e)
        {
            List<dynamic> data = new List<dynamic>();
            data.Add("update_title");
            data.Add(window.Handler);
            data.Add(checkTitle(window.Title, window.Handler));

            ((Func<object, Task<object>>)Main.callback)(data);
        }

        /* The reason we need this method is because utf8 charchters are replaced by '?',
        *  I tried different options to fix this but it does't work because the problem
        *  comes from the MerulaShell library, so i had not choice but check if the title contains '?'
        *  and then get the title with 'proc.MainWindowTitle'.
        */
        public static string checkTitle(string title, IntPtr handler)
        {
            if(title.Contains("?"))
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process proc in processes)
                {
                    if (proc.MainWindowHandle == handler)
                    {
                        return proc.MainWindowTitle;
                    }
                }
            }

            return title;
        }
    }
}
