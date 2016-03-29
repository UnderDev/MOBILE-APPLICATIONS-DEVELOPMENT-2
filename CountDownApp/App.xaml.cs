using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CountDownApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static bool _checkLoaded { get; private set; }
        public static List<string> _wordsList { get; private set; } = new List<string>();
        public static List<TextBox> _TextBoxScores { get; private set; } = new List<TextBox>(10);

        public static Dictionary<string, int> _UserScoreBoard { get; set; } = new Dictionary<string, int>();

        public static int _UserScore { get; set; }
        public static int _NumOfGames = 2;
        public static bool _GameOver = false;

        private static StorageFolder folderRoaming = ApplicationData.Current.RoamingFolder;
        private static StorageFolder folderLocal = ApplicationData.Current.LocalFolder;
        private static StorageFile fileLocal;
        private static string fileName = "ScoreBoard.txt";
        private static string fileContents="";





        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;

        }

        //reads from the file line by line into a List<String>
        private static async void ReadFromFile()
        {
            StorageFile _file = await StorageFile.GetFileFromApplicationUriAsync(new System.Uri(@"ms-appx:///Files/words.txt"));

            using (StreamReader reader = new StreamReader(await _file.OpenStreamForReadAsync()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _wordsList.Add(line); // Add to list line by line
                }
            }
            _checkLoaded = true;//check its all read in and finished
        }//end

        public static async void load()
        {
            //w8 till ReadFromFile() and ScoreBoardFileIO() is finished
            await Task.Run(() => ReadFromFile());
            await Task.Run(() => CreateReadFile());
        }//load



        public static async void CreateReadFile()
        {
            Boolean filePresent = false;
            /*
             * Try to create a file from (App.fileName)
             * Write to that file whats stored in var fileContents
            */
            try
            {
                fileLocal = await folderLocal.CreateFileAsync(fileName);
                filePresent = false;
                // await FileIO.WriteTextAsync(fileLocal, fileContents);
            }
            //If file is there throw an exception
            catch (Exception)
            {
                filePresent = true;
            }


            if (filePresent)
            {
                //Get the file and read in its contents, and store in var textLocal
                fileLocal = await folderLocal.GetFileAsync(fileName);
                string textLocal = await FileIO.ReadTextAsync(fileLocal);

                string[] splitStr;
                splitStr = textLocal.Split(',');
                _UserScoreBoard.Clear();

                for (int i = 0; i <= splitStr.Length - 2; i += 2)
                {
                    if(!(splitStr[i].Equals(" ")))
                    _UserScoreBoard.Add(splitStr[i], Convert.ToUInt16(splitStr[i + 1]));
                }
            }
        }


        public static async void WriteToFile(string newScoreBoard)
        {
               await FileIO.WriteTextAsync(fileLocal, newScoreBoard);               
        }



        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific _file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
