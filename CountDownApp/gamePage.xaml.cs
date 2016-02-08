using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace CountDownApp
{

    public sealed partial class gamePage : Page
    {

        private char[] _vowlesArray = new char[5] { 'A', 'E', 'I', 'O', 'U' };
        private char[] _consonantsArray = new char[21] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
        private List<Char> _listAvailLetters = new List<char>();

        //Create an TextBox array that holds 9 TextBoxs
        private TextBox[] _textboxArray = new TextBox[9];

        private int _rndNum = 0, _textBoxLoc = 0;

        private DispatcherTimer _countDownTimer = new DispatcherTimer();
        private int _countTicks = 29;


        public gamePage()
        {
            this.InitializeComponent();

            //Set up the clock
            setClockMarkers();
        }


        /*Sets up the Clocks Markers(numbers) at runTime into TextBlocks
        */
        private void setClockMarkers()
        {
            int num = 5;//Increment in 5s

            for (int i = 1; i <= 12; ++i)
            {
                TextBlock tb = new TextBlock();

                tb.Text = num.ToString();

                tb.TextAlignment = TextAlignment.Center;
                tb.RenderTransformOrigin = new Point(1, 1);
                tb.FontSize = 10;

                //sets the Number output to the textblocks in incriments of 5
                num += 5;

                double radius = 145;

                //Sets the spacing between each of the numbers
                double angle = Math.PI * i * 30.0 / 180.0;

                //sets the x and y positions of the Circle on the canvas
                double xPos = Math.Sin(angle) * radius + 145;
                double yPos = -Math.Cos(angle) * radius + 145; //-Math.Cos important

                Canvas.SetLeft(tb, xPos);
                Canvas.SetTop(tb, yPos);

                //adds the TextBlocks to the canvas as a child element
                _markersCanvas.Children.Add(tb);
            }

            getTextBoxList();
        }


        /*Gets the amount of TextBoxes from gamePage and puts them into an array
        */
        private void getTextBoxList()
        {
            for (int i = 0; i < _textboxArray.Length; i++)
            {
                _textboxArray[i] = ((TextBox)stkPanContiners.FindName("txtLetterBox" + i.ToString()));
            }
        }


        /*Pass in the Vowels[] length to the random generator
          which then calls a random generator to pick a random number,
          and uses that number against the array to get a random letter
          and passes it to the method sendCharToTextBox(), which finds the 
          Current textBox to place the letter into
        */
        private void btnVowels_Click(object sender, RoutedEventArgs e)
        {
            _rndNum = rndNumGen(_vowlesArray.Length);
            sendCharToTextBox((Char)_vowlesArray.GetValue(_rndNum));
        }


        /*Pass in the Consonants[] length to the random generator
          which then calls a random generator to pick a random number,
          and uses that number against the array to get a random letter
          and passes it to the method sendCharToTextBox(), which finds the 
          Current textBox to place the letter into
        */
        private void btnConsonants_Click(object sender, RoutedEventArgs e)
        {
            _rndNum = rndNumGen(_consonantsArray.Length);
            sendCharToTextBox((Char)_consonantsArray.GetValue(_rndNum));
        }


        /* Get the location of the textBox and displays the char passed in by the 
           btnVowels/btnConsonants btn click events. Also disable the 2 buttons that 
           generate the letters and makes the entry for the word and Button Visible
        */
        private void sendCharToTextBox(char _letter)
        {
            _listAvailLetters.Add(_letter);//Add the current letters to a list 
            _textboxArray[_textBoxLoc].Text = _letter.ToString();//send the textbox the letter

            //If all letters are added
            if (_textBoxLoc == _textboxArray.Length - 1)
            {
                startTimer();//Start the Timer

                ShowStoryboardAnimation.Begin();//clock animation

                _textBoxLoc = 0;

                btnVowels.Visibility = Visibility.Collapsed;
                btnConsonants.Visibility = Visibility.Collapsed;
                txtBoxUsrWord.Visibility = Visibility.Visible;
                btnCheckWord.Visibility = Visibility.Visible;
            }
            else
                _textBoxLoc++;//used to fnd the next TextBox
        }


        /*Generates a number between 0 and the number passed in,
        and adds a check to see if the last number generated was the 
        same as the current, if so get another 
        */
        private int rndNumGen(int _maxNo)
        {
            Random r = new Random();
            int _curNum = r.Next(_maxNo);

            //Stops 2 lettets of the same beside Each other
            if (_rndNum == _curNum)
                _curNum = r.Next(_maxNo);

            return _curNum;
        }


        /*Button click event to check the word from the user
        */
        private void btnCheckWord_Click(object sender, RoutedEventArgs e)
        {
            //Checks that the letters Entered were valid letters given  (True/Fase)       
            if (chkLettersValidity())
            {
                chkWordValidity();
            }
            else
            {
                //Display a message to the user that its an INVALID WORD based on the givn letters
            }


            //RESET AFTER THE GAMES OVER
            resetGame();
        }


        /*Checks that the letters entered by the user(converted to uppercase) are the letters that were displayed
          and that there are no duplicate letters used by the user when not available 
        */
        private Boolean chkLettersValidity()
        {
            Boolean validLetters = true;
            String toUpper;
            int leterIndex;

            //Convert the users input to upper case
            toUpper = txtBoxUsrWord.Text.ToUpper();

            //Creates a new char array from the string to check for correct letters
            Char[] newArray = toUpper.ToCharArray();

            //Check that the Word is using the same letters Displayed
            for (int i = 0; i < newArray.Length; i++)
            {
                //Finds the index of the current letter [i]  in the list _listAvailLetters, 
                leterIndex = _listAvailLetters.IndexOf(newArray[i]);

                //If the letters not in the list
                if (leterIndex == -1)
                {
                    validLetters = false;
                }
                //Remove the letter from the list of available letters to stop duplicate letters 
                else
                    _listAvailLetters.RemoveAt(leterIndex);
            }
            return validLetters;
        }


        /*Checks the Word Entered By the user against the App._wordsList using a Binary Search,
        */
        private void chkWordValidity()
        {
            //Searches the _wordsList for the word entered by the user(To lowercase) and gives its index
            int _index = App._wordsList.BinarySearch(txtBoxUsrWord.Text.ToLower());

            //if the word is found in App._wordsList
            if (_index >= 0)
            {
                //Gets the words length and adds it to the users Score
                App._userScore = txtBoxUsrWord.Text.Length;



                //                ************************************ ScoreBoard *********************************************************
                stkPnlScoreBoard.Visibility = Visibility.Visible;
                txtBlockUsrScore.Text += ("        " + txtBoxUsrWord.Text + "\t\t" + App._userScore + "\n");
                //                *********************************************************************************************************



            }
        }


        /* Adds a KeyDown Event to Checks all input from the keyboard in the textBox txtBoxUsr to see if the user hit Enter key,
           if they did call the btnCheckWord_Click button
        */
        private void txtBoxUsrWord_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                btnCheckWord_Click(sender, e);
            }
        }


        /*Starts the Timer in an intervals of 1 sec
        */
        private void startTimer()
        {
            _countDownTimer.Tick += numTimer_Tick;
            _countDownTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            _countDownTimer.Start();
        }


        /*if the timers _countTicks is equal to -1 reset the game;
        */
        void numTimer_Tick(object sender, object e)
        {
            time_Box.Text = _countTicks--.ToString();
            if (_countTicks == -1)
            {
                resetGame();
            }
        }


        /* Resets the games: Buttons, and emptys the textboxes containing letters
        */
        private void resetGame()
        {
            _countDownTimer.Stop();
            _countDownTimer.Tick -= numTimer_Tick;
            _countTicks = 29;
            ShowStoryboardAnimation.Stop();
            time_Box.Text = "";


            btnVowels.Visibility = Visibility.Visible;
            btnConsonants.Visibility = Visibility.Visible;

            txtBoxUsrWord.Text = "";

            btnCheckWord.Visibility = Visibility.Collapsed;
            txtBoxUsrWord.Visibility = Visibility.Collapsed;

            //Reset all the textboxes to have nothing in them
            foreach (TextBox t in _textboxArray)
            {
                _textboxArray[_textBoxLoc++].Text = "";
                _listAvailLetters.Clear();//Clear the list
            }
            _textBoxLoc = 0;
        }




    }
}
