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

namespace Simplify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 8;
        private const int APPCOMMAND_VOLUME_UP = 10;
        private const int APPCOMMAND_VOLUME_DOWN = 9;
        private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 14;
        private const int APPCOMMAND_MEDIA_NEXTTRACK = 11;
        private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 12;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        IntPtr Handle = Process.GetProcessesByName("explorer").First().MainWindowHandle;

        private void AppCommand(int command)
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)(command << 16));
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
        }



        private void backdrop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void centerButton_Click(object sender, RoutedEventArgs e)
        {
            //AppCommand(APPCOMMAND_MEDIA_PLAY_PAUSE);
        }

        private void centerButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AppCommand(APPCOMMAND_MEDIA_NEXTTRACK);
            //Tell the program to immediately start playing the track
            AppCommand(APPCOMMAND_MEDIA_PLAY_PAUSE);
            
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
                    AppCommand(APPCOMMAND_MEDIA_PLAY_PAUSE);
                }

                if (e.ClickCount == 2)
                {
                    AppCommand(APPCOMMAND_MEDIA_NEXTTRACK);
                } 
            }
        }

        private void centerButton_Hold(object sender, ElapsedEventArgs e)
        {
            AppCommand(APPCOMMAND_MEDIA_PREVIOUSTRACK);
            AppCommand(APPCOMMAND_MEDIA_PREVIOUSTRACK);
        }


    }
}
