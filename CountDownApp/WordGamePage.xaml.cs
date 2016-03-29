using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace CountDownApp
{

    public sealed partial class WordGamePage : Page
    {

        private char[] _vowlesArray = new char[5] { 'A', 'E', 'I', 'O', 'U' };
        private char[] _consonantsArray = new char[21] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
        private List<Char> _listAvailLetters = new List<char>();

        //Create an Button array that holds 9 Button
        private Button[] _btnArray = new Button[9];

        private int _rndNum = 0, _btnLoc = 0;

        private DispatcherTimer _countDownTimer = new DispatcherTimer();
        private int _countTicks = 28;


        public WordGamePage()
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
                tb.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Red);

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

            getBtnList();
        }


        /*Gets the amount of TextBoxes from gamePage and puts them into an array
        */
        private void getBtnList()
        {
            for (int i = 0; i < _btnArray.Length; i++)
            {
                _btnArray[i] = ((Button)stkPanContiners.FindName("btnLetterBox" + i.ToString()));
            }
        }

        /*Enable all Buttons for the user to click
        */
        private void enableAllLetterBtns()
        {
            foreach (Button b in _btnArray)
            {
                b.IsEnabled = true;
                b.Opacity = 1;
            }
        }

        /*Disables all Buttons for the user to click
        */
        private void disableAllLetterBtns()
        {
            foreach (Button b in _btnArray)
            {
                b.IsEnabled = false;
                b.Opacity = 1;
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


        //FILLS THE 9 BUTTONS FROM CONST OR VOWL BUTTON
        /* Get the location of the Button and displays the char passed in by the 
           btnVowels/btnConsonants btn click events. Also disable the 2 buttons that 
           generate the letters and makes the entry for the word and Button Visible
        */
        private void sendCharToTextBox(char _letter)//Send char to BUTTON ************** FIX
        {
            _listAvailLetters.Add(_letter);//Add the current letters to a list 
            _btnArray[_btnLoc].Content = _letter.ToString();//sends the letter to the Btn

            //If all letters are added
            if (_btnLoc == _btnArray.Length - 1)
            {
                enableAllLetterBtns();//enable all the Btns


                startTimer();//Start the Timer

                ShowStoryboardAnimation.Begin();//Clock animation

                btnVowels.Visibility = Visibility.Collapsed;
                btnConsonants.Visibility = Visibility.Collapsed;

                txtBoxUsrWord.Visibility = Visibility.Visible;
                btnCheckWord.Visibility = Visibility.Visible;
                btnReset.Visibility = Visibility.Visible;

                _btnLoc = 0;//Reset the btn Location
            }
            else
                _btnLoc++;//Used to fnd the next TextBox
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

            //If the word is found in App._wordsList
            if (_index >= 0)
            {
                //Gets the words length and adds it to the users Score
                App._userScore = txtBoxUsrWord.Text.Length;

                stkPnlScoreBoard.Visibility = Visibility.Visible;

                txtBlockUsrScore.Text += (App._userScore + "\n");
                txtBlockUsrWord.Text += (txtBoxUsrWord.Text + "\n");
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
            //time_Box.Text = _countTicks--.ToString();
            if (_countTicks == -1)
            {
                resetGame();
            }
        }


        /*This Event gets Fired when the user clicks on any of the Buttons (btnLetterBox).
        * It then gets all the bnt info from the Sender, Checks the last char index in the 
        * the btns Name, and converts that into an int. After finding the number, it gets that button
        * by using the num as an index in _btnArray and mess with its properties.  
        */
        private void LetterBtn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            //String btnIndex;

            //btnIndex = clickedButton.Name;

            //int btnNum = Convert.ToInt32((btnIndex.Substring(btnIndex.Length - 1)));

            //clickedButton = _btnArray[btnNum];

            //Add the contents of that button into the Textbox
            txtBoxUsrWord.Text += clickedButton.Content;
            clickedButton.Opacity = .5;
            clickedButton.IsEnabled = false;
        }

        private void bntReset_Click(object sender, RoutedEventArgs e)
        {
            txtBoxUsrWord.Text = "";
            enableAllLetterBtns();
        }

        //txtBoxUsrWord.Text += txtLetterBox0.Text;

        /* Resets the games: Buttons, and emptys the textboxes containing letters
        */
        private void resetGame()
        {
            disableAllLetterBtns();
            _countDownTimer.Stop();
            _countDownTimer.Tick -= numTimer_Tick;
            _countTicks = 28;
            ShowStoryboardAnimation.Stop();
            //time_Box.Text = "";


            btnVowels.Visibility = Visibility.Visible;
            btnConsonants.Visibility = Visibility.Visible;

            txtBoxUsrWord.Text = "";

            btnCheckWord.Visibility = Visibility.Collapsed;
            txtBoxUsrWord.Visibility = Visibility.Collapsed;
            btnReset.Visibility = Visibility.Collapsed;

            //Reset all the textboxes to have nothing in them
            foreach (Button t in _btnArray)
            {
                _btnArray[_btnLoc++].Content = "";
                _listAvailLetters.Clear();//Clear the list
            }

            _btnLoc = 0;
        }

    }
}
