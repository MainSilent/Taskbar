using System;
using Window = MerulaShell.windows.Window;

namespace Taskbar
{
    /// <summary>
    /// Interaction logic for TaskbarButton.xaml
    /// </summary>
    public partial class TaskbarButton
    {
        private readonly Window window;

        //Contructor with a MerulaShell.windows.window as input
        public TaskbarButton(Window window)
        {
            this.window = window;
            window.TitleChanged += WindowTitleChanged; //when the title of the window changes 
            SetProperties(); //set the window properties
        }

        private delegate void DelegateVoid();

        void WindowTitleChanged(object sender, EventArgs e)
        {
            //Dispatcher.Invoke(new DelegateVoid(SetTitle)); //invoke beacause merula shell runs in another thread
        }

        private void SetTitle()
        {
            //lblTitle.Text = window.Title; // sets the title in the textblock
        }

        private void SetProperties()
        {
            //imgIcon.Source = window.ProgramIcon; // sets the icon of the window
            //lblTitle.Text = window.Title; // sets the title in the textblock
        }

        private bool active;

        private void UserControlMouseUp(object sender)
        {
            //Background = Brushes.LightBlue; //set a nice active color
            if(active) //when active minimize and maximize
                window.MaximizeMinimize(); //minimize or maximize
            else
            {
                active = true; // set active
                window.SetToForeground(); //set window to foreground
            }
            InvokeActivated(new EventArgs());
        }

        public void SetNonActive()
        {
            active = false; //set active to false
            //Background = Brushes.White; //reset color to white
        }

        public event EventHandler Activated; //event to notify the mainwindow

        public void InvokeActivated(EventArgs e)
        {
            EventHandler handler = Activated;
            if (handler != null) handler(this, e);
        }
    }
}
