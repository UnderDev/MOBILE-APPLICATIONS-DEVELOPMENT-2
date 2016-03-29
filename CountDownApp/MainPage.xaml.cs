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

        DispatcherTimer imgTimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            //load all the words into _ListOfWords
            App.load();

            while (App.check == false)
            {
            }
        }

        private void startAnimation()
        {
            //after 2 seconds and the timer stops navigate to the new page
            imgTimer.Tick += imgTimer_Tick;
            imgTimer.Interval = new TimeSpan(0, 0, 0, 2, 0);
            imgTimer.Start();

            btnSinglePlayer.Visibility = Visibility.Collapsed;
            btnMultiPlayer.Visibility = Visibility.Collapsed;
            btnRules.Visibility = Visibility.Collapsed;
        }

        void imgTimer_Tick(object sender, object e)
        {
            imgTimer.Stop();
            Frame.Navigate(typeof(NumberGamePage));
        }

        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            animatedImage_ImageOpened();

            startAnimation();
        }

        private void btnMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            animatedImage_ImageOpened();
            startAnimation();
        }

        private void btnRules_Click(object sender, RoutedEventArgs e)
        {
            animatedImage_ImageOpened();
            startAnimation();
        }

        public void animatedImage_ImageOpened()
        {
            ShowStoryboard.Begin();//Start the Animation 
        }
    }
}
