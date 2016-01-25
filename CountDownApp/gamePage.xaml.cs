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

        TextBox[] _textboxeArray = new TextBox [9];

        private int _rndNum = 0 , _textBoxLoc = 0;


        public gamePage()
        {
            this.InitializeComponent();

            getTextBoxList();
        }


        //gets the amount of textboxes from gamePage and puts them into a list
        private void getTextBoxList()
        {
            for (int i = 0; i < _textboxeArray.Length; i++)
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
            _rndNum = rndNumGen(_vowlesArray.Length);
            sendCharToTextBox((Char)_vowlesArray.GetValue(_rndNum));
        }

        /*
        Pass in the Consonants arrays length to the random generator.
        then gets a random letter from that array and outputs that 
        letter in the aproperate box
        */
        private void btnConsonants_Click(object sender, RoutedEventArgs e)
        {         
            _rndNum = rndNumGen(_consonantsArray.Length);
            sendCharToTextBox((Char)_consonantsArray.GetValue(_rndNum));
        }

        /*
        Get the location of the textbox and display the char passed in
        Also disable the 2 buttons that generate the letters
        */
        private void sendCharToTextBox(char _letter)
        {
            _textboxeArray[_textBoxLoc].Text = _letter.ToString();

            if (_textBoxLoc == _textboxeArray.Length-1)
            {
                _textBoxLoc = 0;
                btnVowels.Visibility = Visibility.Collapsed;
                btnConsonants.Visibility = Visibility.Collapsed;

                btnCheckWord.Visibility = Visibility.Visible;
            }         
            else
                _textBoxLoc++;
        }

        /*
        Generates a number between 0 and the number passed in
        */
        private int rndNumGen(int maxNo)
        {
            var r = new Random();
            int   num = r.Next(maxNo);
            return num;
        }

        private void btnCheckWord_Click(object sender, RoutedEventArgs e)
        {
            //Check that the Word is using the same letters Displayed


            validateUserWord();


            resetGame();
        }

        private void validateUserWord()
        {
            //Searches the _wordsList for the word entered by the user and gives its index
            int i = App._wordsList.BinarySearch(txtBoxUsrWord.Text);

            //if the word is found in _wordsList
            if (i >= 0)
            {
                //Correct WordEntered
            }

            //IF THE WORD IS CORRECT, GET THE "WORDS LENGTH" AND ADD THAT TO THERE SCORE
        }

        /*
        Resets the games: Buttons, and emptys the textboxes containing letters
        */
        private void resetGame()
        {
            btnVowels.Visibility = Visibility.Visible;
            btnConsonants.Visibility = Visibility.Visible;

            txtBoxUsrWord.Text = "";

            btnCheckWord.Visibility = Visibility.Collapsed;

            //Reset all the textboxes to have nothing in them
            foreach (TextBox t in _textboxeArray) {
                _textboxeArray[_textBoxLoc++].Text = "";
            }
            _textBoxLoc = 0;
        }

    }
}
