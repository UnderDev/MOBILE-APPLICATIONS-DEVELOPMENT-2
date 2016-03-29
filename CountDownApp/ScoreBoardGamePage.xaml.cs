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
    public sealed partial class ScoreBoardGamePage : Page
    {
        private const int _MAX_SCORE_BOARD = 9;

        public ScoreBoardGamePage()
        {
            this.InitializeComponent();

            SwitchContent();
        }

        private void SwitchContent()
        {
            if (App._GameOver)
                DisplayNewHighScore();

            else {
                DisplayScoreBoard();
                CreateTextBoxes();
                FillScoreBoard();
            }
        }

        private void DisplayScoreBoard()
        {
            SpScoreBoardContent.Visibility = Visibility.Visible;
            SpScoreBoardHighScore.Visibility = Visibility.Collapsed;
        }

        private void DisplayNewHighScore()
        {
            SpScoreBoardContent.Visibility = Visibility.Collapsed;
            SpScoreBoardHighScore.Visibility = Visibility.Visible;
            UsrScoreTxtBox.Text = Convert.ToString(App._UserScore);
        }

        
        private static void CreateTextBoxes()
        {
            App._TextBoxScores.Clear();
            //For Each Pair <Key,Value> create a textbox and add its contents
            foreach (var pair in App._UserScoreBoard)
            {
                if (!pair.Value.Equals(" "))
                {

                    TextBox tb = new TextBox();
                    tb.IsReadOnly = true;

                    tb.Style = Application.Current.Resources["ScoreBoardContent"] as Style;

                    tb.Text = pair.Value.ToString().PadRight(14, '-')+ pair.Key.ToUpper();
                    App._TextBoxScores.Add(tb);
                }
            }
        }

        /*For each TextBox In the List _TextBoxScores Add it to the StackPannel as a child element
        */
        private void FillScoreBoard()
        {
            foreach (var tb in App._TextBoxScores)
            {
                SPScoreBoard.Children.Add(tb);
            }
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            String ScoreBordWrite="";
            //Check to see that the name entered is unique
            if (App._UserScoreBoard.ContainsKey(TbUserName.Text))
            {
                TbUserName.Text = "Name In Use, Try Another";
            }
            else {
                ChkForNewHighScore();
                CreateTextBoxes();
                //Display the ScoreBoard
                FillScoreBoard();
                DisplayScoreBoard();
                foreach (var result in App._UserScoreBoard)
                {
                    ScoreBordWrite += result.Key + "," + result.Value + ",";
                }
                App.WriteToFile(ScoreBordWrite);
            }
        }

        private void ChkForNewHighScore()
        {
            Boolean highScoreFound = false;
            foreach (var score in App._UserScoreBoard)
            {
                if (App._UserScore >= Convert.ToUInt16(score.Value) || App._UserScoreBoard.Count <= _MAX_SCORE_BOARD)
                    highScoreFound = true;

            }

            if (highScoreFound || App._UserScoreBoard.Count == 0)
            {
                App._UserScoreBoard.Add(TbUserName.Text, App._UserScore);
                Dictionary<string, int> temp = new Dictionary<string, int>();

                //Uses a lambda operator to get the Value from the dictonary ordered by lowest num first 
                foreach (var results in App._UserScoreBoard.OrderByDescending(key => key.Value))
                {
                    temp.Add(results.Key, results.Value);
                    if (temp.Count > _MAX_SCORE_BOARD)
                        temp.Remove(results.Key);
                }
                App._UserScoreBoard.Clear();
                App._UserScoreBoard = temp;
               

            }
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
           App._GameOver = false;
        }

        private void TbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            TbUserName.Text = "";
        }

        private void TbUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TbUserName.Text.Equals(""))
            TbUserName.Text = "Enter Your name Here";
        }
    }
}