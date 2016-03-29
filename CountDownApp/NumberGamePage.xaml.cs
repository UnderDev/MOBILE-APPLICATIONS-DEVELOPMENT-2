using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NumberGamePage : Page
    {
        private List<int> _largeNumLst;
        private List<int> _smallNumLst;

        private List<int> _tempLrgNumLst;
        private List<int> _tempSmlNumLst;

        private List<Button> _tempNumBtnAvail = new List<Button>();
        private List<Button> _btnNumLst = new List<Button>();
        private List<Button> _btnOpLst = new List<Button>();

        private int _rndNum = 0, _btnLoc = 0, _cntPlys = 0, _currTotal = 0, _curNum = 0, _targetNum = 0;

        private string _operatorUsed = "";

        private DispatcherTimer _countDownTimer = new DispatcherTimer();
        private int _countTicks = 28;

        public NumberGamePage()
        {
            this.InitializeComponent();

            fillBtnLists();

            //Populate the lists with associated numbers, Default List
            _largeNumLst = new List<int> { 25, 50, 75, 100 };
            _smallNumLst = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };

            //Create a copy of the lists above, Temp List
            _tempLrgNumLst = new List<int>(_largeNumLst);
            _tempSmlNumLst = new List<int>(_smallNumLst);

            setClockMarkers();
        }


        /* Get all the child buttons In stkPanNum and stkPanOperators
        * and add them to a List<Buttons>
        */
        private void fillBtnLists()
        {
            foreach (Button b in stkPanNum.Children)
            {
                _btnNumLst.Add(b);
            }

            foreach (Button b in stkPanOperators.Children)
            {
                _btnOpLst.Add(b);
            }
        }

        /* Enable all Buttons so the user can click them
        */
        private void enableAllBtns(List<Button> btnList)
        {
            foreach (Button b in btnList)
            {
                b.IsEnabled = true;
                b.Opacity = 1;
            }
        }

        /*Disables all Buttons so the user cant click them
        */
        private void disableAllBtns(List<Button> btnList)
        {
            foreach (Button b in btnList)
            {
                b.IsEnabled = false;
                b.Opacity = 1;
            }
        }

        /* Creates a temp list from either "_tempLrgNumLst" or "_tempSmlNumLst"
        *  based on the Sender and the buttons Name propriety, using btnSmallNum as default.
        *  then Picks a random element from that temp list,
        *  Also only alows the user to add 4 unique large numbers/ num from _tempSmlNumLst.
        */
        private void NumChoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            List<int> tempArray;
            int max;

            if (b.Name.Equals("btnLargeNum"))
            {
                max = _tempLrgNumLst.Count;
                tempArray = _tempLrgNumLst;
            }
            else {
                max = _tempSmlNumLst.Count;
                tempArray = _tempSmlNumLst;
            }

            _rndNum = rndNumGen(0, max);//get a rnd num
            sendNumToBtn(tempArray[_rndNum]);//send num to Btn

            //Removes the element in the list based on which btn was clicked
            if (b.Name.Equals("btnLargeNum"))
                _tempLrgNumLst.RemoveAt(_rndNum);

            else
                _tempSmlNumLst.RemoveAt(_rndNum);

            //Hide the btn btnLargeNum when all 4 numbers are drawn
            if (_tempLrgNumLst.Count == 0)
                btnLargeNum.IsEnabled = false;
        }

        /*Used to send the numbers from the Large/Small num btns to the appropriate btnNumBox    
        */
        private void sendNumToBtn(int num)
        {
            //Sends the Number to the Btn.content
            _btnNumLst[_btnLoc].Content = num.ToString();

            //If all numbers are added to the buttons
            if (_btnLoc == _btnNumLst.Count - 1)
            {
                enableAllBtns(_btnNumLst);
                _tempNumBtnAvail.AddRange(_btnNumLst);//Populate the temp Btn slist

                stkPanNumBtns.Visibility = Visibility.Collapsed;
                stkPanOperators.Visibility = Visibility.Visible;
                stkPanReset.Visibility = Visibility.Visible;

                /////////////////////*********************************
                _targetNum = rndNumGen(101, 999);
                txtBoxTarget.Text = _targetNum.ToString();

                startTimer();
                ShowStoryboardAnimation.Begin();//Clock animation

                _btnLoc = 0;//Reset the btn Location
            }
            else
                _btnLoc++;//Used to fnd the next button
        }

        /*Generates a number between 0 and the number passed in.
        */
        private int rndNumGen(int minNo, int maxNo)
        {
            Random r = new Random();
            return r.Next(minNo, maxNo);
        }

        /* Method is called from Enter btn click event and is use to get the diffrence between the total the user
        * got to from the Target Number.
        * The users Score is then calculated based on how close they were from the target;
        * 0  away = 10 points
        * 5  away = 7 points
        * 10 away = 5 points
        */
        private void BtnResults_Click(object sender, RoutedEventArgs e)
        {
            //Take the _currTotal from user - the Target to get cal score 
            int totalAway = Math.Abs(_targetNum - _currTotal);
            int score = 0;
            //Get the Total from target and add core to App._UserScore
            if (totalAway == 0)
            {
                App._UserScore += 10;
                score = 10;
            }

            else if (totalAway <= 5)
            {
                App._UserScore += 7;
                score = 7;
            }

            else if (totalAway <= 10)
            {
                App._UserScore += 5;
                score = 5;
            }
            showScoreBoard(totalAway, score);

        }

        /*Displayes the ScoreBoard
        */
        private void showScoreBoard(int totAway, int score)
        {
            NumAwayTxtBox.Text = "You Were " + totAway + " Away! Score: " + score;
            SpLayout.Visibility = Visibility.Collapsed;
            SpResultsLayout.Visibility = Visibility.Visible;
            UsrScoreTxtBox.Text = Convert.ToString(App._UserScore);
        }

        /*Resets all the Fields Lised Below so the user can have another try (Timer not reset)
        */
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtBoxMathTotals.Text = "";
            txtBoxMathsSigns.Text = "";
            txtBoxMathsNum.Text = "";
            txtBoxMathsEquals.Text = "";
            txtBoxMathTotal.Text = "";

            txtBoxTotal.Text = "Total";
            enableAllBtns(_btnNumLst);
            disableAllBtns(_btnOpLst);

            _tempLrgNumLst = _largeNumLst;
            _tempSmlNumLst = _smallNumLst;
            _tempNumBtnAvail.Clear();

            _tempNumBtnAvail.AddRange(_btnNumLst);//Copy contents from one list to another (reset)

            _cntPlys = 0;
            _currTotal = 0;
        }
        
        /*Method gets the Sender as a button, takes its contents and sets/hides settings below
        */
        private void AddBtnContToTxtBox_Click(object sender, RoutedEventArgs e)
        {
            //Append the number onto a string eg "34"
            Button clickedButton = sender as Button;
            StackPanel sp = clickedButton.Parent as StackPanel;//Gets the buttons Parent stackPanel
            string btnContents = clickedButton.Content.ToString();

            //Add the contents of that button into the Textbox
            //txtBoxMaths.Text += (btnContents).PadRight(7);

            //if the _cntPlys is != maxplays Keep adding btn contents to other btn 
            if (!((_cntPlys++) > (10)))
            {
                //If the click event came from the Child elements in the stack pannel stkPanNum
                if (sp.Name.Equals("stkPanNum"))
                {
                    if (!((_cntPlys) > (10)))
                        enableAllBtns(_btnOpLst);

                    disableAllBtns(_tempNumBtnAvail);
                    _tempNumBtnAvail.Remove(clickedButton);

                    _curNum = Convert.ToInt16(clickedButton.Content);

                    if (_cntPlys == 1)
                        txtBoxMathTotals.Text += (btnContents + "\n");
                    else
                        txtBoxMathsNum.Text += (btnContents + "\n");
                }
                else//else the button click event came from the operators stackPanal 
                {
                    enableAllBtns(_tempNumBtnAvail);
                    disableAllBtns(_btnOpLst);

                    if ((_currTotal == 0))
                        _currTotal = _curNum;

                    if (btnContents.Equals("÷"))
                        hideUnDivisibleBtns();

                    _operatorUsed = btnContents;

                    txtBoxMathsSigns.Text += (btnContents+"\n");
                }
            }
            else {
                disableAllBtns(_btnNumLst);
                disableAllBtns(_btnOpLst);
            }

            //Calculate Total after the user has clicked on 2 numbers and 1 operator, and after each following operator thereafter
            if (((_cntPlys < 4) && (_cntPlys % 3 == 0)) || ((_cntPlys > 3) && (_cntPlys % 2 == 1)))
                printCalculations(btnContents);

            clickedButton.Opacity = .5;
            clickedButton.IsEnabled = false;
        }

        /*Prints the calculations fron the user to the Maths textbox
        */
        private void printCalculations(String btnContents)
        {
            _currTotal = calculateTotal(_operatorUsed, _currTotal, _curNum);

            txtBoxMathTotals.Text += (_currTotal.ToString() + "\n");
            txtBoxMathsEquals.Text += ("=\n");
            txtBoxMathTotal.Text += (_currTotal.ToString() + "\n");

            txtBoxTotal.Text = _currTotal.ToString();

        }

        /*Calculates the Total based on what operator was used
        */
        private int calculateTotal(string op, int num1, int num2)
        {
            int total = 0;

            //if 1st number is < the 2nd. swap positions to avoid negitive values when using ÷ or -
            if (num1 < num2)
            {
                int temp = num1;
                num1 = num2;
                num2 = temp;
            }
            switch (op)
            {
                case "+": return (total = (num1 + num2));
                case "-": return (total = (num1 - num2));
                case "*": return (total = (num1 * num2));
                case "÷": return (total = (num1 / num2));
                default: throw new Exception("Invalid Logic");
            }
        }

        /*Gets the index of each button and divides its contents against the value
        * the user picked first or the _total, then checks to see if it divides evenly
        * if not, and the answer is a fraction, disable the button.
        */
        private void hideUnDivisibleBtns()
        {
            int btnContent;
            double chkDbl;
            int numToDivide = _currTotal;

            foreach (Button b in _btnNumLst)
            {
                btnContent = Convert.ToInt16(b.Content);

                //Swap the numbers around so they divide properarly if wrong
                if (btnContent < numToDivide)
                {
                    int temp = btnContent;
                    btnContent = numToDivide;
                    numToDivide = temp;
                }

                chkDbl = ((double)btnContent / numToDivide);
                //resets the numToDivide var back to the first chosen number to divide 
                numToDivide = _currTotal;

                //Hides all buttons that dont divide evenly not leaving a fraction
                if (!(chkDbl % 1 == 0))
                {
                    b.IsEnabled = false;
                }
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
            _countTicks--;
            if (_countTicks == -1)
            {
                stkPanNumBtns.Visibility = Visibility.Collapsed;
                stkPanNum.Visibility = Visibility.Collapsed;
                stkPanOperators.Visibility = Visibility.Collapsed;
                stkPanReset.Visibility = Visibility.Collapsed;

                SPResultsBtn.Visibility = Visibility.Visible;

                _countDownTimer.Stop();
                _countDownTimer.Tick -= numTimer_Tick;
            }
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

                double radius = 55;

                //Sets the spacing between each of the numbers
                double angle = Math.PI * i * 30.0 / 180.0;

                //sets the x and y positions of the Circle on the canvas
                double xPos = Math.Sin(angle) * radius + radius;
                double yPos = -Math.Cos(angle) * radius + radius; //-Math.Cos important

                Canvas.SetLeft(tb, xPos);
                Canvas.SetTop(tb, yPos);

                //adds the TextBlocks to the canvas as a child element
                MarkersCanvas.Children.Add(tb);
            }
        }

        /*Navigates the use to another page
        */
        private void BtnNxtLvl_Click(object sender, RoutedEventArgs e)
        {
            //if the game is over
            if (--App._NumOfGames < 1)
            {
                App._GameOver = true;               
                Frame.Navigate(typeof(ScoreBoardGamePage));
            }
            else
                Frame.Navigate(typeof(WordGamePage));
        }

        /*Quit Button
        */
        private void BtnQuitLvl_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

    }
}
