using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TI_WindowsLib;

namespace Simplify
{
    class BLEListener : BLEButtonListener
    {
        private Button centerButton;

        private bool centerButtonPressed
        {
            set
            {
                centerButton.Dispatcher.BeginInvoke(new Action(() =>
                {
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(centerButton, new object[] { value });
                }
                    ));
            }
        }

        public BLEListener(Button centerButton)
        {
            this.centerButton = centerButton;
        }

        public override void OnLeft(BLEButton sender, DateTimeOffset timestamp)
        {
            AppCommand.MEDIA_PLAY_PAUSE.Exec();
            centerButtonPressed = true;
        }


        public override void OnRight(BLEButton sender, DateTimeOffset timestamp)
        {
            AppCommand.MEDIA_NEXTTRACK.Exec();
            centerButtonPressed = true;
        }

        public override void OnBoth(BLEButton sender, DateTimeOffset timestamp)
        {
            AppCommand.MEDIA_PREVIOUSTRACK.Exec();
            AppCommand.MEDIA_PLAY_PAUSE.Exec();
            centerButtonPressed = true;
        }

        public override void OnUp(BLEButton sender, DateTimeOffset timestamp)
        {
            centerButtonPressed = false;
        }


    }
}
