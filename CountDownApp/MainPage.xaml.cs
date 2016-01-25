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
        public MainPage()
        {
            this.InitializeComponent();

            //load all the words into _ListOfWords
            App.load();

            while (App.check == false)
            {
            }
        }

        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the Main Game Page
            Frame.Navigate(typeof(gamePage));
        }

        private void btnMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the Main Game Page
            Frame.Navigate(typeof(gamePage));
        }

        private void btnRules_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the Main Rules Page
            Frame.Navigate(typeof(rulesPage));
        }
    }
}
