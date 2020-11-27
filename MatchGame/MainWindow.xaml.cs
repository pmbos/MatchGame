using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock lastTextBlockClicked; //The last text block to be clicked
        bool findingMatch = false; //Indicator whether we are looking for a match or not
        readonly DispatcherTimer timer = new DispatcherTimer(); //A timer
        int tenthsOfSecondsElapsed; //The number of tenths of seconds that have passed since the timer was started
        int matchesFound; //The number of matches we found

        public MainWindow()
        {
            InitializeComponent();

            //Define a the timer interval as tenths of seconds and subscribe to its Tick event.
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            //Set up the game
            SetUpGame();
        }

        /// <summary>
        /// Handle the timer's Tick event. Stop the timer once 8 matches are found.
        /// </summary>
        /// <param name="sender">The object emitting the event</param>
        /// <param name="e">The event arguments</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //Increase the tenths of seconds that have passed and display how many seconds have passed in total.
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");

            //If 8 matches were found
            if (matchesFound == 8)
            {
                //Stop the timer and indicate to the player that they can click to play again.
                timer.Stop();
                timeTextBlock.Text += " - Click to play again";
            }
        }

        /// <summary>
        /// Set up the game.
        /// </summary>
        private void SetUpGame()
        {
            List<string> animalEmojis = new List<string> //The emoji to use in the game
            {
                "🎅", "🎅",
                "🤴", "🤴",
                "🧛‍", "🧛‍",
                "🧙‍", "🧙‍",
                "🎃", "🎃",
                "🎄", "🎄",
                "🎆", "🎆",
                "💎", "💎",
            };

            Random random = new Random(); //A random generator

            //For each text block whose name is not 'timeTextBlock'.
            foreach (TextBlock textBlock in 
                mainGrid.Children.OfType<TextBlock>()
                .Where((textBlock) => textBlock.Name != "timeTextBlock"))
            {
                //Get a random index of the emoji list and retrieve its emoji
                int index = random.Next(animalEmojis.Count);
                string nextEmoji = animalEmojis[index];
                //Display the emoji and remove it from the list.
                textBlock.Text = nextEmoji;
                animalEmojis.RemoveAt(index);

                //Make the text block visible if it was hidden in the past.
                if (textBlock.Visibility == Visibility.Hidden)
                    textBlock.Visibility = Visibility.Visible;
            }

            //Initialise our records and start the timer.
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
            timer.Start();
        }

        /// <summary>
        /// Handle mouse down events on text blocks (user clicks on emoji)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Register the sender as the text block we clicked on
            var currentTextBlock = sender as TextBlock;
            //If we are not looking for a match
            if (!findingMatch)
            {
                //Hide the current text block, register it as the last one we clicked on and indicate that we
                //are looking for a match
                currentTextBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = currentTextBlock;
                findingMatch = true;
            }
            //Else, if we are looking for a match and the current text block matches the last one
            else if (currentTextBlock.Text == lastTextBlockClicked.Text)
            {
                //Hide the current one and indicate that we found a match
                currentTextBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
                matchesFound++;
            }
            //Otherwise, we failed to find a match
            else
            {
                //Bring back the last text block we clicked on and indicate that we want to
                //stop looking for matches
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        /// <summary>
        /// Handle mouse down events on the time text block (clicks on the timer).
        /// Restart the game if 8 matches were found.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
                SetUpGame();
        }
    }
}
