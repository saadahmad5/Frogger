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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frogger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOver : Page
    {
        public GameOver()
        {
            this.InitializeComponent();
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Main_Page));
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Credits));
        }

        public void ShowScore(score score)
        {
            score displayScore = score;
            Canvas canvas = new Canvas();
            TextBlock text = new TextBlock();
            text.Text = $"SCORE: {displayScore.Score}";
            Canvas.SetTop(text, 300);
            Canvas.SetLeft(text, 200);
        }
    }
}
