using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CountDownApp
{

    public sealed partial class gamePage : Page
    {

        private char[] _vowlesArray = new char[5] { 'A', 'E', 'I', 'O', 'U'};
        private char[] _consonantsArray = new char[21] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R' , 'S', 'T', 'V', 'W', 'X', 'Y', 'Z'};

        List<TextBox> textboxes = new List<TextBox>();
        TextBox[] _textboxeArray = new TextBox [9];

        private int _rndNum = 0 , _textBoxLoc = 0;
        private char _curChar;


        public gamePage()
        {
            this.InitializeComponent();

            fillTextBoxList();
        }


        private void fillTextBoxList()
        {
            for (int i = 0; i < 9; i++)
            {
                _textboxeArray[i] = ((TextBox)LettersGrid.FindName("textBox" + i.ToString()));
            }
        }


        /*
        pass in the Vowels arrays length to the random generator
        then gets a random letter from the array and Outputs that
        letter in the aproperate box
        */
        private void btnVowels_Click(object sender, RoutedEventArgs e)
        {
            _rndNum = rndNumGenerator(_vowlesArray.Length);
            sendCharToTextBox((Char)_vowlesArray.GetValue(_rndNum));
        }

        /*
        Pass in the Consonants arrays length to the random generator.
        then gets a random letter from that array and outputs that 
        letter in the aproperate box
        */
        private void btnConsonants_Click(object sender, RoutedEventArgs e)
        {         
            _rndNum = rndNumGenerator(_consonantsArray.Length);
            sendCharToTextBox((Char)_consonantsArray.GetValue(_rndNum));
        }

        /*
        Get the location of the textbox and display the char passed in
        Also disable the 2 buttons that generate the letters
        */
        private void sendCharToTextBox(char _letter)
        {
            _textboxeArray[_textBoxLoc].Text = _letter.ToString();

            if (_textBoxLoc == 8){
                _textBoxLoc = 0;
                btnVowels.Visibility = Visibility.Collapsed;
                btnConsonants.Visibility = Visibility.Collapsed;
            }         
            else
                _textBoxLoc++;
        }

        /*
        Generates a number between 0 and the number passed in
        */
        private int rndNumGenerator(int maxNo)
        {
            var r = new Random();
            int   num = r.Next(maxNo);
            return num;
        }

        private void btnCheckWord_Click(object sender, RoutedEventArgs e)
        {
            resetGame();
        }

        private void validateUserWord()
        {
            //GET THE USERS INPUT
            //COMPARE THAT AGAINST A DICTONARY TEXT FILE
            //IF THE WORD IS CORRECT, GET THE "WORDS LENGTH" AND ADD THAT TO THERE SCORE
        }

        /*
        Resets the games: Buttons, and emptys the textboxes containing letters
        */
        private void resetGame()
        {
            btnVowels.Visibility = Visibility.Visible;
            btnConsonants.Visibility = Visibility.Visible;
            foreach (TextBox t in _textboxeArray) {
                _textboxeArray[_textBoxLoc++].Text = "";
            }
            _textBoxLoc = 0;
        }
    }
}
