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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NumberGamePage : Page
    {
        private List<int> _largeNumLst;
        private List<int> _smallNumLst;

        private List<int> _tempLrgNumLst;
        private List<int> _tempSmlNumLst;

        private List<Button> _btnNumLst = new List<Button>();
        private List<Button> _btnOpLst = new List<Button>();

        private int _rndNum = 0, _btnLoc = 0, _cntPlys = 0, _total = 0 , _curNum = 0,targetNum=0;

        private string _operatorUsed = "";


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

            _rndNum = rndNumGen(0,max);//get a rnd num
            sendNumToBtn(tempArray[_rndNum]);//send num to Btn

            if (b.Name.Equals("btnLargeNum"))
                _tempLrgNumLst.RemoveAt(_rndNum);

            else
                _tempSmlNumLst.RemoveAt(_rndNum);

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

                stkPanNumBtns.Visibility = Visibility.Collapsed;
                stkPanOperators.Visibility = Visibility.Visible;
                stkPanReset.Visibility = Visibility.Visible;



                /////////////////////*********************************
                targetNum = rndNumGen(101, 999);
                txtBoxTarget.Text = targetNum.ToString();

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
            return r.Next(minNo,maxNo);
        }



        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtBoxResult.Text = "";
            enableAllBtns(_btnNumLst);
            disableAllBtns(_btnOpLst);

            _tempLrgNumLst = _largeNumLst;
            _tempSmlNumLst = _smallNumLst;

            _cntPlys = 0;
            _total = 0;
        }


        private void resetGame(object sender, RoutedEventArgs e)
        {
            foreach (Button b in _btnNumLst)
            {
                b.Content = "";
            }
            _btnLoc = 0;
        }














        //Get the number
        private void AddBtnContToTxtBox_Click(object sender, RoutedEventArgs e)
        {
            //Append the number onto a string eg "34"
            Button clickedButton = sender as Button;
            StackPanel sp = clickedButton.Parent as StackPanel;//Gets the buttons Parent stackPanel
            string btnContents = clickedButton.Content.ToString();
            
            //Add the contents of that button into the Textbox
            txtBoxResult.Text += btnContents;

            //if the countPlays is == maxplays all numbers have been played **Fix**
            if (!((_cntPlys++) >= (10)))
            {
                if (sp.Name.Equals("stkPanNum"))
                {
                    enableAllBtns(_btnOpLst);
                    disableAllBtns(_btnNumLst);

                    _curNum = Convert.ToInt16(clickedButton.Content);

                    //Makes sure its not the first time and that it takes the total from the next number pressed
                    if (_cntPlys > 3 && _cntPlys % 2 == 1)
                    {
                        _total = calculateTotal(_operatorUsed,_total,_curNum);
                        txtBoxResult.Text += (" = " + _total + "\n");
                    }
                    
                }
                else//if the button click event came from the operators stackPanal 
                {
                    enableAllBtns(_btnNumLst);
                    disableAllBtns(_btnOpLst);

                    if ((_total == 0))
                        _total = _curNum;

                    if (btnContents.Equals("÷"))
                        hideUndivideabeBtns();

                    _operatorUsed = btnContents;
                }

                //Do calculation once off
                if ((_cntPlys < 4) && (_cntPlys % 3 == 0))
                {
                    _total = calculateTotal(_operatorUsed, _total, _curNum);
                    txtBoxResult.Text += (" = " + _total + "\n");
                }

            }
            else{
                _total = calculateTotal(_operatorUsed, _total, _curNum);
                txtBoxResult.Text += (" = " + _total + "\n");

                disableAllBtns(_btnNumLst);
                disableAllBtns(_btnOpLst);
            }

            clickedButton.Opacity = .5;
            clickedButton.IsEnabled = false;
        }








        /*Method calculates and returns the total to the caller from 2 
        * numbers and the associated operator. 
        */
        private int calculateTotal(string op,int num1, int num2)
        {
            int total = 0;

            //if 1st number is < the 2nd. swap positions to avoid negitive values etc
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
        private void hideUndivideabeBtns()
        {
            int btnContent;
            double chkDbl;
            int numToDivide = _total;
      
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
                numToDivide = _total;

                //Hides all buttons that dont divide evenly not leaving a fraction
                if (!(chkDbl % 1 == 0))
                {
                    b.IsEnabled = false;
                }
            }
        }





    }
}
