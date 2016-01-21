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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CountDownApp
{

    public sealed partial class gamePage : Page
    {

        protected char[] _vowlesArray = new char[5] { 'A', 'E', 'I', 'O', 'U'};
        protected char[] _consonantsArray = new char[21] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R' , 'S', 'T', 'V', 'W', 'X', 'Y', 'Z'};

        private int rndNum = 0;

        public gamePage()
        {
            this.InitializeComponent();


        }

        private void btnVowels_Click(object sender, RoutedEventArgs e)
        {
            rndNum = rndNumGenerator(_vowlesArray.Length);
        }

        private void btnConsonants_Click(object sender, RoutedEventArgs e)
        {
            rndNum = rndNumGenerator(_consonantsArray.Length);
        }


        //Generates a number between 0 and the number passed in
        private int rndNumGenerator(int maxNo)
        {
            var r = new Random();
            int   num = r.Next(maxNo);
            return num;
        }



    }
}
