using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Timers;
using TI_WindowsLib;

namespace Simplify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // TODO: extract this nested class.
        private class BLEListener : BLEButtonListener
        {
            public BLEListener(BLEButton bleButton)
            {
                this.BleButton = bleButton;
            }
            public override void OnLeft(BLEButton sender, DateTimeOffset timestamp)
            {
                AppCommand.MEDIA_PLAY_PAUSE.Exec();
            }


            public override void OnRight(BLEButton sender, DateTimeOffset timestamp)
            {
                AppCommand.MEDIA_NEXTTRACK.Exec();
            }

            public override void OnBoth(BLEButton sender, DateTimeOffset timestamp)
            {
                AppCommand.MEDIA_PREVIOUSTRACK.Exec();
                AppCommand.MEDIA_PLAY_PAUSE.Exec();
            }



            public BLEButton BleButton { get; set; }
        }

        HelpPopUp popUp;
        Boolean popUpIsOpen = false;
        Timer holdTimer = new Timer(); 
        const int HOLD_INTERVAL = 250; //In ms

        public MainWindow()
        {
            InitializeComponent();
            holdTimer.Interval = HOLD_INTERVAL;
            holdTimer.AutoReset = false;
            holdTimer.Elapsed += centerButton_Hold;

            BLEButtonFactory factory = new BLEButtonFactory();
            List<BLEButton> buttons = factory.GetAllButtons();
            if (buttons.Count > 0)
            {
                BLEButton bleButton = factory.GetAllButtons()[0];
                bleButton.Listener = new BLEListener(bleButton);
                bleButton.Connect();
                MessageBox.Show("Found a button");
            }
        }

        private void backdrop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void centerButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AppCommand.MEDIA_NEXTTRACK.Exec();
            //Tell the program to immediately start playing the track
            AppCommand.MEDIA_PLAY_PAUSE.Exec();
            
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            //Close/open a help pop-up.
            if (popUpIsOpen)
            {
                popUp.Close();
            }
            else
            {
                popUp = new HelpPopUp();
                popUp.Show();
            }

            //Toggle the boolean that keeps track of the help pop-up's status.
            popUpIsOpen = !popUpIsOpen;

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if(popUp != null)
                popUp.Close();
            this.Close();
        }


        private void centerButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Start counting to to determine if the button is being held
            holdTimer.Start();
        }

        private void centerButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //If still counting time...
            if (holdTimer.Enabled)
            {
                //Disable timer before it triggers
                holdTimer.Stop();

                //Check if single or double click
                if (e.ClickCount == 1)
                {
                    AppCommand.MEDIA_PLAY_PAUSE.Exec();
                }
            }
        }

        private void centerButton_Hold(object sender, ElapsedEventArgs e)
        {
            AppCommand.MEDIA_PREVIOUSTRACK.Exec();
            AppCommand.MEDIA_PREVIOUSTRACK.Exec();
        }

    }
}
