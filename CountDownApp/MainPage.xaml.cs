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

            //Load all the words into _ListOfWords
            App.load();

            while (App.check == false)
            {
            }
        }


        private void MenuChoice_Click(object sender, RoutedEventArgs e)
        {
            Button btnMenu =  (Button)sender;
            animatedImage_ImageOpened();
            startAnimation();

            String name = btnMenu.Name;
            int btnNum = Convert.ToInt16(name.Substring(name.Length-1));

            MenuNavigation(btnNum);
        }

        private Boolean MenuNavigation(int btnNum)
        {

            if (btnNum == 1)
                App._NumOfGames = 10;
            else if(btnNum == 2)
                App._NumOfGames = 1000;

            switch (btnNum)
            {
                case 0: return (Frame.Navigate(typeof(WordGamePage)));
                case 1: return (Frame.Navigate(typeof(WordGamePage)));
                //case 2: return (Frame.Navigate(typeof(RulesGamePage)));
                //case 3: return (Frame.Navigate(typeof(ScoreGamePage)));
                default: throw new Exception("Cant Navigate To Page");
            }
        }

        private void startAnimation()
        {
            //after 2 seconds and the timer stops navigate to the new page
            imgTimer.Tick += imgTimer_Tick;
            imgTimer.Interval = new TimeSpan(0, 0, 0, 2, 0);
            imgTimer.Start();

            btnSinglePlayer0.Visibility = Visibility.Collapsed;
            btnMultiPlayer1.Visibility = Visibility.Collapsed;
            btnRules2.Visibility = Visibility.Collapsed;
            btnScoreBoard3.Visibility = Visibility.Collapsed;
        }

        void imgTimer_Tick(object sender, object e)
        {
            imgTimer.Stop();
          //  Frame.Navigate(typeof(WordGamePage));
        }

        public void animatedImage_ImageOpened()
        {
            ShowStoryboard.Begin();//Start the Animation 
        }

    }
}
