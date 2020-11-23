using System.Windows.Forms;
using System.Threading.Tasks;

namespace Taskbar
{
    class Media
    {
        public async Task<object> control(string input)
        {
            switch(input)
            {
                case "play_pause":
                    KeyboardSend.KeyDown(Keys.MediaPlayPause);
                    KeyboardSend.KeyUp(Keys.MediaPlayPause);
                    break;

                case "next":
                    KeyboardSend.KeyDown(Keys.MediaNextTrack);
                    KeyboardSend.KeyUp(Keys.MediaNextTrack);
                    break;

                case "previous":
                    KeyboardSend.KeyDown(Keys.MediaPreviousTrack);
                    KeyboardSend.KeyUp(Keys.MediaPreviousTrack);
                    break;

                case "volume_up":
                    KeyboardSend.KeyDown(Keys.VolumeUp);
                    KeyboardSend.KeyUp(Keys.VolumeUp);
                    break;

                case "volume_down":
                    KeyboardSend.KeyDown(Keys.VolumeDown);
                    KeyboardSend.KeyUp(Keys.VolumeDown);
                    break;
            }

            return null;
        }
    }
}
