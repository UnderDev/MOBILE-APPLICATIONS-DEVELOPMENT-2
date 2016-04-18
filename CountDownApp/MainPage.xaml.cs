using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CountDownApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private DispatcherTimer imgTimer = new DispatcherTimer();
        private int _countTicks = 2, _btnNum = 0;

        public MainPage()
        {
            this.InitializeComponent();

            App._UserScore = 0;
            //Load all the words into _ListOfWords
            App.load();

            //W8 till all the contents of the files are loaded
            while (App._checkLoaded == false)
            {
            }
        }


        /*Click event to find what button the use clicked
        */
        private void MenuChoice_Click(object sender, RoutedEventArgs e)
        {
            Button btnMenu = (Button)sender;
            animatedImage_ImageOpened();

            String name = btnMenu.Name;
            _btnNum = Convert.ToInt16(name.Substring(name.Length - 1));
            startAnimation();
        }


        /*Navigates to a new page depending on what button the user clicked
        */
        private Boolean MenuNavigation()
        {
            switch (_btnNum)
            {
                case 0: return (Frame.Navigate(typeof(WordGamePage)));
                case 1: return (Frame.Navigate(typeof(WordGamePage)));
                case 2: return (Frame.Navigate(typeof(ScoreBoardGamePage)));
                default: throw new Exception("Cant Navigate To Page");
            }
        }


        /*Starts the animation 
        */
        private void startAnimation()
        {
            //after 2 seconds and the timer stops navigate to the new page
            imgTimer.Tick += imgTimer_Tick;
            imgTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            imgTimer.Start();

            ContentPanel.Visibility = Visibility.Collapsed;
        }


        /*Tick event for counting down then calling the method below when the timer is -1
        */
        private void imgTimer_Tick(object sender, object e)
        {
            _countTicks--;
            if (_countTicks == -1)
            {
                imgTimer.Stop();
                imgTimer.Tick -= imgTimer_Tick;
                MenuNavigation();
            }
        }

        private void animatedImage_ImageOpened()
        {
            ShowStoryboard.Begin();//Start the Animation 
        }

    }
}
