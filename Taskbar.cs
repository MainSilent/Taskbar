using System;
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
            
            //((Func<object, Task<object>>)Main.callback)("fgh");
            new MainWindow();

            return null;
        }
    }
}
